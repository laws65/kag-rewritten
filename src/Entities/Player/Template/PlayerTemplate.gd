extends KinematicBody2D


func update_info(new_position: Vector2, animation: String, facing: int) -> void:
	global_position = new_position
	get_node("Sprites/AnimationPlayer").play(animation)
	get_node("Sprites").scale.x = facing
