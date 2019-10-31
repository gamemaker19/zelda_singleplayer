uniform sampler2D texture;
uniform vec4 color;

void main()
{
   vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);
   
   if(distance(color, pixel) == 0)
   {
      gl_FragColor = vec4(0, 0, 0, 0); 
   }
   else
   {
      gl_FragColor = pixel * gl_Color;
   }
}

/*
uniform sampler2D texture;

void main()
{
    // lookup the pixel in the texture
    vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);

    // multiply it by the color
    gl_FragColor = gl_Color * pixel;
}
*/