uniform sampler2D texture;

void main()
{
   vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);

   if(pixel.a == 0)
   {
      gl_FragColor = vec4(0,0,0,0);   
   }
   else
   {
      gl_FragColor = vec4(pixel.r + 80/255.0f, pixel.g + 175/256.0f, pixel.b + 225/256.0f, pixel.a);
   }
}