extends Node2D

onready var anims = $AnimationPlayer
var offset = float(19)

func _ready():
	pass
	
func init(o_tr, o_pos):
	global_transform = o_tr
	global_position = Vector2(o_pos.x, o_pos.y - offset)
	anims.play("Play")

func destroy():
	queue_free()
