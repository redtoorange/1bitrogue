shader_type canvas_item;

uniform vec2 backDropSize;
uniform vec2 area;
uniform vec2 size;

bool areaContains(	vec2 point,
					vec2 scaledArea,
					vec2 scaledSize) {
	bool insideX = point.x >= scaledArea.x && point.x <= (scaledArea.x + scaledSize.x);
	if(insideX) {
		bool insideY = point.y >= scaledArea.y && point.y <= (scaledArea.y + scaledSize.y);		
		if(insideY){
			return true;
		}
	}
	return false;
}

void fragment()
{
	vec2 scaledArea = area / backDropSize;
	vec2 scaledSize = size / backDropSize;
	
	if(areaContains(UV.xy, scaledArea, scaledSize)) {
		COLOR = vec4(COLOR.rgb, 0);
	}
}