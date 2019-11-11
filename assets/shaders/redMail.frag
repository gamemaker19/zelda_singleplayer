texture al_tex;
sampler2D s = sampler_state {
   texture = <al_tex>;
};

int areEqual(float f1, float f2)
{
  if(abs(f1 - f2) < 0.05) return 1;
  return 0;
}

float4 ps_main(VS_OUTPUT Input) : COLOR0
{
   float4 pixel = tex2D(s, Input.TexCoord.xy);

   float3 origColor = float3(56/255.0f,144/255.0f,104/255.0f);
   float3 replaceColor = float3(184/255.0f,16/255.0f,32/255.0f);
   float3 origColor2 = float3(64/255.0f,216/255.0f,112/255.0f);
   float3 replaceColor2 = float3(240/255.0f,88/255.0f,136/255.0f);
   float3 origColor3 = float3(80/255.0f,144/255.0f,16/255.0f);
   float3 replaceColor3 = float3(152/255.0f,120/255.0f,216/255.0f);
   float3 origColor4 = float3(120/255.0f,184/255.0f,32/255.0f);
   float3 replaceColor4 = float3(200/255.0f,168/255.0f,248/255.0f);

   if(areEqual(pixel.r, origColor.r) == 1 && areEqual(pixel.b, origColor.b) == 1 && areEqual(pixel.g, origColor.g) == 1)
   {
     return float4(replaceColor.r, replaceColor.g, replaceColor.b, 1); 
   }
   if(areEqual(pixel.r, origColor2.r) == 1 && areEqual(pixel.b, origColor2.b) == 1 && areEqual(pixel.g, origColor2.g) == 1)
   {
     return float4(replaceColor2.r, replaceColor2.g, replaceColor2.b, 1);
   }
   if(areEqual(pixel.r, origColor3.r) == 1 && areEqual(pixel.b, origColor3.b) == 1 && areEqual(pixel.g, origColor3.g) == 1)
   {
     return float4(replaceColor3.r, replaceColor3.g, replaceColor3.b, 1);
   }
   if(areEqual(pixel.r, origColor4.r) == 1 && areEqual(pixel.b, origColor4.b) == 1 && areEqual(pixel.g, origColor4.g) == 1)
   {
     return float4(replaceColor4.r, replaceColor4.g, replaceColor4.b, 1);
   }
   return pixel;
}