extends Node2D
class_name Character

var move_speed = 300
puppet var repl_position = Vector2()

func _process(delta):
	if (is_network_master()):
		var move_dir = Vector2(0, 0)
		
		if (Input.is_action_pressed("move_up")):
			move_dir.y -= 1
		if (Input.is_action_pressed("move_down")):
			move_dir.y += 1
		if (Input.is_action_pressed("move_left")):
			move_dir.x -= 1
		if (Input.is_action_pressed("move_right")):
			move_dir.x += 1
		
		position += move_dir.normalized() * move_speed * delta
		
		rset("repl_position", position)
	else:
		position = repl_position
