extends Node2D

### Input
puppetsync var flip_h = false

puppetsync var jumping = false
puppetsync var crouching = false

puppetsync var moveRight = false
puppetsync var moveLeft = false
### ---

export (int) var fps = 24

func _ready():
	pass

func _process(delta):
	_sync(delta)

func _unhandled_input(event):
	if not is_network_master():
		return
	
	# Walk
	if Input.is_action_pressed("move_left"):
		moveLeft = true
	else:
		moveLeft = false
		
	if Input.is_action_pressed("move_right"):
		moveRight = true
	else:
		moveRight = false
	
	# Jump
	if Input.is_action_just_pressed("jump"):
		jumping = true
	
	if Input.is_action_just_released("jump"):
		jumping = false
	
	# Crouch
	if Input.is_action_pressed("crouch"):
		crouching = true
	else:
		crouching = false

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
