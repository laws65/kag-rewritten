extends KinematicBody2D
class_name Character

### Special effects
onready var dust_effect = load("res://base/entities/content/effects/dust.tscn")
### ---

### Sync
puppet var r_position = Vector2(0, 0)
puppet var r_animation = ""
puppet var r_flip_h = false
### ---

onready var c_anim = $Sprite/Animation
onready var c_body = $Sprite/Body
onready var c_head = $Sprite/Head
onready var c_name = $Name

### Physics
export (int) var gravity = 500
export (int) var move_speed = 75
export (int) var jump_speed = 160

var velocity = Vector2(0, 0)
### ---

### Input
var flip_h = false

var jumping = false
var crouching = false

var moveRight = false
var moveLeft = false
### ---

func _ready():
	if is_network_master():
		game_camera.target = self
	
	if get_network_master() in network.players:
		var pinfo = network.players[get_network_master()]
		print(network.players.size())
		c_name.text = pinfo.name

func _animate(animation, t_flip_h = null):
	if c_anim.has_animation(animation):
		c_anim.play(animation)
	
	if t_flip_h != null && t_flip_h != flip_h:
		flip_h = t_flip_h
		
		$Sprite.scale.x *= -1

func _physics_process(delta):
	if is_network_master():
		_process_input()
		_process_animation()
		
		velocity.y += gravity * delta
		velocity = move_and_slide(velocity, Vector2(0, -1))
	
	_sync()

### Event processing
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
	if Input.is_action_pressed("jump") and not (jumping or crouching) and is_on_floor():
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
		
		if jumping:
			_animate("jump")

func _sync():
	if is_network_master():
		if jumping and is_on_floor():
			jumping = false
			rpc_unreliable("_play_dust_effect", global_position)
		
		rset_unreliable("r_animation", c_anim.current_animation)
		rset_unreliable("r_flip_h", flip_h)
		rset_unreliable("r_position", position)
	else:
		_animate(r_animation, r_flip_h)
		position = r_position
### ---

### Remote events
remotesync func _play_dust_effect(pos):
	var dust = dust_effect.instance()
	dust.set_global_position(pos)
	
	get_node("/root").add_child(dust)
### ---
