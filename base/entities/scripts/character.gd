extends KinematicBody2D
class_name Character

### Sync
puppet var r_position = Vector2(0, 0)
puppet var r_animation = ""
puppet var r_flip_h = false
### ---

onready var c_anim = $Animation
onready var c_sprite = $Sprite

### Physics
export (int) var gravity = 500
export (int) var move_speed = 100
export (int) var jump_speed = 125

var jumping = false
var velocity = Vector2(0, 0)
### ---

func _ready():
	if (is_network_master()):
		game_camera.target = self

func _animate(animation, flip_h = null):
	c_anim.play(animation)
	c_sprite.flip_h = flip_h if flip_h != null else c_sprite.flip_h
	
	rset("r_animation", animation)
	rset("r_flip_h", c_sprite.flip_h)

func _get_input():
	velocity.x = 0
	
	# Walk
	if Input.is_action_pressed('ui_right'):
		c_sprite.flip_h = false
		velocity.x += move_speed
	if Input.is_action_pressed('ui_left'):
		c_sprite.flip_h = true
		velocity.x -= move_speed
	
	# Jump
	if Input.is_action_pressed('ui_up') and not jumping:
		jumping = true
		velocity.y = -jump_speed
	
	if (abs(velocity.x) <= 0.001):
		_animate("idle")
	else:
		_animate("walk")

func _physics_process(delta):
	if (is_network_master()):
		_get_input()
		
		velocity.y += gravity * delta
		velocity = move_and_slide(velocity, Vector2(0, -1))
		
		if jumping and is_on_floor():
			jumping = false
		
		rset("r_position", position)
	else:
		position = r_position
		c_anim.play(r_animation)
		c_sprite.flip_h = r_flip_h
