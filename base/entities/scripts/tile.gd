extends Node
class_name Tile

enum TileFlags {
	MAIN_SPAWN,
	LIQUID,
	AIR,
}

export (TileFlags, FLAGS) var flags
export (Color) var representative_color = Color(0, 0, 0, 1)

#func _ready():
#	pass

#func _process(delta):
#	pass
