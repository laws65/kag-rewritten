extends KinematicBody2D

var game_class
var team


func update_data(data: Dictionary) -> void:
	print("updating")
	position = Vector2(int(data["P"].x), int(data["P"].y))
	get_node("Sprites/AnimationPlayer").play(data["A"])
	get_node("Sprites").scale.x = data["R"]


func set_team(team: int) -> void:
	self.team = team
	get_node("Sprites").get_material().set_shader_param("should_swap", bool(team))


func initialise(data: Dictionary) -> void:
	name = str(data["id"])
	game_class = data["class"]
	position = data["spawnpoint"]
	set_team(data["team"])
