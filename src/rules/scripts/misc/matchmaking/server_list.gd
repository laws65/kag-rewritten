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
		var json_object = JSON.parse(result.payload)
		_update(json_object.result)

func _update(servers):
	print(servers)
	for server in servers:
		var item = get_node(server_item).duplicate()
		item.server_name = server.server_name
		item.server_ip = server.server_ip
		item.server_port = server.server_port
		item.show()

		$Layout/Content.add_child(item)
