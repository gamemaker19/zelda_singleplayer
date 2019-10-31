uniform sampler2D texture;

int areEqual(float f1, float f2)
{
  if(abs(f1 - f2) < 0.05) return 1;
  return 0;
}

void main()
{
   vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);

   vec3 origColor = vec3(56/255.0f,144/255.0f,104/255.0f);
   vec3 replaceColor = vec3(184/255.0f,16/255.0f,32/255.0f);
   vec3 origColor2 = vec3(64/255.0f,216/255.0f,112/255.0f);
   vec3 replaceColor2 = vec3(240/255.0f,88/255.0f,136/255.0f);
   vec3 origColor3 = vec3(80/255.0f,144/255.0f,16/255.0f);
   vec3 replaceColor3 = vec3(152/255.0f,120/255.0f,216/255.0f);
   vec3 origColor4 = vec3(120/255.0f,184/255.0f,32/255.0f);
   vec3 replaceColor4 = vec3(200/255.0f,168/255.0f,248/255.0f);

   if(areEqual(pixel.r, origColor.r) == 1 && areEqual(pixel.b, origColor.b) == 1 && areEqual(pixel.g, origColor.g) == 1)
   {
     gl_FragColor = vec4(replaceColor.r, replaceColor.g, replaceColor.b, 1); 
   }
   else if(areEqual(pixel.r, origColor2.r) == 1 && areEqual(pixel.b, origColor2.b) == 1 && areEqual(pixel.g, origColor2.g) == 1)
   {
     gl_FragColor = vec4(replaceColor2.r, replaceColor2.g, replaceColor2.b, 1);
   }
   else if(areEqual(pixel.r, origColor3.r) == 1 && areEqual(pixel.b, origColor3.b) == 1 && areEqual(pixel.g, origColor3.g) == 1)
   {
     gl_FragColor = vec4(replaceColor3.r, replaceColor3.g, replaceColor3.b, 1);
   }
   else if(areEqual(pixel.r, origColor4.r) == 1 && areEqual(pixel.b, origColor4.b) == 1 && areEqual(pixel.g, origColor4.g) == 1)
   {
     gl_FragColor = vec4(replaceColor4.r, replaceColor4.g, replaceColor4.b, 1);
   }
   else 
   {
     gl_FragColor = pixel;
   }
}