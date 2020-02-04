extends CanvasLayer

export (int) var max_characters = 150

onready var chat_display = $Panel/Layout/ChatDisplay
onready var chat_input = $Panel/Layout/ChatInput

func _ready():
	chat_input.connect("text_entered", self, "_send_message")

func _process(delta):
	if Input.is_action_just_pressed("open_chat"):
		chat_input.grab_focus()
	
	if Input.is_action_just_pressed("close_chat"):
		chat_input.release_focus()

func _send_message(message):
	chat_input.clear()
	rpc("_forward_message", message)

remotesync func _forward_message(message: String):
	var pinfo = network.players[get_tree().get_rpc_sender_id()]
	
	var filtered = message
	if max_characters > 0 and message.length() > max_characters:
		filtered = message.substr(0, max_characters) + " [...]"
	
	chat_display.append_bbcode(str("[color=yellow]<", pinfo.name, ">[/color] ", filtered, "\n"))
