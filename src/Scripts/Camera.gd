extends Camera2D


export var PlayerPath: NodePath
onready var Player = get_node(PlayerPath)


func _physics_process(_delta: float) -> void:
	zoom = _calculate_zoom()
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
	
	camera_position.x = lerp(camera_position.x, Player.position.x, 0.5)
	camera_position.y = lerp(camera_position.y, Player.position.y, 0.5)
	
	return camera_position
	
