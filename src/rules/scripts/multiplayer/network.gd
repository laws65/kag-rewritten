extends Node2D

signal login_success()
signal login_failure()

signal game_joined()
signal game_exited()

signal player_added(pinfo)
signal player_removed(pinfo)

var player = {
	id = 1,
	username = "",
	character = "",
}

var players = {}

### Nakama API
var api_key = "kag-rewritten"
var api_host = "vamist.dev"
var api_scheme = "https"
var api_port = 443

var api: NakamaClient
var api_socket: NakamaSocket
var api_session: NakamaSession
### ---

onready var Nakama = $Nakama
onready var Server: NetworkServer = $Server
onready var Client: NetworkClient = $Client

func _ready():
	get_tree().connect("network_peer_connected", self, "on_player_connected")
	get_tree().connect("network_peer_disconnected", self, "on_player_disconnected")

	get_tree().connect("connected_to_server", self, "on_connection_opened")
	get_tree().connect("server_disconnected", self, "on_connection_closed")
	get_tree().connect("connection_failed", self, "on_connection_closed")

	Server.connect("create_success", self, "on_connection_opened")
	Server.connect("create_fail", self, "on_connection_closed")

func login(email, password):
	api = Nakama.create_client(api_key, api_host, api_port, api_scheme)
	api_session = yield(api.authenticate_email_async(email, password, null, false), "completed")

	if !api_session.is_exception():
		api_socket = Nakama.create_socket_from(api)
		yield(api_socket.connect_async(api_session), "completed")

		player.name = api_session.username
		emit_signal("login_success")
	else:
		emit_signal("login_failure")

func register(username, email, password):
	api = Nakama.create_client(api_key, api_host, api_port, api_scheme)
	api_session = yield(api.authenticate_email_async(email, password, username, true), "completed")

	if !api_session.is_exception():
		api_socket = Nakama.create_socket_from(api)
		yield(api_socket.connect_async(api_session), "completed")

		player.name = api_session.username
		emit_signal("login_success")
	else:
		emit_signal("login_failure")

func login_with_token(token):
	api = Nakama.create_client(api_key, api_host, api_port, api_scheme)
	api_session = NakamaClient.restore_session(token)

	if api_session.is_valid() && !api_session.is_expired():
		api_socket = Nakama.create_socket_from(api)
		yield(api_socket.connect_async(api_session), "completed")

		player.name = api_session.username
		emit_signal("login_success")
	else:
		emit_signal("login_failure")

var guest_cfg = ConfigFile.new()
func login_as_guest():
	var unique_id

	guest_cfg.load("user://login.cfg")

	if guest_cfg.has_section_key("Login", "UniqueID"):
		unique_id = guest_cfg.get_value("Login", "UniqueID")
	else:
		randomize()

		var arr = PoolStringArray()
		for _i in range(0, 16):
			arr.append(str(randi()%10))
		unique_id = arr.join("")

		guest_cfg.set_value("Login", "UniqueID", unique_id)

	guest_cfg.save("user://login.cfg")

	api = Nakama.create_client(api_key, api_host, api_port, api_scheme)
	api_session = yield(api.authenticate_device_async(unique_id), "completed")

	if !api_session.is_exception():
		api_socket = Nakama.create_socket_from(api)
		yield(api_socket.connect_async(api_session), "completed")

		player.name = "Guest_" + api_session.username
		emit_signal("login_success")
	else:
		emit_signal("login_failure")

func create_server(server_name: String, server_port: int, is_private: bool = false):
	Server.create_server(server_name, server_port, is_private)

func join_server(server_ip: String, server_port: int):
	Client.join_server(server_ip, server_port)

### Helper functions

func get_player(id: int):
	return players[id]

func get_local_player():
	return get_player(get_tree().get_network_unique_id())

func is_local_player(node):
	return node.get_network_master() == get_local_player().id

### Events

func on_player_connected(id):
	if id != 1:
		rpc_id(id, "register_player", player)

func on_player_disconnected(id):
	unregister_player(id)

func on_connection_opened():
	emit_signal("game_joined")

	if get_tree().is_network_server():
		call_deferred("register_player", player)
	else:
		player.id = get_tree().get_network_unique_id()
		register_player(player)

func on_connection_closed():
	get_tree().set_network_peer(null)

	emit_signal("game_exited")

### Remote functions

remote func register_player(pinfo):
	if get_tree().get_rpc_sender_id() == 0 && pinfo.id != 1:
		rpc_id(1, "register_player", player)

	players[pinfo.id] = pinfo
	emit_signal("player_added", pinfo)

func unregister_player(id):
	if not (id in players):
		print("No player with this ID to unregister.")
		return

	var pinfo = players[id]
	players.erase(id)

	emit_signal("player_removed", pinfo)
