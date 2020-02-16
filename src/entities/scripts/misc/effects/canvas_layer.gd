extends CanvasItem

export (int) var z_index = 0

func _ready():
	VisualServer.canvas_item_set_z_index(get_canvas_item(), z_index);
