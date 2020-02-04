extends CanvasLayer

### PanelPlayer
onready var playerName = $Layout/PanelPlayer/Content/txtPlayerName
### ---

### PanelHost
onready var hostServerName = $Layout/PanelHost/Content/txtServerName
onready var hostServerPort = $Layout/PanelHost/Content/txtServerPort
### ---

### PanelJoin
onready var joinServerIP = $Layout/PanelJoin/Content/txtJoinIP
onready var joinServerPort = $Layout/PanelJoin/Content/txtJoinPort
### ---

func _ready():
	network.connect("connection_established", self, "_on_connection_established")
	network.connect("connection_closed", self, "_on_connection_closed")

func set_player_info():
	if (!playerName.text.empty()):
		network.player.name = playerName.text

func _on_btCreate_pressed():
	set_player_info()
	
	var name = hostServerName.text
	var port = int(joinServerPort.text)
	
	network.create_server(name, port)

func _on_btJoin_pressed():
	set_player_info()
	
	var ip = joinServerIP.text
	var port = int(joinServerPort.text)
	
	network.join_server(ip, port)

### --- Events

func _on_connection_established():
	if get_tree().change_scene("res://base/levels/content/multiplayer.tscn") != OK:
		push_error("Failed loading the scene.")

func _on_connection_closed():
	print("Failed to join server.")
