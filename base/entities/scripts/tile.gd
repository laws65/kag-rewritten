extends Node2D
class_name Tile

enum TileFlags {
	MAIN_SPAWN = 1,
	SIDE_SPAWN = 2,
	LIGHT_PASSES = 4,
}

export (TileFlags, FLAGS) var flags
export (Color) var representative_color = Color(0, 0, 0, 1)
