extends Node


onready var Animations = get_node("../..//Sprites/AnimationPlayer")
onready var Player = get_parent()


func _physics_process(_delta: float) -> void:
	_process_animations()


func _process_animations() -> void:
	var player = instance_from_id(Server.player_instance_id)
	if not player:
		return
	
	if get_node("../../").is_on_floor():
		if Player.velocity != Vector2.ZERO:
			Animations.play("knight_walk")
		else:
			Animations.play("knight_idle")
	else:
		if Player.velocity.y < 0:
			Animations.play("knight_jump")
		else:
			Animations.play("knight_fall")
			# Ideally this wouldn't be hard coded but I don't think the animation player node can do this
			if Player.get_node("../Sprites").scale.x == -1: 
				Player.get_node("../Sprites/Head").position = Vector2(-1, -4)
