extends CanvasLayer

onready var config := ConfigFile.new()
onready var CONFIG_FILE = "user://login.cfg"

onready var login_email = $Login/Layout/Email
onready var login_password = $Login/Layout/Password
onready var login_remember = $Login/Layout/Remember

onready var register_username = $Register/Layout/Username
onready var register_email = $Register/Layout/Email
onready var register_password = $Register/Layout/Password

func _ready():
	_show_login()

	network.connect("login_success", self, "_on_login_success")
	network.connect("login_failure", self, "_on_login_failure")

	if "--host=true" in OS.get_cmdline_args():
		network._create_server("", 3074)

	if config.load(CONFIG_FILE) == OK:
		login_remember.pressed = config.get_value("Login", "Remember", false)

		if login_remember.pressed:
			login_email.text = config.get_value("Login", "Email", "")

			var token = config.get_value("Login", "Token", "")

			#if token:
			#	network._login_with_token(token)

	$Register/Layout/GoToLogin.connect("pressed", self, "_show_login")
	$Login/Layout/GoToRegister.connect("pressed", self, "_show_register")

	$Login/Layout/Confirm.connect("pressed", self, "_login")
	$Register/Layout/Confirm.connect("pressed", self, "_register")

func _login():
	network._login(login_email.text, login_password.text)

func _register():
	network._register(register_username.text, register_email.text, register_password.text)

func _on_login_success():
	get_tree().change_scene("res://rules/content/matchmaking.tscn")

	if login_remember.pressed:
		config.set_value("Login", "Email", login_email.text)
		config.set_value("Login", "Token", network.api_session.token)
	else:
		config.set_value("Login", "Remember", false)
		config.set_value("Login", "Email", "")
		config.set_value("Login", "Token", null)

	config.set_value("Login", "Remember", login_remember.pressed)
	config.save(CONFIG_FILE)

func _on_login_failure():
	# TODO: Show a proper alert instead of console message
	printerr("An error occurred during authentication: ", network.api_session)

func _show_login():
	$Login.show()
	$Register.hide()

func _show_register():
	$Register.show()
	$Login.hide()
