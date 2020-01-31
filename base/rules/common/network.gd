extends Node2D

signal server_created()
signal join_success()
signal join_fail()
signal player_list_changed()
signal player_removed(pinfo)

var server_info = {
	name = "KAG Server",
	used_port = 0,
}

var players = {}

func _ready():
	get_tree().connect("network_peer_connected", self, "_on_player_connected")
	get_tree().connect("network_peer_disconnected", self, "_on_player_disconnected")
	get_tree().connect("connected_to_server", self, "_on_connected_to_server")
	get_tree().connect("connection_failed", self, "_on_connection_failed")
	get_tree().connect("server_disconnected", self, "_on_disconnected_from_server")

### ---

func create_server():
	var net
	if OS.has_feature("HTML5"):
		# We are unable to Host from browsers
		# So we create a placeholder Peer to simulate single-player
		# As workaround
		net = NetworkedMultiplayerENet.new()
		
		if (net.create_server(server_info.used_port) != OK):
			print("Failed to create server")
			return 
	else:
		net = WebSocketServer.new()
		
		if (net.listen(server_info.used_port, PoolStringArray(), true) != OK):
			print("Failed to create server")
			return
	
	get_tree().set_network_peer(net)
	emit_signal("server_created")
	register_player(game_state.player_info)

func join_server(ip, port):
	var net = WebSocketClient.new()
	
	if (net.connect_to_url("ws://" + ip + ":" + str(port), PoolStringArray(), true) != OK):
		print("Failed to create web client")
		emit_signal("join_fail")
		return
	
	get_tree().set_network_peer(net)

### --- Events

func _on_player_connected(id):
	pass

func _on_player_disconnected(id):
	print("Player ", players[id].name, " disconnected from server")
	
	if (get_tree().is_network_server()):
		unregister_player(id)
		rpc("unregister_player", id)

func _on_connected_to_server():
	emit_signal("join_success")
	game_state.player_info.network_id = get_tree().get_network_unique_id()

	rpc_id(1, "register_player", game_state.player_info)
	register_player(game_state.player_info)

func _on_connection_failed():
	emit_signal("join_fail")
	get_tree().set_network_peer(null)

func _on_disconnected_from_server():
	print("Disconnected from server")
	players.clear()
	game_state.player_info.network_id = 1

### --- Remote functions

remote func register_player(pinfo):
	if (get_tree().is_network_server()):
		for id in players:
			rpc_id(pinfo.network_id, "register_player", players[id])
			
			if (id != 1):
				rpc_id(id, "register_player", pinfo)
	
	print("Registering player ", pinfo.name, " (", pinfo.network_id, ") to internal player table")
	players[pinfo.network_id] = pinfo
	emit_signal("player_list_changed")

remote func unregister_player(id):
	var pinfo = players[id]
	print("Removing player ", pinfo.name, " from internal table")
	
	players.erase(id)
	emit_signal("player_list_changed")
	emit_signal("player_removed", pinfo)
