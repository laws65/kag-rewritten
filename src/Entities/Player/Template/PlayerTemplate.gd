extends KinematicBody2D


func update_info(new_position, animation, facing) -> void:
	global_position = new_position
	get_node("Sprites/AnimationPlayer").play(animation)
	get_node("Sprites/Body").flip_h = facing
	get_node("Sprites/Head").flip_h = facing
