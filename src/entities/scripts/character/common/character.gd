extends KinematicBody2D
class_name Character

### Special effects
var dust_effect # Will be added back once fall damage has been implemented
### ---

### Sync
puppetsync var r_position = Vector2(0, 0)
puppetsync var r_animation = ""
puppetsync var r_flip_h = false

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
	if c_controller.moveRight:
		velocity.x += walk_speed

	if c_controller.moveLeft:
		velocity.x -= walk_speed

	# Jump
	if c_controller.jumping && !c_controller.crouching && is_on_floor():
		velocity.y = -(game_map.gravity * jump_speed)

func _process_animation(_delta):
	if is_on_floor():
		if c_controller.moveLeft or c_controller.moveRight:
			_animate("walk", c_controller.moveLeft)
		else:
			if c_controller.crouching:
				_animate("crouch")
			else:
				_animate("idle")

		if c_controller.jumping:
			_animate("jump")

var timer = 0
func _sync(delta):
	timer += delta
	if timer > 1.0 / fps:
		timer -= 1.0 / fps
	else:
		return

	if is_network_master():
		rset_unreliable("r_animation", c_anim.current_animation)
		rset_unreliable("r_flip_h", c_controller.flip_h)
		rset_unreliable("r_position", position)
	else:
		_animate(r_animation, r_flip_h)
		p_position = position
### ---

func _animate(animation, t_flip_h = null):
	if c_anim.has_animation(animation):
		c_anim.play(animation)

	if t_flip_h != null && t_flip_h != c_controller.flip_h:
		c_controller.flip_h = t_flip_h

		c_sprite.scale.x *= -1
