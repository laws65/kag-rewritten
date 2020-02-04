extends Node2D

### Input
puppetsync var flip_h = false

puppetsync var jumping = false
puppetsync var crouching = false

puppetsync var moveRight = false
puppetsync var moveLeft = false
### ---

func _ready():
	pass

func _physics_process(_delta):
	if is_network_master():
		_process_input()

func _process_input():
	# Walk
	if Input.is_action_pressed("move_right"):
		rset_unreliable_id(1, "moveRight", true)
	else:
		rset_unreliable_id(1, "moveRight", false)
	
	if Input.is_action_pressed("move_left"):
		rset_unreliable_id(1, "moveLeft", true)
	else:
		rset_unreliable_id(1, "moveLeft", false)
	
	# Jump
	if Input.is_action_pressed("jump"):
		rset_unreliable_id(1, "jumping", true)
	
	# Crouch
	if Input.is_action_pressed("crouch"):
		rset_unreliable_id(1, "crouching", true)
	else:
		rset_unreliable_id(1, "crouching", false)
