varying vec4 v_worldSpacePos;
varying vec3 normal;

void main()
{
	v_worldSpacePos = gl_Vertex;
	normal = gl_NormalMatrix * gl_Normal;

	gl_FrontColor = gl_Color;
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_Position = ftransform();
}