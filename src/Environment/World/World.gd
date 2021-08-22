extends Node2D

var player_spawn = preload("res://src/Entities/Player/Template/PlayerTemplate.tscn")
var spawn_point = []
var last_world_state = 0
var world_state_buffer = []
const interpolation_offset = 100
onready var Tilemap = $TileMap


func _ready() -> void:
	#spawn_point = Tilemap.create_world()
	get_node("YSort/Player").initialise(Tilemap)


func _physics_process(_delta: float) -> void:
	
	var render_time = Server.client_clock - interpolation_offset
	if world_state_buffer.size() > 1:
		while world_state_buffer.size() > 2 and render_time > world_state_buffer[2].T:
			world_state_buffer.remove(0)
		if world_state_buffer.size() > 2: # if the game should interpolate
			
			var interpolation_factor = float(render_time - world_state_buffer[1]["T"]) / float(world_state_buffer[2]["T"] - world_state_buffer[1]["T"])
			for player in world_state_buffer[2].keys():
				
				if str(player) == "T":
					continue
				if player == get_tree().get_network_unique_id():
					continue
				if not world_state_buffer[1].has(player):
					continue
				
				if get_node("YSort/OtherPlayers").has_node(str(player)):
					var new_position = lerp(world_state_buffer[1][player]["P"], world_state_buffer[2][player]["P"], interpolation_factor)
					get_node("YSort/OtherPlayers/" + str(player)).update_info(new_position, world_state_buffer[2][player]["A"], world_state_buffer[2][player]["R"])
				else:
					spawn_new_player(player)
					
		elif render_time > world_state_buffer[1].T: # if the game should extrapolate instead
			
			var extrapolation_factor = float(render_time - world_state_buffer[0]["T"]) / float(world_state_buffer[1]["T"] - world_state_buffer[0]["T"]) - 1.00
			for player in world_state_buffer[1].keys():
				
				if str(player) == "T":
					continue
				if player == get_tree().get_network_unique_id():
					continue
				if not world_state_buffer[0].has(player):
					continue
					
				if get_node("YSort/OtherPlayers").has_node(str(player)):
					var position_delta = (world_state_buffer[1][player]["P"] - world_state_buffer[0][player]["P"])
					var new_position = world_state_buffer[1][player]["P"] + (position_delta * extrapolation_factor)
					get_node("YSort/OtherPlayers/" + str(player)).update_info(new_position, world_state_buffer[1][player]["A"], world_state_buffer[1][player]["R"])


func spawn_new_player(player_id) -> void:
	if get_tree().get_network_unique_id() == player_id: # check if ur not spawning in a clone of urself
		pass
	elif not get_node("YSort/OtherPlayers").has_node(str(player_id)): # check if it already exists
		var new_player = player_spawn.instance()
		new_player.global_position = Vector2(0, 0)
		new_player.name = str(player_id)
		get_node("YSort/OtherPlayers").add_child(new_player, true)


func despawn_player(player_id) -> void:
	if get_node("YSort/OtherPlayers").has_node(str(player_id)):
		get_node("YSort/OtherPlayers/" + str(player_id)).queue_free()


func update_world_state(world_state) -> void:
	if world_state["T"] > last_world_state:
		last_world_state = world_state["T"]
		world_state_buffer.append(world_state)
