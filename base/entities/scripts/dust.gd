extends Node2D

onready var c_anim = $Animation

func _ready():
	c_anim.connect("animation_finished", self, "destroy")
	c_anim.play("play")

func destroy(animation):
	queue_free()
