extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
onready var label = $RichTextLabel2

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


func _on_StateMachine_StateChanged(n):
	label.text = (n as Node).name
	pass # Replace with function body.
