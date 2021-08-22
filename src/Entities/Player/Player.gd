extends KinematicBody2D
var speed = Vector2(100, 200)
var velocity = Vector2.ZERO
const FLOOR_NORMAL = Vector2.UP
const gravity = 700

var current_class = "knight" setget _set_class

var game_class
var team
const classes = {
	"knight" : preload("res://src/Entities/Player/Knight/Knight.tscn")
}


func _physics_process(_delta: float) -> void:
	_flip_player()
	var direction: = _get_direction()
	velocity = _calculate_move_velocity(velocity, direction, speed)
	velocity = move_and_slide(velocity, FLOOR_NORMAL)
	_send_player_info()


func _get_direction() -> Vector2:
	var direction = get_node("InputDirection").get_input_direction()
	if Input.is_action_just_pressed("move_up") and is_on_floor():
		direction.y = -1.0
	else:
		direction.y = 1.0
	return direction


# This method handles movement calculation
func _calculate_move_velocity(
		linear_velocity: Vector2,
		direction: Vector2,
		speed: Vector2
	) -> Vector2:
	var out: = linear_velocity
	out.x = speed.x * direction.x
	out.y += gravity * get_physics_process_delta_time() # so we dont have to pass delta to the function
	if direction.y == -1.0:
		out.y = speed.y * direction.y
	return out # out is the new velocity


# This method flips the player sprites direction based on the mouses position
func _flip_player() -> void:
	var mouse_position = sign(get_global_mouse_position().x - global_position.x)
	if mouse_position < 0:
		get_node("Sprites").scale.x = -1
	elif mouse_position > 0:
		get_node("Sprites").scale.x = 1


# This method changes class by instancing a class node and deleting an old one
func _set_class(new_class) -> void:
	if current_class == new_class:
		pass
	var instance = classes[new_class].instance()
	instance.global_position = self.global_position
	instance.name = new_class
	get_node(current_class).queue_free()
	self.add_child(instance)


# This method sends player info to the server
func _send_player_info() -> void:
	if Server.is_connected:
		var state = {"T": Server.client_clock, # time
			 "P": get_global_position(),  # position
			 "A": get_node("Sprites/AnimationPlayer").current_animation, # animation
			 "R": get_node("Sprites").scale.x # rotation
			} 
		Server.send_player_state(state)



