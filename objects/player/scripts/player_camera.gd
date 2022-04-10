extends SpringArm

export var mouse_sensitivity := 0.1

onready var _player = get_parent()

func _ready():
	set_as_toplevel(true)


func _process(_delta):
	translation = _player.translation

func _input(event):
	if(Input.is_action_just_pressed("ui_cancel")):
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	if(Input.is_action_just_pressed("throw_web")):
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	if event is InputEventMouseMotion:
		rotation_degrees.y -= event.relative.x * mouse_sensitivity
		rotation_degrees.y = wrapf(rotation_degrees.y, 0, 360)
		rotation_degrees.x -= event.relative.y * mouse_sensitivity
		rotation_degrees.x = clamp(rotation_degrees.x, -90, 90)
