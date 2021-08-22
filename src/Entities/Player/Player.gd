extends KinematicBody2D

var world
var speed = Vector2(100, 200)
var velocity = Vector2.ZERO
const FLOOR_NORMAL = Vector2.UP
const gravity = 700

var current_class = "knight" setget _set_class

const classes = {
	"knight" : preload("res://src/Entities/Player/Knight/Knight.tscn")
}


func _physics_process(_delta: float) -> void:
	_flip_player()
	var direction: = _get_direction()
	velocity = _calculate_move_velocity(velocity, direction, speed)
	velocity = move_and_slide(velocity, FLOOR_NORMAL)
	_send_player_info()


# This method processes player movement inputs
func _get_direction() -> Vector2:
	return Vector2(
		Input.get_action_strength("move_right") - Input.get_action_strength("move_left"),
		-1.0 if Input.is_action_just_pressed("jump") and is_on_floor() else 1.0 # negative x is to the left, negative y is up
	)


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
		get_node("Sprites/Body").flip_h = true
		get_node("Sprites/Head").flip_h = true
	elif mouse_position > 0:
		get_node("Sprites/Body").flip_h = false
		get_node("Sprites/Head").flip_h = false


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
			 "R": get_node("Sprites/Body").flip_h # rotation
			} 
		Server.send_player_state(state)


# This method is called by the world scene
func initialise(world) -> void:
	self.world = world



