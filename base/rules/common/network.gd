extends Node2D

signal join_success()
signal join_fail()
signal player_added(pinfo)
signal player_removed(pinfo)

var server_info = {
	name = "KAG Server",
	port = 0,
}

var players = {}

func _ready():
	get_tree().connect("network_peer_connected", self, "_on_player_connected")
	get_tree().connect("network_peer_disconnected", self, "_on_player_disconnected")
	get_tree().connect("connected_to_server", self, "_on_connected_to_server")
	get_tree().connect("connection_failed", self, "_on_connection_failed")

func create_server(name: String, port: int):
	server_info.name = name
	server_info.port = port
	
	# We are unable to Host from browsers
	# So we create a placeholder Peer to simulate single-player
	# As workaround
	var net
	if OS.has_feature("HTML5"):
		net = NetworkedMultiplayerENet.new()
		
		if (net.create_server(server_info.port) != OK):
			print("Failed to create server")
			return 
	else:
		net = WebSocketServer.new()
		
		if (net.listen(server_info.port, PoolStringArray(), true) != OK):
			print("Failed to create server")
			return
	
	get_tree().set_network_peer(net)
	
	emit_signal("join_success")
	call_deferred("register_player", game_state.player_info)

func join_server(ip, port: int):
	var net = WebSocketClient.new()
	
	if (net.connect_to_url("ws://" + ip + ":" + str(port), PoolStringArray(), true) != OK):
		print("Failed to join server.")
		emit_signal("join_fail")
		return
	
	get_tree().set_network_peer(net)
	game_state.player_info.network_id = get_tree().get_network_unique_id()

### --- Events

func _on_player_connected(id):
	if id != 1:
		rpc_id(id, "register_player", game_state.player_info)

func _on_player_disconnected(id):
	unregister_player(id)

func _on_connected_to_server():
	emit_signal("join_success")
	game_state.player_info.network_id = get_tree().get_network_unique_id()
	
	rpc_id(1, "register_player", game_state.player_info)

func _on_connection_failed():
	emit_signal("join_fail")
	get_tree().set_network_peer(null)

### --- Remote functions

remote func register_player(pinfo):
	players[pinfo.network_id] = pinfo
	emit_signal("player_added", pinfo)

func unregister_player(id):
	if not players.has(id):
		print("No player with this ID to unregister.")
		return
	
	var pinfo = players[id]
	players.erase(id)
	
	emit_signal("player_removed", pinfo)
