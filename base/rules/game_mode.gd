extends Node2D
class_name GameMode

export (String) var default_character

func _ready():
	gamestate.player_info.character = default_character
	
	network.connect("player_list_changed", self, "_on_player_list_changed")
	if (get_tree().is_network_server()):
		network.connect("player_removed", self, "_on_player_removed")
	
	if (get_tree().is_network_server()):
		spawn_player(gamestate.player_info, 1)
	else:
		rpc_id(1, "spawn_player", gamestate.player_info, -1)

### --- Events

func _on_player_list_changed():
	pass
	
func _on_player_removed(pinfo):
	despawn_player(pinfo)

### --- Remote functions

remote func spawn_player(pinfo, spawn_index):
	if (spawn_index == -1):
		spawn_index = network.players.size()
	
	if (get_tree().is_network_server() && pinfo.network_id != 1):
		var s_index = 1
		for id in network.players:
			if (id != pinfo.network_id):
				rpc_id(pinfo.network_id, "spawn_player", network.players[id], s_index)
			
			if (id != 1):
				rpc_id(id, "spawn_player", pinfo, spawn_index)
			
			s_index += 1
	
	print("Spawning actor for player ", pinfo.name, "(", pinfo.network_id, ") - ", spawn_index)
	
	# Create a character
	var pclass = load(pinfo.character)
	var pchar = pclass.instance()
	
	# pchar.position = 
	
	if (pinfo.network_id != 1):
		pchar.set_network_master(pinfo.network_id)
		pchar.set_name(str(pinfo.network_id))
	
	add_child(pchar)

remote func despawn_player(pinfo):
	if (get_tree().is_network_server()):
		for id in network.players:
			if (id == pinfo.network_id || id == 1):
				continue
			
			rpc_id(id, "despawn_player", pinfo)
	
	# Try to locate the player actor
	var player_node = get_node(str(pinfo.network_id))
	if (!player_node):
		print("Cannot remove invalid node from tree")
		return
	
	player_node.queue_free()
