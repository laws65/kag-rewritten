extends Node2D

onready var o_spr = $Sprite
onready var o_ani = $AnimationPlayer

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("ui_right"):
		o_spr.frame += 4
		set_position(Vector2(9,-2))

func _updatepos(moveLeft, moveRight, crouching, jumping):
	if (jumping):
		if (o_spr.flip_h == true):
			o_ani.play("jumpleft")
		else:
			o_ani.play("jumpright")
	elif (moveLeft):
		o_spr.set_flip_h(true)
		o_spr.set_offset(Vector2(-2,0))
		o_ani.play("walkleft")
	elif (moveRight):
		o_spr.set_flip_h(false)
		o_spr.set_offset(Vector2(0,0))
		o_ani.play("walkright")
	elif (crouching):
		o_ani.play("crouch")
	elif (!moveLeft && !moveRight && !crouching && !jumping):
		o_ani.play("idle")

