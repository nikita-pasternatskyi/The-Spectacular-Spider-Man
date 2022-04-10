extends Node

var angle_cone := deg2rad(90.0)
var max_distance := 150.0
var angle_between_rays := deg2rad(2.0)

func _ready():
	var raycount := angle_cone / angle_between_rays
	for index in raycount:
		var newRay := RayCast.new()
		var angle = angle_between_rays * (index - raycount / 2.0)
		newRay.cast_to = -Vector3.FORWARD.rotated(Vector3.UP,angle) * max_distance
		add_child(newRay)
		newRay.enabled = true
		print(newRay.name)
		

func _check():
	for child in get_children():
		if(child is RayCast):
			var cast := (child as RayCast).is_colliding()
			if(cast == true):
				print("true")

	pass
