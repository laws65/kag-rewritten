extends Camera2D

var zoom_modifier = false
var zoom_modifier_level = 4
var zoom_level = 1

var target

func _ready():
	Globals.game_camera = self

var tick = 0.0
func _physics_process(delta):
	if is_instance_valid(target):
		global_position = target.global_position

	var zoom_target = 1.0
	if zoom_modifier:
		match zoom_modifier_level:
			0:
				zoom_target = 0.5
				zoom_level = 0
			1:
				zoom_target = 0.5625
				zoom_level = 0
			2:
				zoom_target = 0.625
				zoom_level = 0
			3:
				zoom_target = 0.75
				zoom_level = 0
			4:
				zoom_target = 1.0
				zoom_level = 1
			5:
				zoom_target = 1.5
				zoom_level = 1
			6:
				zoom_target = 2.0
				zoom_level = 2
	else:
		match zoom_level:
			0:
				zoom_target = 0.5
				zoom_modifier_level = 0
			1:
				zoom_target = 1.0
				zoom_modifier_level = 4
			2:
				zoom_target = 2.0
				zoom_modifier_level = 6

	tick += delta
	zoom = zoom.linear_interpolate(Vector2(zoom_target / 2.0, zoom_target / 2.0), tick)

func _unhandled_input(event):
	if event.is_pressed() and event is InputEventMouseButton:
		if event.button_index == BUTTON_WHEEL_UP:
			tick = 0.0
			zoom_modifier = Input.is_key_pressed(KEY_CONTROL)

			zoom_level = clamp(zoom_level - 1, 0, 2)
			zoom_modifier_level = clamp(zoom_modifier_level - 1, 0, 6)

		if event.button_index == BUTTON_WHEEL_DOWN:
			tick = 0.0
			zoom_modifier = Input.is_key_pressed(KEY_CONTROL)

			zoom_level = clamp(zoom_level + 1, 0, 2)
			zoom_modifier_level = clamp(zoom_modifier_level + 1, 0, 6)
