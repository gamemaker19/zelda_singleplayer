uniform sampler2D texture;
uniform int palette;

/*
void main()
{
    // lookup the pixel in the texture
    vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);

    // multiply it by the color
    gl_FragColor = gl_Color * pixel;
}
*/

int areEqual(vec4 f1, vec4 f2)
{
  if(distance(f1, f2) == 0) return 1;
  return 0;
}

void main()
{
   vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);
   float firstRow = 441 / 895.0f;
   vec4 pixel1a = texture2D(texture, vec2(120/191.0f,firstRow));
   vec4 pixel2a = texture2D(texture, vec2(121/191.0f,firstRow));
   vec4 pixel3a = texture2D(texture, vec2(122/191.0f,firstRow));
   vec4 pixel4a = texture2D(texture, vec2(123/191.0f,firstRow));
   vec4 pixel5a = texture2D(texture, vec2(124/191.0f,firstRow));
   vec4 pixel6a = texture2D(texture, vec2(125/191.0f,firstRow));
   vec4 pixel7a = texture2D(texture, vec2(126/191.0f,firstRow));
   vec4 pixel8a = texture2D(texture, vec2(127/191.0f,firstRow));
   
   float nextRow = (441 + (palette * 2)) / 895.0f;

   vec4 pixel1b = texture2D(texture, vec2(120/191.0f,nextRow));
   vec4 pixel2b = texture2D(texture, vec2(121/191.0f,nextRow));
   vec4 pixel3b = texture2D(texture, vec2(122/191.0f,nextRow));
   vec4 pixel4b = texture2D(texture, vec2(123/191.0f,nextRow));
   vec4 pixel5b = texture2D(texture, vec2(124/191.0f,nextRow));
   vec4 pixel6b = texture2D(texture, vec2(125/191.0f,nextRow));
   vec4 pixel7b = texture2D(texture, vec2(126/191.0f,nextRow));
   vec4 pixel8b = texture2D(texture, vec2(127/191.0f,nextRow));

   //if(areEqual(pixel, pixel1a) == 1) gl_FragColor = pixel1b;
   if(areEqual(pixel, pixel2a) == 1) gl_FragColor = pixel2b;
   else if(areEqual(pixel, pixel3a) == 1) gl_FragColor = pixel3b;
   else if(areEqual(pixel, pixel4a) == 1) gl_FragColor = pixel4b;
   else if(areEqual(pixel, pixel5a) == 1) gl_FragColor = pixel5b;
   else if(areEqual(pixel, pixel6a) == 1) gl_FragColor = pixel6b;
   else if(areEqual(pixel, pixel7a) == 1) gl_FragColor = pixel7b;
   else if(areEqual(pixel, pixel8a) == 1) gl_FragColor = pixel8b;
   else gl_FragColor = pixel;
}