﻿uniform sampler2D tex;

void main()
{
	gl_FragColor = vec4(1.0, 0.0, 1.0, 1.0);
	//texture2D(tex, gl_TexCoord[0].xy) * gl_Color;
}