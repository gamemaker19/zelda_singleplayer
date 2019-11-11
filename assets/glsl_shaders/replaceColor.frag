uniform sampler2D texture;
uniform vec3 origColor;
uniform vec3 replaceColor;
uniform vec3 origColor2;
uniform vec3 replaceColor2;

int areEqual(vec3 f1, vec3 f2)
{
  if(distance(f1, f2) < 0.05) return 1;
  return 0;
}

void main()
{
   vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);
   vec3 pixel3 = vec3(pixel.r, pixel.g, pixel.b);

   if(pixel.a > 0 && areEqual(pixel3, origColor) == 1)
   {
     gl_FragColor = vec4(replaceColor.r, replaceColor.g, replaceColor.b, 1); 
   }
   else if(pixel.a > 0 && areEqual(pixel3, origColor2) == 1)
   {
     gl_FragColor = vec4(replaceColor2.r, replaceColor2.g, replaceColor2.b, 1);
   }
   else
   {
     gl_FragColor = pixel;
   }
}