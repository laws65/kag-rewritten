extends CanvasLayer

### Info
onready var username = $Info/Layout/Username
###

### PanelHost
onready var hostServerName = $PanelHost/Content/txtServerName
onready var hostServerPort = $PanelHost/Content/txtServerPort
onready var hostServerPrivate = $PanelHost/Content/btPrivate
### ---

### PanelJoin
onready var joinServerIP = $PanelJoin/Content/txtJoinIP
onready var joinServerPort = $PanelJoin/Content/txtJoinPort
### ---

func _ready():
	username.text = Network.player.name

func _on_btCreate_pressed():
	var name = hostServerName.text
	var port = int(joinServerPort.text)
	var is_private = hostServerPrivate.pressed

	Network.create_server(name, port, is_private)

func _on_btJoin_pressed():
	var ip = joinServerIP.text
	var port = int(joinServerPort.text)

	Network.join_server(ip, port)

### --- Events
