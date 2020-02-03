extends Node2D
class_name NetworkServer

signal create_success()
signal create_fail()

var server_info = {
	name = "KAG Server",
	port = 0,
}

var server

func _ready():
	network.connect("player_connected", self, "_on_player_connected")
	network.connect("player_disconnected", self, "_on_player_disconnected")

func _process(_delta):
	if server is WebSocketServer:
		if server.is_listening():
			server.poll()

func _create_server(name: String, port: int):
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
	
	get_tree().set_network_peer(server)
	emit_signal("create_success")

func _on_player_connected(id):
	pass

func _on_player_disconnected(id):
	pass

func _register_player(id):
	pass

func _unregister_player(id):
	pass
