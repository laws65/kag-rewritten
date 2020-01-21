extends Node

signal server_created
signal join_success
signal join_fail
signal player_list_changed
signal player_removed(pinfo)

var server_info = {
	name = "KAG Server",
	max_players = 0,
	used_port = 0
}

var players = {}

func _ready():
	get_tree().connect("network_peer_connected", self, "_on_player_connected")
	get_tree().connect("network_peer_disconnected", self, "_on_player_disconnected")
	get_tree().connect("connected_to_server", self, "_on_connected_to_server")
	get_tree().connect("connection_failed", self, "_on_connection_failed")
	get_tree().connect("server_disconnected", self, "_on_disconnected_from_server")

func create_server():
	# Initialize the networking system
	var net = NetworkedMultiplayerENet.new()
	
	# Try to create the server
	if (net.create_server(server_info.used_port, server_info.max_players) != OK):
		print("Failed to create server")
		return
	
	get_tree().set_network_peer(net)
	emit_signal("server_created")
	register_player(gamestate.player_info)


func join_server(ip, port):
	var net = NetworkedMultiplayerENet.new()
	
	if (net.create_client(ip, port) != OK):
		print("Failed to create client")
		emit_signal("join_fail")
		return
		
	get_tree().set_network_peer(net)

### Event handlers

# Everyone gets notified whenever a new client joins the server
func _on_player_connected(id):
	pass

# Everyone gets notified whenever someone disconnects from the server
func _on_player_disconnected(id):
	print("Player ", players[id].name, " disconnected from server")
	if (get_tree().is_network_server()):
		unregister_player(id)
		rpc("unregister_player", id)

# Peer trying to connect to server is notified on success
func _on_connected_to_server():
	emit_signal("join_success")
	gamestate.player_info.net_id = get_tree().get_network_unique_id()

	rpc_id(1, "register_player", gamestate.player_info)
	register_player(gamestate.player_info)

# Peer trying to connect to server is notified on failure
func _on_connection_failed():
	emit_signal("join_fail")
	get_tree().set_network_peer(null)

# Peer is notified when disconnected from server
func _on_disconnected_from_server():
	print("Disconnected from server")
	players.clear()
	gamestate.player_info.net_id = 1

### Remote functions
remote func register_player(pinfo):
	if (get_tree().is_network_server()):
		for id in players:
			rpc_id(pinfo.net_id, "register_player", players[id])

			if (id != 1):
				rpc_id(id, "register_player", pinfo)

	print("Registering player ", pinfo.name, " (", pinfo.net_id, ") to internal player table")
	players[pinfo.net_id] = pinfo
	emit_signal("player_list_changed")

remote func unregister_player(id):
	var pinfo = players[id]
	print("Removing player ", pinfo.name, " from internal table")
	
	players.erase(id)
	emit_signal("player_list_changed")
	emit_signal("player_removed", pinfo)
