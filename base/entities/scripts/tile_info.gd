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

onready var sprite = $Sprite
onready var collider = $Collider

func _get_id() -> int:
	var id = _get_id_from(representative_color)
	return id

static func _get_id_from(color: Color) -> int:
	var stream = StreamPeerBuffer.new()
	
	stream.put_u8(color.r8)
	stream.put_u8(color.g8)
	stream.put_u8(color.b8)
	stream.put_u8(0)
	stream.seek(0)
	
	var id = stream.get_u32()
	return id
