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
	network.connect("join_success", self, "_on_join_success")
	network.connect("join_fail", self, "_on_join_fail")

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

func _on_join_success():
	if get_tree().change_scene("res://base/levels/content/multiplayer.tscn") != OK:
		push_error("Loading the _on_ready_to_play() scene failed.")

func _on_join_fail():
	print("Failed to join server")
