extends CanvasLayer

onready var fps_label = $FPS

func _process(_delta):
	fps_label.set_text(str(Engine.get_frames_per_second()))
