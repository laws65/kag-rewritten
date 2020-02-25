extends TileBehavior

func _start():
	state.tile.get_node("Sprite").frame = randi()%8
