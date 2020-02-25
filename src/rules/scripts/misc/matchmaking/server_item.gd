extends Button

var server_name
var server_ip
var server_port

func _ready():
	connect("pressed", self, "_join")
	text = "%s (%s:%s)" % [server_name, server_ip, server_port]

func _join():
	Network.join_server(server_ip, server_port)
