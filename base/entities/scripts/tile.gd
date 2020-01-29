extends Node
class_name Tile

enum TileFlags {
	MAIN_SPAWN,
	SIDE_SPAWN
}

export (TileFlags, FLAGS) var flags
export (Color) var representative_color = Color(0, 0, 0, 1)
