extends Tile

export (String) var spawn_title

func _ready():
	var game_mode = get_node("/root/Game")
	
	if game_mode:
		game_mode.spawn_list[0] = self
		pass