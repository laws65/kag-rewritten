extends Node2D
class_name NetworkServer

signal create_success()
signal create_fail()

var server

var server_info = {
	name = "KAG Server",
	port = 0,
}

func _process(_delta):
	if server is WebSocketServer:
		if server.is_listening():
			server.poll()

func _create_server(name: String, port: int):
	if !name.empty():
		server_info.name = name
	server_info.port = port

	# We are unable to Host from browsers
	# So we create a placeholder Peer to simulate single-player
	# As workaround
	if OS.has_feature("HTML5"):
		server = NetworkedMultiplayerENet.new()

		if (server.create_server(server_info.port) != OK):
			emit_signal("create_fail")
			return
	else:
		server = WebSocketServer.new()

		if (server.listen(server_info.port, PoolStringArray(), true) != OK):
			emit_signal("create_fail")
			return

		#yield(network.api_socket.create_match_async(), "completed")
		var result = yield(network.api_socket.rpc_async("create_server", { "server_name": server_info.name, "server_port": server_info.port }), "completed")
		if result.is_exception():
			printerr(result.get_exception())
		else:
			pass

	get_tree().set_network_peer(server)
	emit_signal("create_success")
