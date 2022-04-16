extends RayCast

var parent : Spatial

func _ready():
	parent = get_parent_spatial()
	pass

func _process(delta):
	var translation = parent.translation
	var destination = parent.translation + cast_to
	DrawLine3d.DrawLine(parent.translation, destination, Color.red)
	pass
