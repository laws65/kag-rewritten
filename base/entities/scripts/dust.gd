extends Node2D

onready var anims = $AnimationPlayer

func _ready():
	pass
	
func init(pose, posy):
	global_transform = pose
	global_position = Vector2(posy.x, posy.y - float(19))
	anims.play("Play")

func destroy():
	queue_free()
