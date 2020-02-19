extends CanvasLayer

### Info
onready var username = $Info/Layout/Username
###

### PanelHost
onready var hostServerName = $PanelHost/Content/txtServerName
onready var hostServerPort = $PanelHost/Content/txtServerPort
### ---

### PanelJoin
onready var joinServerIP = $PanelJoin/Content/txtJoinIP
onready var joinServerPort = $PanelJoin/Content/txtJoinPort
### ---

func _ready():
	username.text = network.player.name

	$Refresh.connect("pressed", $Server_List, "_refresh")
	network.connect("connection_established", self, "_on_connection_established")
	network.connect("connection_closed", self, "_on_connection_closed")

func _on_btCreate_pressed():
	var name = hostServerName.text
	var port = int(joinServerPort.text)

	network._create_server(name, port)

func _on_btJoin_pressed():
	var ip = joinServerIP.text
	var port = int(joinServerPort.text)

	network._join_server(ip, port)

### --- Events

func _on_connection_established():
	if get_tree().change_scene("res://rules/content/multiplayer.tscn") != OK:
		push_error("Failed loading the scene.")

func _on_connection_closed():
	print("Failed to join server.")
