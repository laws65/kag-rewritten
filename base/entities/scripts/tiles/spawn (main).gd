extends TileBehavior

func _init(t_state: TileState).(t_state):
	var game_mode = game_map.get_node("/root/Multiplayer")
	if game_mode:
		game_mode.spawn_list[0] = state
