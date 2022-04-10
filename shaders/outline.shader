shader_type spatial;
render_mode cull_front, unshaded, ambient_light_disabled;

uniform bool use_texture = false;
uniform sampler2D outline_texture_color : hint_albedo;
uniform float outline_thickness : hint_range(0.0, 1.0, 0.001) = 0.005;
uniform vec4 outline_color : hint_color = vec4(0.0, 0.0, 0.0, 1.0);
uniform vec4 c : hint_color;

void vertex()
{
	VERTEX = (NORMAL * outline_thickness) + VERTEX;
}

void fragment()
{
	if(use_texture == false)
	{
		ALBEDO = outline_color.rgb;
	}
	else
	{
		vec4 base = texture(outline_texture_color, UV).rgba * outline_color.rgba;
		ALBEDO = base.rgb;
		
	}
}