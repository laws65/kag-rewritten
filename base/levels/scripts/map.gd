extends Node2D

onready var shadow_layer = $Shadow

### Customizable
export (String) var map_path
export (Array, String) var search_directories
### ---

var tile_array = {}
var shadow_array = []

### Metadata
var map_image
var map_width
var map_height
### ---

func _ready():
	load_tiles()
	
	map_image = load(map_path)
	map_width = map_image.get_width()
	map_height = map_image.get_height()
	shadow_array.resize(map_width * map_height)
	
	var tmp_pixel
	var tmp_key
	var tmp_child
	var tmp_index
	
	map_image.lock()
	for x in range(map_width):
		for y in range(map_height):
			tmp_pixel = map_image.get_pixel(x, y)
			tmp_key = tmp_pixel.to_html(false)
			tmp_index = x + y * map_width
			if tile_array.has(tmp_key):
				tmp_child = tile_array[tmp_key].instance()
				tmp_child.position = Vector2(8 * x, 8 * y)
				add_child(tmp_child)
				
				if (tmp_child.flags & Tile.TileFlags.LIGHT_PASSES) == Tile.TileFlags.LIGHT_PASSES:
					shadow_array[tmp_index] = false
				else:
					shadow_array[tmp_index] = true
			else:
				shadow_array[tmp_index] = false
	map_image.unlock()
	
	generate_shadows()

func load_tiles():
	var file = ""
	var dir = Directory.new()
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

func generate_shadows():
	var shadow_texture = ImageTexture.new()
	var shadow_image = Image.new()
	
	shadow_image.create(map_width, map_height, true, Image.FORMAT_RGBAF)
	shadow_image.fill(Color(0, 0, 0, 1))
	
	shadow_image.lock()
	for x in range(map_width):
		for y in range(map_height):
			if shadow_array[x + y * map_width] == false:
				shadow_image.set_pixel(x, y, Color(0, 0, 0, 0))
	shadow_image.lock()
	
	shadow_texture.create_from_image(shadow_image)
	shadow_layer.set_texture(shadow_texture)
	shadow_layer.set_scale(Vector2(8, 8))
	
	var material = shadow_layer.get_material()
	material.set_shader_param("Step", Vector2(0.5/map_width, 0.5/map_height))
	material.set_shader_param("Step2", Vector2(0.5/map_width, -0.5/map_height))
	
	#scale(Vector2(8, 8))
	#var shadow_texture = ImageTexture.new()
	#shadow_texture.create(map_width, map_height, Image.FORMAT_RGB8, Texture.FLAG_FILTER)
	#shadow_layer.set_texture(shadow_texture)
