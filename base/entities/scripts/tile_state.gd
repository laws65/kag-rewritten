extends GDScript
class_name TileState

### Metadata
var tile
### ---

### Data
var flags
var map_position
var world_position
### ---

### Behavior
var behavior
### ---

func _init(t_tile):
	tile = t_tile
	flags = tile.flags
	
	if tile.tile_behavior is GDScript:
		behavior = tile.tile_behavior.new(self)

func _add_to(t_tilemap: TileMap, x, y):
	map_position = Vector2(x, y)
	world_position = t_tilemap.map_to_world(map_position)
	
	t_tilemap.set_cellv(map_position, tile.tileset_id)
