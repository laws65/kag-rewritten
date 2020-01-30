extends Node2D

var tile_array = {}
export (String) var map_path
export (Array, String) var search_directories

func _ready():
	load_tiles()
	
	var map_image = load(map_path)
	
	map_image.lock()
	for x in range(map_image.get_width()):
		for y in range(map_image.get_height()):
			var pixel = map_image.get_pixel(x, y)
			var key = pixel.to_html(false)
			if tile_array.has(key):
				var child = tile_array[key].instance()
				child.position = Vector2(8 * x, 8 * y)
				add_child(child)
	map_image.unlock()

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
					var color = scene.instance().representative_color
					var key = color.to_html(false)
					tile_array[key] = scene
			file = dir.get_next()
