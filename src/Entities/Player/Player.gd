extends GrabbingEntity

var speed = Vector2(100, 200)
var velocity = Vector2.ZERO
var game_class: int
const FLOOR_NORMAL = Vector2.UP
const gravity = 700



func initialise(data: Dictionary) -> void:
	.initialise(data)
	game_class = data["class"]


func update_data(data: Dictionary) -> void:
	position = Vector2(int(data["P"].x), int(data["P"].y))
	get_node("Sprites/AnimationPlayer").play(data["A"])
	get_node("Sprites").scale.x = data["R"]
