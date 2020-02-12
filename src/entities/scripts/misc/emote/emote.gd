extends Sprite

# These don't include the emote when chatting
signal emote_started()
signal emote_ended()

export (Array, Texture) var emote_array = []
export (Texture) var chat_emote

export (float) var duration = 2.0
export (float) var anim_speed = 0.2 setget _set_anim_speed
export (float) var sway_amplitude = 6.0
export (float) var sway_frequency = 4.0

onready var timer = $Timer
onready var animation = $Animation

func _ready():
	visible = false
	animation.playback_speed = 1 / anim_speed

	if is_network_master():
		game_chat.connect("chat_opened", self, "_on_chat_opened")
		game_chat.connect("chat_closed", self, "_on_chat_closed")
	else:
		set_process_unhandled_input(false)

var _tick = 0
func _process(delta: float):
	_tick += delta
	self.rotation_degrees = sin(_tick * sway_frequency) * sway_amplitude

func _unhandled_input(event):
	for index in emote_array.size():
		if Input.is_action_just_pressed("emote_" + str(index + 1)):
			rpc("_show_emote", index)

### Main behavior
puppetsync func _show_emote(index):
	if index < 0 || index >= emote_array.size():
		return

	emit_signal("emote_started")

	set_texture(emote_array[index])
	if !visible:
		animation.play("show")

	timer.start(duration)
	yield (timer, "timeout")

	_hide_emote()

func _hide_emote():
	animation.play("hide")
	emit_signal("emote_ended")
### ---

### Specific to chat
func _on_chat_opened():
	rpc("_show_chat_emote")

func _on_chat_closed():
	rpc("_hide_chat_emote")

puppetsync func _show_chat_emote():
	set_texture(chat_emote)
	animation.play("show")

puppetsync func _hide_chat_emote():
	animation.play("hide")
### ---

func _set_anim_speed(value):
	anim_speed = value
	if self.has_node("Animation"):
		animation.playback_speed = 1 / anim_speed
