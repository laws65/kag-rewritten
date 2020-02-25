extends Node

### Signals
signal map_loaded
### ---

onready var sky = $Sky
onready var shadow = $Shadow
onready var tilemap = $TileMap
var tileset = TileSet.new()

### Customizable
export (String) var map_path
export (Array, String) var tile_directories
export (Vector2) var tile_size = Vector2(8, 8)

export (float) var gravity = 9.81
### ---

var tile_array = []
var shadow_array = []

### Metadata
var tile_scenes = {}

var map_image
var map_width
var map_height
### ---

func _ready():
	Globals.game_map = self

func load_tiles():
	var file = ""
	var dir = Directory.new()
	for path in tile_directories:
		if dir.open(path) != OK:
			continue
		dir.list_dir_begin()
		file = dir.get_next()
		while file != "":
			if file.ends_with(".tscn"):
				var tile = load(path + file).instance()
				if tile is TileInfo:
					add_to_tileset(tile)

			file = dir.get_next()

func add_to_tileset(tile):
	var key = tile.representative_color.to_html(false)
	var id = tileset.get_tiles_ids().size()

	if tile_scenes.has(key):
		return

	tile.tileset_id = id
	tile_scenes[key] = tile

	tileset.create_tile(id)
	tileset.tile_set_texture(id, tile.get_node("Sprite").texture)
	tileset.tile_set_region(id, tile.get_node("Sprite").region_rect)
	tileset.tile_set_z_index(id, tile.z_index)

	if not tile.get_node("Collider").disabled:
		tileset.tile_add_shape(id, tile.get_node("Collider").shape, tile.get_node("Collider").transform)

func load_map():
	tilemap.tile_set = tileset
	load_tiles()

	map_image = load(map_path).get_data()
	map_width = map_image.get_width()
	map_height = map_image.get_height()

	# Adjust Sky to fit map
	sky.scale.x = map_width * tile_size.x
	sky.scale.y = map_height * tile_size.y

	shadow_array.resize(map_width * map_height)
	tile_array.resize(map_width * map_height)

	var tmp_key
	var tmp_scene
	var tmp_state

	var tmp_pixel
	var tmp_index

	map_image.lock()
	for x in range(map_width):
		for y in range(map_height):
			tmp_pixel = map_image.get_pixel(x, y)
			tmp_key = tmp_pixel.to_html(false)
			tmp_index = x + y * map_width

			if tile_scenes.has(tmp_key):
				tmp_scene = tile_scenes[tmp_key]

				tmp_state = TileState.new(tmp_scene)
				tmp_state.add_to_tilemap(tilemap, x, y)

				tile_array[tmp_index] = tmp_state

				if (tmp_state.tile.flags & TileInfo.TileFlags.LIGHT_PASSES) == TileInfo.TileFlags.LIGHT_PASSES:
					shadow_array[tmp_index] = false
				else:
					shadow_array[tmp_index] = true
			else:
				shadow_array[tmp_index] = false
	map_image.unlock()

	generate_shadows()
	optimize_tilemap()

	emit_signal("map_loaded")

func generate_shadows():
	var shadow_texture = ImageTexture.new()
	var shadow_image = Image.new()

	shadow_image.create(map_width, map_height, false, Image.FORMAT_RGBAF)
	shadow_image.fill(Color(0, 0, 0, 0))

	shadow_image.lock()
	for x in range(map_width):
		for y in range(map_height):
			if shadow_array[x + y * map_width]:
				shadow_image.set_pixel(x, y, Color(0, 0, 0, 1))
	shadow_image.unlock()

	shadow_image.resize(map_width * tile_size.x, map_height * tile_size.y, Image.INTERPOLATE_CUBIC)
	shadow_texture.create_from_image(shadow_image, 4)
	shadow.set_texture(shadow_texture)

	var material = shadow.get_material()
	material.set_shader_param("Step", Vector2(0.5 / map_width, 0.5 / map_height))
	material.set_shader_param("Step2", Vector2(0.5 / map_width, -0.5 / map_height))


func optimize_tilemap():
	var top
	var bottom
	var left
	var right
	var topleft
	var topright
	var bottomleft
	var bottomright

	for x in map_width:
		for y in map_height:
			var middle = shadow_array[x + y * map_width]

			if x == 0 || x == map_width - 1 || y == 0 || y == map_height - 1:
				continue

			top = shadow_array[x + (y + 1) * map_width]
			bottom = shadow_array[x + (y - 1) * map_width]
			left = shadow_array[(x - 1) + y * map_width]
			right = shadow_array[(x + 1) + y * map_width]
			topleft = shadow_array[(x - 1) + (y + 1) * map_width]
			topright = shadow_array[(x + 1) + (y + 1) * map_width]
			bottomleft = shadow_array[(x - 1) + (y - 1) * map_width]
			bottomright = shadow_array[(x + 1) + (y - 1) * map_width]

			if middle && top && bottom && left && right:
				if topleft && topright && bottomleft && bottomright:
					tilemap.set_cell(x, y, -1)
