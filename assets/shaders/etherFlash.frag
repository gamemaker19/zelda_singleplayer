texture al_tex;
sampler2D s = sampler_state {
   texture = <al_tex>;
};
float4 ps_main(VS_OUTPUT Input) : COLOR0
{
   float4 pixel = tex2D(s, Input.TexCoord.xy);
   
   if(pixel.a == 0)
   {
      return pixel;
   }

   return float4(1 - pixel.r, 1 - pixel.g, 1 - pixel.b, pixel.a);
}

int areEqual(float f1, float f2)
{
	if(abs(f1 - f2) < 0.01) return 1;
	return 0;
}