extends Node2D

export (int) var fps = 24

### Input
puppetsync var flip_left = false # default right

puppetsync var jumping = false
puppetsync var crouching = false

puppetsync var moveRight = false
puppetsync var moveLeft = false
### ---

func _physics_process(delta):
	_sync(delta)

	if is_network_master():
		pass
	else:
		set_process_unhandled_input(false)

func _unhandled_input(event):
	if Input.is_action_just_pressed("move_left"):
		moveLeft = true
	if Input.is_action_just_released("move_left"):
		moveLeft = false

	if Input.is_action_just_pressed("move_right"):
		moveRight = true
	if Input.is_action_just_released("move_right"):
		moveRight = false

	# Crouch
	if Input.is_action_just_pressed("crouch"):
		crouching = true
	if Input.is_action_just_released("crouch"):
		crouching = false

	# Jump
	if Input.is_action_just_pressed("jump"):
		jumping = true
	if Input.is_action_just_released("jump"):
		jumping = false
		
	if event is InputEventMouseMotion:
		flip_left = get_global_mouse_position().x < get_parent().global_position.x

var timer = 0
func _sync(delta):
	timer += delta
	if timer > 1.0 / fps:
		timer -= 1.0 / fps
	else:
		return

	if is_network_master():
		rset_id(1, "moveLeft", moveLeft)
		rset_id(1, "moveRight", moveRight)
		rset_id(1, "jumping", jumping)
		rset_id(1, "crouching", crouching)
		rset_id(1, "flip_left", flip_left)
