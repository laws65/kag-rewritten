extends Node2D
class_name NetworkClient

var client

func _process(_delta):
	if client is WebSocketClient:
		if (client.get_connection_status() == NetworkedMultiplayerPeer.CONNECTION_CONNECTED
			or client.get_connection_status() == NetworkedMultiplayerPeer.CONNECTION_CONNECTING):
				client.poll()

func _join_server(ip, port: int):
	client = WebSocketClient.new()
	
	if (client.connect_to_url("ws://" + ip + ":" + str(port), PoolStringArray(), true) != OK):
		return
	
	get_tree().set_network_peer(client)
