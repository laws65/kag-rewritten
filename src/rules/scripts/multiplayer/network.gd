extends Node2D

signal login_success()
signal login_failure()

signal connection_established()
signal connection_closed()

signal player_added(pinfo)
signal player_removed(pinfo)

var player = {
	id = 1,
	name = "Guest",
	character = "",
}

var players = {}

### Nakama API
var api_key = "kag-rewritten"
var api_host = "vamist.dev"
var api_scheme = "http"
var api_port = 7350

var api: NakamaClient
var api_socket: NakamaSocket
var api_session: NakamaSession
### ---

func _ready():
	get_tree().connect("network_peer_connected", self, "_on_player_connected")
	get_tree().connect("network_peer_disconnected", self, "_on_player_disconnected")

	get_tree().connect("connected_to_server", self, "_on_connection_established")
	get_tree().connect("server_disconnected", self, "_on_connection_closed")
	get_tree().connect("connection_failed", self, "_on_connection_closed")

	$Server.connect("create_success", self, "_on_connection_established")
	$Server.connect("create_fail", self, "_on_connection_closed")

func _login(email, password):
	api = Nakama.create_client(api_key, api_host, api_port, api_scheme)
	api_session = yield(api.authenticate_email_async(email, password, null, false), "completed")

	if !api_session.is_exception():
		api_socket = Nakama.create_socket_from(api)
		yield(api_socket.connect_async(api_session), "completed")

		player.name = api_session.username
		emit_signal("login_success")
	else:
		emit_signal("login_failure")

func _register(username, email, password):
	api = Nakama.create_client(api_key, api_host, api_port, api_scheme)
	api_session = yield(api.authenticate_email_async(email, password, username, true), "completed")

	if !api_session.is_exception():
		api_socket = Nakama.create_socket_from(api)
		yield(api_socket.connect_async(api_session), "completed")

		player.name = api_session.username
		emit_signal("login_success")
	else:
		emit_signal("login_failure")

func _login_with_token(token):
	api = Nakama.create_client(api_key, api_host, api_port, api_scheme)
	api_session = NakamaClient.restore_session(token)

	if api_session.is_valid() && !api_session.is_expired():
		api_socket = Nakama.create_socket_from(api)
		yield(api_socket.connect_async(api_session), "completed")

		player.name = api_session.username
		emit_signal("login_success")
	else:
		emit_signal("login_failure")

func _create_server(server_name: String, server_port: int):
	$Server._create_server(server_name, server_port)

func _join_server(server_ip: String, server_port: int):
	$Client._join_server(server_ip, server_port)

### Helper functions

func get_player(id: int):
	return players[id]

func get_local_player():
	return get_player(get_tree().get_network_unique_id())

func is_local_player(node):
	return node.get_network_master() == get_local_player().id

### Events

func _on_player_connected(id):
	if id != 1:
		rpc_id(id, "register_player", player)

func _on_player_disconnected(id):
	unregister_player(id)

func _on_connection_established():
	emit_signal("connection_established")

	if get_tree().is_network_server():
		call_deferred("register_player", player)
	else:
		network.player.id = get_tree().get_network_unique_id()
		register_player(player)

func _on_connection_closed():
	get_tree().set_network_peer(null)

	emit_signal("connection_closed")

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
