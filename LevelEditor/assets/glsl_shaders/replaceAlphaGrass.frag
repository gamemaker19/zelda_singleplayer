uniform sampler2D texture;

void main()
{
   vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);
   vec4 replaceColor = vec4(72/255.0f,160/255.0f,72/255.0f,1);  
   
   if(pixel.a == 0)
   {
      gl_FragColor = replaceColor;
   }
   else
   {
      gl_FragColor = pixel;
   }
}