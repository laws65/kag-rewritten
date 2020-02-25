extends Node2D
class_name NetworkServer

signal create_success()
signal create_fail()

var server

var server_info = {
	server_name = "KAG Server",
	server_port = 3074,
}

func _ready():
	Network.connect("game_exited", self, "on_game_exited")

var ping_rate = 28
var tick = 0.0
func _process(_delta):
	tick += _delta
	if tick > ping_rate:
		tick = 0.0
		if server:
			Network.api_socket.rpc_async("ping")

	if server is WebSocketServer:
		if server.is_listening():
			server.poll()

func on_game_exited():
	if server is WebSocketServer:
		server.stop()

func create_server(name: String, port: int, is_private: bool = false):
	Network.player.id = 1

	if !name.empty():
		server_info.server_name = name
	server_info.server_port = port

	# We are unable to Host from browsers
	# So we create a placeholder Peer to simulate single-player
	# As workaround
	if OS.has_feature("HTML5"):
		server = NetworkedMultiplayerENet.new()

		if server.create_server(server_info.server_port) != OK:
			emit_signal("create_fail")
			return
	else:
		server = WebSocketServer.new()

		if server.listen(server_info.server_port, PoolStringArray(), true) != OK:
			emit_signal("create_fail")
			return

		if !is_private:
			var result = yield(Network.api_socket.rpc_async("create_server", server_info), "completed")

			if result.is_exception():
				printerr(result.get_exception())

	get_tree().set_network_peer(server)
	emit_signal("create_success")
