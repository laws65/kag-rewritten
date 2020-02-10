extends Sprite

# These don't include the emote when chatting
signal emote_started(emote_index)
signal emote_ended(emote_index)

export var emote_duration = 2.0
export var emote_anim_speed = 0.2 setget _set_emote_anim_speed
export var sway_amplitude = 6.0
export var sway_frequency = 4.0

onready var timer = get_node("Timer")
onready var animation = get_node("Animation")

var _tick = 0
var _chat_open = false

enum Emotes {
	SKULL,
	BLUE_FLAG,
	NOTE,
	RIGHT,
	SMILE,
	RED_FLAG,
	FLEX,
	DOWN,
	FROWN,
	TROLL,
	MIDDLE_FINGER,
	LEFT,
	ANNOYED,
	ARCHER,
	SWEAT,
	UP,
	LAUGH,
	KNIGHT,
	QUESTION,
	THUMBS_UP,
	CONFUSED,
	BUILDER,
	MAD,
	THUMBS_DOWN,
	DROOL,
	LADDER,
	ATTENTION,
	PERFECT,
	CRY,
	WALL,
	HEART,
	FIRE,
	WINK,
	COOL,
	DOTS,
	COG,
	THINK,
	JOY,
	DERP,
	AWKWARD,
	SMUG,
	LOVE,
	KISS,
	PICKUP,
	RAISED,
	CLAP
}

var emote_names = [
	"Skull",
	"Blue flag",
	"Musical note",
	"Point right",
	"Smile",
	"Red flag",
	"Flex",
	"Point down",
	"Frown",
	"Troll",
	"Rude gesture",
	"Point left",
	"Annoyed",
	"Archer",
	"Sweat drop",
	"Point up",
	"Laugh",
	"Knight",
	"Question mark",
	"Thumbs up",
	"Confused",
	"Builder",
	"Mad",
	"Thumbs down",
	"Drool",
	"Ladder",
	"Attention",
	"Perfect",
	"Cry",
	"Wall",
	"Heart",
	"Fire",
	"Wink",
	"Cool",
	"Speaking",
	"Cog",
	"Think",
	"Joy",
	"Derp",
	"Awkward laugh",
	"Smug",
	"Love",
	"Kiss",
	"Pickup",
	"Raised eyebrow",
	"Clap"
]

var emote_hotkeys = [
	Emotes.ATTENTION,
	Emotes.SMILE,
	Emotes.FROWN,
	Emotes.ANNOYED,
	Emotes.LAUGH,
	Emotes.CONFUSED,
	Emotes.TROLL,
	Emotes.MAD,
	Emotes.LADDER
]

func _ready():
	self.visible = false
	animation.playback_speed = 1 / emote_anim_speed

	game_chat.connect("chat_opened", self, "_on_chat_opened")
	game_chat.connect("chat_closed", self, "_on_chat_closed")

func _process(delta: float):
	_tick += delta
	self.rotation_degrees = sin(_tick * sway_frequency) * sway_amplitude * self.modulate.a

func _unhandled_input(event):
	if !is_network_master():
		return

	for i in emote_hotkeys.size():
		var action = "emote_" + str(i + 1)
		var emote = emote_hotkeys[i]
		if Input.is_action_just_pressed(action):
			rpc("_show_emote", emote)

func get_emote_name(emote_index):
	if is_valid_emote(emote_index):
		return emote_names[emote_index]
	else:
		return ""

func get_emote_index(emote_name):
	return emote_names.find(emote_name)

func is_emote_visible():
	return self.visible

func is_emote_fully_visible():
	return is_emote_visible() && self.modulate.a == 1

func is_valid_emote(emote_index):
	var valid = emote_index < emote_names.size()
	if !valid:
		push_warning("Emote index out of bounds")
	return valid

func can_emote():
	return !_chat_open

func _set_emote_index(emote_index):
	if (is_valid_emote(emote_index)):
		if (emote_index < 0):
			# Hide emote
			self.frame = 0
			self.visible = false
		else:
			# Show emote
			self.frame = emote_index
			self.visible = true

func _play_show_anim():
	if !is_emote_visible():
		animation.play("show")
	elif animation.current_animation == "hide":
		# Play anim from where the hide anim is currently at
		var pos = animation.current_animation_length - animation.current_animation_position
		animation.play("show")
		animation.seek(pos, true)

func _play_hide_anim():
	timer.stop()
	animation.play("hide")
	yield(animation, "animation_finished")
	if animation.assigned_animation == "hide":
		_set_emote_index(-1)

puppetsync func _show_emote(emote_index):
	if is_valid_emote(emote_index):
		emit_signal("emote_started", emote_index)

		_play_show_anim()
		_set_emote_index(emote_index)

		timer.start(emote_duration)
		yield (timer, "timeout")
		if !_chat_open:
			rpc("_hide_emote")

puppetsync func _hide_emote():
	emit_signal("emote_ended", self.frame)
	_play_hide_anim()

func _on_chat_opened():
	rpc("_show_chat_emote")

func _on_chat_closed():
	rpc("_hide_chat_emote")

puppetsync func _show_chat_emote():
	_chat_open = true
	_play_show_anim()
	_set_emote_index(Emotes.DOTS)

puppetsync func _hide_chat_emote():
	_chat_open = false
	_play_hide_anim()

func _set_emote_anim_speed(value):
	emote_anim_speed = value
	if self.has_node("Animation"):
		animation.playback_speed = 1 / emote_anim_speed
