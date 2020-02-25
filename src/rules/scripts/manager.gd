extends Node

export (PackedScene) var authentication_scene
export (PackedScene) var matchmaking_scene
export (PackedScene) var multiplayer_scene

var current_scene: Node

func _ready():
	OS.min_window_size = OS.window_size

	Network.connect("login_success", self, "on_login_success")
	Network.connect("login_failure", self, "on_login_failure")

	Network.connect("game_joined", self, "on_game_joined")
	Network.connect("game_exited", self, "on_game_exited")

	if "--host=true" in OS.get_cmdline_args():
		Network._login_as_guest()
		return

	load_scene(authentication_scene)

func on_login_success():
	if "--host=true" in OS.get_cmdline_args():
		Network._create_server("Dedicated KAG Server", 3074)
		return

	load_scene(matchmaking_scene)

func on_login_failure():
	pass

func on_game_joined():
	load_scene(multiplayer_scene)

func on_game_exited():
	load_scene(matchmaking_scene)

func load_scene(scene: PackedScene):
	if current_scene:
		current_scene.queue_free()

	current_scene = scene.instance()
	add_child(current_scene)
