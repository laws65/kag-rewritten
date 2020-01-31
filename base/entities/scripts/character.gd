extends KinematicBody2D
class_name Character

### Sync
puppet var r_position = Vector2(0, 0)
puppet var r_animation = ""
puppet var r_flip_h = false
### ---

onready var c_anim = $Animation
onready var c_sprite = $Sprite

onready var dust = load("res://base/entities/content/effects/dust.tscn")
onready var effects = Node.new()

### Physics
export (int) var gravity = 500
export (int) var move_speed = 75
export (int) var jump_speed = 125

var velocity = Vector2(0, 0)
### ---

### Input
var jumping = false
var crouching = false

var moveRight = false
var moveLeft = false
### ---

func _ready():
	if (is_network_master()):
		game_camera.target = self

func _animate(animation, flip_h = null):
	c_anim.play(animation)
	c_sprite.flip_h = flip_h if flip_h != null else c_sprite.flip_h
	
	rset("r_animation", c_anim.current_animation)
	rset("r_flip_h", c_sprite.flip_h)

func _process_input():
	velocity.x = 0
	
	# Walk
	if Input.is_action_pressed("move_right"):
		velocity.x += move_speed
		moveRight = true
	else:
		moveRight = false
	
	if Input.is_action_pressed("move_left"):
		velocity.x -= move_speed
		moveLeft = true
	else:
		moveLeft = false
	
	# Jump
	if Input.is_action_pressed("jump") and not (jumping or crouching):
		velocity.y = -jump_speed
		jumping = true
	
	# Crouch
	if Input.is_action_pressed("crouch"):
		crouching = true
	else:
		crouching = false

func _process_animation():
	if is_on_floor():
		if moveLeft or moveRight:
			_animate("walk", moveLeft)
		else:
			if crouching:
				_animate("crouch")
			else:
				_animate("idle")
	else:
		if jumping:
			_animate("jump")
		else:
			#_animate("fall")
			pass

func _physics_process(delta):
	if (is_network_master()):
		_process_input()
		_process_animation()
		
		velocity.y += gravity * delta
		velocity = move_and_slide(velocity, Vector2(0, -1))
		
		if jumping and is_on_floor():
			jumping = false
			var dust_instance = dust.instance()
			add_child(effects)
			effects.add_child(dust_instance)
			dust_instance.init(global_transform, global_position)
		
		rset("r_position", position)
	else:
		position = r_position
		c_anim.play(r_animation)
		c_sprite.flip_h = r_flip_h
