tool
extends TileMap


enum Tile {
	STONE = 0,
}


func load_map(map: Array) -> void:
	for x in map.size():
		for y in map[x].size():
			set_cell(x, y, map[x][y])


# Function override of the inbuilt 
func set_cell(x: int, y: int, tile, flip_x=false, flip_y=false, transpose=false, autotile_coord=Vector2()): 
	if tile == Tile.STONE:
		.set_cell(x, y, (x+y)%2, flip_x, flip_y, transpose, autotile_coord)
	else:
		.set_cell(x, y, tile, flip_x, flip_y, transpose, autotile_coord)
