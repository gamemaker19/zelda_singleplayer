texture al_tex;
sampler2D s = sampler_state {
   texture = <al_tex>;
};

float4 ps_main(VS_OUTPUT Input) : COLOR0
{
   float4 pixel = tex2D(s, Input.TexCoord.xy);

   if(pixel.a == 0)
   {
      return float4(0,0,0,0);   
   }

   return float4(pixel.r + 80/255.0f, pixel.g + 175/256.0f, pixel.b + 225/256.0f, pixel.a);
}