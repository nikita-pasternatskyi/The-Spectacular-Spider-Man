//By David Lipps aka DaveTheDev @ EXPWorlds
//v2.0.0 for Godot 3.2

shader_type spatial;
render_mode unshaded;
render_mode ambient_light_disabled;

void fragment(){
	vec4 t = inverse(WORLD_MATRIX) * CAMERA_MATRIX * vec4(VERTEX, 1.0);
    ALBEDO = vec3(t.z,0,0);
}