extends TileBehavior

func _init(t_state: TileState).(t_state):
	state.tile.get_node("Sprite").frame = randi()%8
