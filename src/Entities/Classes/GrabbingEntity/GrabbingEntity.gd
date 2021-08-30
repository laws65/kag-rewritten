extends Entity
class_name GrabbingEntity

var held_blob: Entity


func _input(event: InputEvent) -> void:
	if not event.is_action_pressed("pickup"):
		return
	if held_blob:
		Server.call_on_blob(int(self.name), "throw_held_blob")
	for area in get_node("GrabbingRange").get_overlapping_areas():
		if area.get_parent() is Entity and area.get_parent().pickable:
			Server.call_on_blob(int(self.name), "pickup_blob", [int(area.get_parent().name)])
			Server.call_on_blob(int(area.get_parent().name), "get_picked_up", [int(self.name)])


func pickup_blob(data: Array) -> void:
	held_blob = data[0]


func throw_held_blob() -> void:
	held_blob.blob_holding_me = null
	held_blob.is_picked_up = false
	
