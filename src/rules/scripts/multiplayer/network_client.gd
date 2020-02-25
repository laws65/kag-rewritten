extends Node2D
class_name NetworkClient

var client

func _ready():
	Network.connect("game_exited", self, "on_game_exited")

func _process(_delta):
	if client is WebSocketClient:
		if client.get_connection_status() == NetworkedMultiplayerPeer.CONNECTION_CONNECTED:
			client.poll()

func on_game_exited():
	if client is WebSocketClient:
		client.disconnect_from_host()

func join_server(ip: String, port: int):
	client = WebSocketClient.new()

	if client.connect_to_url("ws://" + ip + ":" + str(port), PoolStringArray(), true) == OK:
		get_tree().set_network_peer(client)
