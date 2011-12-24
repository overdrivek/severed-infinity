uniform sampler2D tex;
uniform sampler2D normalMap;
varying vec4 v_worldSpacePos;
varying vec3 normal;

const float ambientAmount = 0.3;
const float diffuseAmount = 0.05;
const float specularPower = 60.0;
const float specularAmount = 0.0;

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main()
{
	vec4 lightDir = vec4(v_worldSpacePos.x + 20, v_worldSpacePos.y + 20, v_worldSpacePos.z, 1.0);

	vec4 texColor = texture2D(tex, gl_TexCoord[0].xy);

	// eye position in world space
	vec4 eyePos = gl_ModelViewMatrixInverse * vec4(0.0, 0.0, 0.0, 1.0);
	eyePos /= eyePos.w;

	vec3 fallingVec = v_worldSpacePos.xyz - eyePos.xyz;
	fallingVec = normalize(fallingVec);
	vec3 reflected = normalize(reflect(lightDir, normal));

	float specularTerm = max(0.0, dot(reflected, fallingVec));
	specularTerm = pow(specularTerm, specularPower) * specularAmount;

	float diffuseTerm = max(0.0, dot(lightDir, normal));
	
	texColor *= diffuseTerm * diffuseAmount + ambientAmount;
	texColor += specularTerm;


	gl_FragColor = texColor;
	//texture2D(tex, gl_TexCoord[0].xy) * gl_Color;
}