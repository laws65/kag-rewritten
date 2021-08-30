extends KinematicBody2D
class_name Entity

var team: int
var blob_owner: int

export var pickable : bool = false
var blob_holding_me: Entity
var is_picked_up = false

export var player_logic_scene: PackedScene


func _physics_process(_delta: float) -> void:
	if is_picked_up:
		position = blob_holding_me.get_node("AttatchmentPoint").position


func set_blob_ownership(player_id: int) -> void:
	blob_owner = player_id
	if blob_owner == get_tree().get_network_unique_id():
		if not has_node("PlayerLogic"):
			var player_logic_instance = player_logic_scene.instance()
			add_child(player_logic_instance)


func set_team(team: int) -> void:
	self.team = team
	get_node("Sprites").get_material().set_shader_param("should_swap", bool(team))


func initialise(data: Dictionary) -> void:
	name = str(data["id"])
	position = data["spawnpoint"]
	set_team(data["team"])


func get_picked_up(data: Array) -> void:
	if has_node("../" + str(data[0])):
		is_picked_up = true
		blob_holding_me = get_node("../" + str(data[0]))
