uniform sampler2D tex;
uniform vec3 pal[256];
varying vec4 varying_color;
varying vec2 varying_texcoord;
void main()
{
  vec4 c = texture2D(tex, varying_texcoord);
  int index = int(c.r * 255);
  if (index != 0) 
  {
    gl_FragColor = vec4(pal[index], 1);
  }
  else 
  {
    gl_FragColor = vec4(0, 0, 0, 0);
  }
}