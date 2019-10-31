uniform sampler2D texture;

void main()
{
  vec4 flashColor = vec4(0.5,0.5,0.5,1);
  vec4 pixel_color = texture2D(texture, gl_TexCoord[0].xy);
  float percent = flashColor.a;

  pixel_color.r = pixel_color.r + flashColor.r * percent;
  pixel_color.g = pixel_color.g + flashColor.g * percent;
  pixel_color.b = pixel_color.b + flashColor.b * percent;

  gl_FragColor = pixel_color; 
}