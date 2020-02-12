extends KinematicBody2D
class_name Character

### Special effects
var dust_effect # Will be added back once fall damage has been implemented
### ---

### Sync
puppetsync var r_position = Vector2(0, 0)
puppetsync var r_animation = ""
puppetsync var r_flip_left = false

var p_position = Vector2(0, 0)
var p_jumping = false
### ---

onready var c_sprite = $Sprite
onready var c_anim = $Sprite/Animation
onready var c_body = $Sprite/Body
onready var c_head = $Sprite/Head
onready var c_name = $Name

onready var c_client = $Client
onready var c_controller = $Client/Controller

# Has information about the local player
var pinfo

### Physics
export (int) var fps = 24

export (float) var mass = 64
export (float) var gravity_scale = 1

export (float) var run_speed = 68
export (float) var walk_speed = 68
export (float) var jump_speed = 20
export (float) var facing_mod = 0.8

var velocity = Vector2(0, 0)
### ---

func _setup(t_pinfo):
	pinfo = t_pinfo
	set_name(str(pinfo.id))

	set_network_master(1)
	get_node("Client").set_network_master(pinfo.id)

func _ready():
	c_name.text = pinfo.name

	if c_client.is_network_master():
		game_camera.target = self

func _process(delta):
	if is_network_master():
		pass
	else:
		# TODO: Implement position interpolation
		position = r_position

func _physics_process(delta):
	if is_network_master():
		_process_input(delta)
		_process_animation(delta)

		velocity.y += (game_map.gravity * mass * gravity_scale) * delta
		velocity = move_and_slide(velocity, Vector2(0, -1))

	_sync(delta)

### Event processing
func _process_input(_delta):
	velocity.x = 0

	# Walk
	var _xvel = 0
	
	if c_controller.moveRight:
		_xvel = walk_speed * (facing_mod if c_controller.flip_left else 1)
		velocity.x += _xvel

	if c_controller.moveLeft:
		_xvel = -walk_speed * (1 if c_controller.flip_left else facing_mod)
		velocity.x += _xvel

	# Jump
	if c_controller.jumping && !c_controller.crouching && is_on_floor():
		velocity.y = -(game_map.gravity * jump_speed)

func _process_animation(_delta):
	if is_on_floor():
		if c_controller.moveLeft or c_controller.moveRight:
			_animate("walk")
		else:
			if c_controller.crouching:
				_animate("crouch")
			else:
				_animate("idle")

		if c_controller.jumping:
			_animate("jump")

	_animate(null, true) # airborne

var timer = 0
func _sync(delta):
	timer += delta
	if timer > 1.0 / fps:
		timer -= 1.0 / fps
	else:
		return

	if is_network_master():
		rset_unreliable("r_animation", c_anim.current_animation)
		rset_unreliable("r_position", position)
		rset_unreliable("r_flip_left", c_controller.flip_left)
	else:
		_animate(r_animation)
		_animate(null, true)
		p_position = position
### ---

func _animate(animation, airborne = false):
	if airborne:
		c_sprite.scale.x = -sign(float(r_flip_left) - 0.1)
		# if animation != null:
		# 	to add here: stuns, attacks, etc
		return
	
	if c_anim.has_animation(animation):
		c_anim.play(animation)
