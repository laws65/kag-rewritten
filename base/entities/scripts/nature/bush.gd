extends Node2D

onready var c_sprite = $Sprite

export (Color) var representative_color = Color(0, 0, 0, 1)

func _ready():
	var rng = RandomNumberGenerator.new()
	rng.randomize()
	c_sprite.frame = rng.randi_range(0, 7)
