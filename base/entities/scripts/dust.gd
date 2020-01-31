extends Node2D

onready var anims = $AnimationPlayer

func _ready():
	anims.connect("finished", self, "destroy")
	anims.play("play")

func destroy():
	queue_free()
