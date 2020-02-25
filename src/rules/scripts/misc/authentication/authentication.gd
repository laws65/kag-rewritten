extends CanvasLayer

var config := ConfigFile.new()
var CONFIG_FILE = "user://login.cfg"

onready var login_email = $Login/Layout/Email
onready var login_password = $Login/Layout/Password
onready var login_remember = $Login/Layout/Remember

onready var register_username = $Register/Layout/Username
onready var register_email = $Register/Layout/Email
onready var register_password = $Register/Layout/Password

func _ready():
	show_login()

	if config.load(CONFIG_FILE) == OK:
		login_remember.pressed = config.get_value("Login", "Remember", false)
		var guest = config.get_value("Login", "Guest", false)

		if login_remember.pressed && !guest:
			login_email.text = config.get_value("Login", "Email", "")

			var token = config.get_value("Login", "Token", "")

			# TODO: Fix glitch that happens while using different database
			# when the token refers to the previous one
			#if token:
			#	network._login_with_token(token)

func on_login_success():
	config.set_value("Login", "Remember", login_remember.pressed)

	if login_remember.pressed:
		config.set_value("Login", "Email", login_email.text)
		config.set_value("Login", "Token", Network.api_session.token)
	else:
		config.set_value("Login", "Email", "")
		config.set_value("Login", "Token", "")

	config.save(CONFIG_FILE)

func on_login_failure():
	# TODO: Show a proper alert instead of console message
	printerr("An error occurred during authentication: ", Network.api_session)

func login():
	config.set_value("Login", "Guest", false)
	Network.login(login_email.text, login_password.text)

func login_as_guest():
	config.set_value("Login", "Guest", true)
	Network.login_as_guest()

func register():
	Network.register(register_username.text, register_email.text, register_password.text)

func show_login():
	$Login.show()
	$Register.hide()

func show_register():
	$Register.show()
	$Login.hide()
