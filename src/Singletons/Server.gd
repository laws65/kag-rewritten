extends Node

var network = NetworkedMultiplayerENet.new()
var ip = "127.0.0.1"
var port = 28574
var is_connected = false
var decimal_collector: float = 0
var latency_array = []
var latency = 0
var delta_latency = 0
var client_clock = 0

# Vars for other nodes to see
var current_map : String = "Default" 
var is_spectating : bool = true


func _ready() -> void:
	connect_to_server(ip, port)


func _physics_process(delta: float) -> void:
	client_clock += int(delta*1000) + delta_latency
	delta_latency = 0
	decimal_collector += (delta*1000) - int(delta*1000)
	if decimal_collector >= 1.00:
		client_clock += 1
		decimal_collector -= 1.00


func connect_to_server(ip, port) -> void:
	network.create_client(ip, port)
	get_tree().set_network_peer(network)
	
	network.connect("connection_failed", self, "on_connection_failed")
	network.connect("connection_succeeded", self, "on_connection_succeeded")


func on_connection_failed() -> void:
	print("Failed to connect to server")


func on_connection_succeeded() -> void:
	print("Successfully connected to server")
	is_connected = true
	rpc_id(1, "fetch_server_time", OS.get_system_time_msecs())
	var timer = Timer.new()
	timer.wait_time = 0.5
	timer.autostart = true
	timer.connect("timeout", self, "determine_latency")
	self.add_child(timer)


func determine_latency() -> void:
	rpc_id(1, "determine_latency", OS.get_system_time_msecs())


remote func return_latency(client_time) -> void:
	latency_array.append((OS.get_system_time_msecs() - client_time) / 2)
	if latency_array.size() == 9:
		var total_latency = 0
		latency_array.sort()
		var mid_point = latency_array[4]
		for i in range(latency_array.size()-1, -1, -1):
			if latency_array[i] > (2 * mid_point) and latency_array[i] > 20:
				latency_array.remove(i)
			else:
				total_latency += latency_array[i]
		delta_latency = (total_latency / latency_array.size()) - latency
		latency = total_latency / latency_array.size()
		latency_array.clear()


remote func return_server_time(server_time, client_time) -> void:
	latency = (OS.get_system_time_msecs() - client_time) / 2
	client_clock = server_time + latency


func send_player_state(player_state) -> void:
	if is_connected:
		rpc_unreliable_id(1, "recieve_player_state", player_state)


remote func recieve_world_state(world_state) -> void:
	get_node("../World").update_world_state(world_state)


remote func spawn_player(player_id: int, player_data: Dictionary) -> void:
	get_node("../World").spawn_player(player_id, player_data)
	if player_id == get_tree().get_network_unique_id():
		is_spectating = false


remote func despawn_player(player_id: int) -> void:
	get_node("../World").despawn_player(player_id)
	if player_id == get_tree().get_network_unique_id():
		is_spectating = true


func get_stat(context, type, value, requester) -> void:
	print("Interface attempts to get stat")
	rpc_id(1, "get_stat", context, type, value, requester)


remote func return_stat(stat, requester, value) -> void:
	print("Interface recieved stat from server")
	instance_from_id(requester).update_variable(stat, value)


remote func load_map(map_name: String, map_data: Array) -> void:
	get_node("../World/TileMap").load_map(map_data)
	print("Loading map " + map_name)
