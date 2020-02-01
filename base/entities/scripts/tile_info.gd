extends Node2D
class_name TileInfo

enum TileFlags {
	MAIN_SPAWN = 1,
	SIDE_SPAWN = 2,
	LIGHT_PASSES = 4,
}

export (String) var display_name
export (TileFlags, FLAGS) var flags
export (Color) var representative_color = Color(0, 0, 0, 1)

export (GDScript) var tile_behavior

var tileset_id
onready var sprite = $Spritewa
onready var collider = $Collider
