extends Node2D

var spawn_point = []
var last_world_state = 0
var world_state_buffer = []
const interpolation_offset = 100

export var PlayerScene: PackedScene


func _physics_process(_delta: float) -> void:
	var render_time = Server.client_clock - interpolation_offset
	if world_state_buffer.size() <= 1:
		return
		
	while world_state_buffer.size() > 2 and render_time > world_state_buffer[2].T:
		world_state_buffer.remove(0)

	if world_state_buffer.size() > 2: # if the game should interpolate
		var interpolation_factor = float(render_time - world_state_buffer[1]["T"]) / float(world_state_buffer[2]["T"] - world_state_buffer[1]["T"])
		for player in world_state_buffer[2].keys():
			
			if str(player) == "T":
				continue
			if player == Server.player_instance_id:
				continue
			if not world_state_buffer[1].has(player):
				continue
			if not get_node("YSort/Entities").has_node(str(player)):
				continue
			if get_node("YSort/Entities/" + str(player)).blob_owner == get_tree().get_network_unique_id():
				continue
			var new_position = lerp(world_state_buffer[1][player]["P"], world_state_buffer[2][player]["P"], interpolation_factor)
			var data_dict = {"P": new_position, "A": world_state_buffer[2][player]["A"], "R": world_state_buffer[2][player]["R"]}
			get_node("YSort/Entities/" + str(player)).update_data(data_dict)

	elif render_time > world_state_buffer[1].T: # if the game should extrapolate instead
		
		var extrapolation_factor = float(render_time - world_state_buffer[0]["T"]) / float(world_state_buffer[1]["T"] - world_state_buffer[0]["T"]) - 1.00
		for player in world_state_buffer[1].keys():
			
			if str(player) == "T":
				continue
			if player == Server.player_instance_id:
				continue
			if not world_state_buffer[0].has(player):
				continue
			if not get_node("YSort/Entities").has_node(str(player)):
				continue
			if get_node("YSort/Entities/" + str(player)).blob_owner == get_tree().get_network_unique_id():
				continue
			var position_delta = (world_state_buffer[1][player]["P"] - world_state_buffer[0][player]["P"])
			var new_position = world_state_buffer[1][player]["P"] + (position_delta * extrapolation_factor)
			var data_dict = {"P": new_position, "A": world_state_buffer[1][player]["A"], "R": world_state_buffer[1][player]["R"]}
			get_node("YSort/Entities/" + str(player)).update_data(data_dict)


func despawn_blob(player_network_id: int, blob_id: int) -> void:
	if get_node("YSort/Entities").has_node(str(blob_id)):
		get_node("YSort/Entities/" + str(blob_id)).queue_free()
	if player_network_id == get_tree().get_network_unique_id():
		Server.is_spectating = true


func update_world_state(world_state) -> void:
	if world_state["T"] > last_world_state:
		last_world_state = world_state["T"]
		world_state_buffer.append(world_state)


func spawn_blob(blob_name: String, blob_data: Dictionary) -> void:
	if blob_name == "player":
		if get_node("YSort/Entities").has_node(str(blob_data["id"])):
			return
		var new_player = PlayerScene.instance()
		new_player.initialise(blob_data)
		get_node("YSort/Entities").add_child(new_player, true)


func set_blob_ownership(player_id: int, blob_id: int) -> void:
	if not has_node("YSort/Entities/" + str(blob_id)):
		return
	if player_id == get_tree().get_network_unique_id():
		Server.player_instance_id = get_node("YSort/Entities/" + str(blob_id)).get_instance_id()
	get_node("YSort/Entities/" + str(blob_id)).set_blob_ownership(player_id)
