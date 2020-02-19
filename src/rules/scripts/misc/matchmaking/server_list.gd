extends Control

export (NodePath) var server_item

func _ready():
	_refresh()

func _refresh():
	for child in $Layout/Content.get_children():
		child.queue_free()

	var result = yield(network.api_socket.rpc_async("get_servers"), "completed")
	if result.is_exception():
		printerr(result.get_exception())
	else:
		_update(JSON.parse(result.payload).result)

func _update(servers):
	for server in servers:
		var item = get_node(server_item).duplicate()
		item.server_name = server.value.server_name
		item.server_ip = server.value.server_ip
		item.server_port = server.value.server_port
		item.show()

		$Layout/Content.add_child(item)
