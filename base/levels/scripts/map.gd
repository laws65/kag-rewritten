extends Node2D

var tile_array = []
export (String) var map_path
export (Array, String) var search_directories

func _ready():
	load_tiles()
	
	var map_image = load(map_path)
	
	map_image.lock()
	for x in range(map_image.get_width()):
		for y in range(map_image.get_height()):
			var pixel = map_image.get_pixel(x, y)
			for tile in tile_array:
				var child = tile.instance()
				if pixel.to_html(false) == child.representative_color.to_html(false):
					child.position = Vector2(8 * x, 8 * y)
					add_child(child)
	map_image.unlock()
	
	pass

#func _process(delta):
#	pass

func load_tiles():
	var dir = Directory.new()
	var file
	for path in search_directories:
		if dir.open(path) != OK:
			pass
		dir.list_dir_begin()
		file = dir.get_next()
		while file != "":
			if file.ends_with(".tscn"):
				var scene = load(path + file)
				if scene.instance() is Tile:
					tile_array.append(scene)
			file = dir.get_next()
	pass