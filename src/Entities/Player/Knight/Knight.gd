extends Node2D


onready var Animations = get_node("../Sprites/AnimationPlayer")
onready var Player = get_parent()


func _process(delta: float) -> void:
	_process_animations()


func _process_animations() -> void:
	if Player.is_on_floor():
		if Player.velocity != Vector2.ZERO:
			Animations.play("walk")
		else:
			Animations.play("idle")
	else:
		if Player.velocity.y < 0:
			Animations.play("jump")
		else:
			Animations.play("fall")
			# Ideally this wouldn't be hard coded but I don't think the animation player node can do this
			if Player.get_node("Sprites/Head").flip_h: 
				Player.get_node("Sprites/Head").position = Vector2(-1, -4)
