uniform sampler2D tex;

void main()
{
	gl_FragColor = vec3(1.0f, 1.0f, 1.0f);//texture2D(tex, gl_TexCoord[0].xy) * gl_Color;
}