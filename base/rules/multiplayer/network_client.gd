extends Node2D
class_name NetworkClient

signal join_success()
signal join_fail()

var client

func _ready():
	network.connect("player_connected", self, "_on_player_connected")
	network.connect("player_disconnected", self, "_on_player_disconnected")

func _process(_delta):
	if client is WebSocketClient:
		if client.get_connection_status() != NetworkedMultiplayerPeer.CONNECTION_DISCONNECTED:
			client.poll()

func _join_server(ip, port: int):
	client = WebSocketClient.new()
	
	if (client.connect_to_url("ws://" + ip + ":" + str(port), PoolStringArray(), true) != OK):
		print("Failed to join server.")
		return
	
	get_tree().set_network_peer(client)

func _on_player_connected(id):
	pass

func _on_player_disconnected(id):
	pass

func _register_player(id):
	pass

func _unregister_player(id):
	pass
