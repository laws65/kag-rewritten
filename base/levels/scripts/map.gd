extends Node2D

onready var shadow_layer = $Shadow
var tile_array = {}
var solid_array = []
export (String) var map_path
export (Array, String) var search_directories

var map_width
var map_height

func _ready():
	load_tiles()
	
	var map_image = load(map_path)
	
	map_image.lock()
	map_width = map_image.get_width()
	map_height = map_image.get_height()
	solid_array.resize(map_width*map_height)
	for x in range(map_width):
		for y in range(map_height):
			var pixel = map_image.get_pixel(x, y)
			var key = pixel.to_html(false)
			if tile_array.has(key):
				var child = tile_array[key].instance()
				child.position = Vector2(8 * x, 8 * y)
				add_child(child)
				if child.get_node("Collider").is_disabled():
					solid_array[x+y*map_width] = false
				else:
					solid_array[x+y*map_width] = true
			else:
				solid_array[x+y*map_width] = false
	map_image.unlock()
	generate_shadows()

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

func generate_shadows():
	var shadow_texture = ImageTexture.new()
	var shadow_image = Image.new()
	
	shadow_image.create(map_width, map_height, true, Image.FORMAT_RGBAF)
	shadow_image.fill(Color(0, 0, 0, 1))
	shadow_image.lock()
	for x in range(map_width):
		for y in range(map_height):
			if solid_array[x+y*map_width] == false:
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