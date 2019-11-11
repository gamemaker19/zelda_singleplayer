texture al_tex;
sampler2D s = sampler_state {
   texture = <al_tex>;
};
float3 origColor;
float3 replaceColor;
float3 origColor2;
float3 replaceColor2;

int areEqual(float f1, float f2)
{
  if(abs(f1 - f2) < 0.05) return 1;
  return 0;
}

float4 ps_main(VS_OUTPUT Input) : COLOR0
{
   float4 pixel = tex2D(s, Input.TexCoord.xy);

   /*
   float3 origColor = float3(41/255.0f,123/255.0f,57/255.0f);
   float3 replaceColor = float3(104/255.0f,104/255.0f,40/255.0f);
   float3 origColor2 = float3(72/255.0f,152/255.0f,72/255.0f);
   float3 replaceColor2 = float3(144/255.0f,128/255.0f,64/255.0f);
   */

   if(areEqual(pixel.r, origColor.r) == 1 && areEqual(pixel.b, origColor.b) == 1 && areEqual(pixel.g, origColor.g) == 1)
   {
     return float4(replaceColor.r, replaceColor.g, replaceColor.b, 1); 
   }
   if(areEqual(pixel.r, origColor2.r) == 1 && areEqual(pixel.b, origColor2.b) == 1 && areEqual(pixel.g, origColor2.g) == 1)
   {
     return float4(replaceColor2.r, replaceColor2.g, replaceColor2.b, 1);
   }
   return pixel;
}