extends KinematicBody2D
var speed = Vector2(100, 200)
var velocity = Vector2.ZERO
const FLOOR_NORMAL = Vector2.UP
const gravity = 700

export var player_logic_scene: PackedScene
var game_class
var team

var last_position = Vector2()

var blob_owner: int # is the network unique id

func set_team(team: int) -> void:
	self.team = team
	get_node("Sprites").get_material().set_shader_param("should_swap", bool(team))


func initialise(data: Dictionary) -> void:
	name = str(data["id"])
	game_class = data["class"]
	position = data["spawnpoint"]
	set_team(data["team"])


func update_data(data: Dictionary) -> void:
	position = Vector2(int(data["P"].x), int(data["P"].y))
	get_node("Sprites/AnimationPlayer").play(data["A"])
	get_node("Sprites").scale.x = data["R"]


func set_blob_ownership(player_id: int) -> void:
	blob_owner = player_id
	if blob_owner == get_tree().get_network_unique_id():
		if not has_node("PlayerLogic"):
			var player_logic_instance = player_logic_scene.instance()
			add_child(player_logic_instance)
