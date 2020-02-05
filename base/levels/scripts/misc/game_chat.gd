extends CanvasLayer

export (int) var max_characters = 150

onready var chat_display = $Panel/Layout/ChatDisplay
onready var chat_input = $Panel/Layout/ChatInput

func _ready():
	chat_input.connect("text_entered", self, "_send_message")

func _input(event):
	if Input.is_action_just_pressed("open_chat"):
		chat_input.grab_focus()
		get_tree().set_input_as_handled()
	
	if Input.is_action_just_pressed("close_chat"):
		chat_input.release_focus()
		get_tree().set_input_as_handled()

func _send_message(message):
	chat_input.clear()
	rpc("_forward_message", message)

remotesync func _forward_message(message: String):
	if !(message is String) or message.replace(" ", "").empty():
		return
	
	var pinfo = network.players[get_tree().get_rpc_sender_id()]
	
	var filtered = message.dedent()
	if max_characters > 0 and message.length() > max_characters:
		filtered = filtered.substr(0, max_characters) + " [...]"
	
	chat_display.append_bbcode(str("\n", "[color=yellow]<", pinfo.name, ">[/color] ", filtered))
