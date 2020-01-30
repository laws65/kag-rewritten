extends Camera2D

var target

func _process(delta):
	if target:
		global_position = target.global_position