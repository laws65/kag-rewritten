extends Camera2D


var PlayerPath: NodePath = "../YSort/Player"


func _physics_process(delta: float) -> void:
	zoom = _calculate_zoom()
	if Server.is_spectating:
		_spectator_movement(delta)
	elif has_node(PlayerPath):
		position = _calculate_position()


func _calculate_zoom() -> Vector2:
	var camera_zoom := zoom
	
	if Input.is_action_pressed("zoom_in"):
		camera_zoom -= Vector2(0.1, 0.1)
	elif Input.is_action_pressed("zoom_out"):
		camera_zoom += Vector2(0.1, 0.1)
	
	camera_zoom.x = clamp(camera_zoom.x, 0.7, 2.5)
	camera_zoom.y = clamp(camera_zoom.y, 0.7, 2.5)
	
	return camera_zoom


func _calculate_position() -> Vector2:
	var camera_position := position
	
	camera_position.x = lerp(camera_position.x, get_node(PlayerPath).position.x, 0.5)
	camera_position.y = lerp(camera_position.y, get_node(PlayerPath).position.y, 0.5)
	
	return camera_position


func _spectator_movement(delta: float) -> void:
	var direction = get_node("InputDirection").get_input_direction().normalized()
	position += direction*delta*250
	position = Vector2(int(position.x), int(position.y))
