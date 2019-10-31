texture al_tex;
sampler2D s = sampler_state {
   texture = <al_tex>;
};
float3 tint;
float4 ps_main(VS_OUTPUT Input) : COLOR0
{
   float4 pixel = tex2D(s, Input.TexCoord.xy);
   float4 replaceColor = float4(72/255.0f,160/255.0f,72/255.0f,1);  
   
   if(pixel.a == 0)
   {
      return replaceColor;
   }

   return pixel;
}

int areEqual(float f1, float f2)
{
	if(abs(f1 - f2) < 0.01) return 1;
	return 0;
}