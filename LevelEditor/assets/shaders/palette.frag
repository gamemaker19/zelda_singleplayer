texture al_tex;
sampler2D s = sampler_state {
   texture = <al_tex>;
};
int palette;

int areEqual(float4 f1, float4 f2)
{
  if(distance(f1, f2) == 0) return 1;
  return 0;
}

float4 ps_main(VS_OUTPUT Input) : COLOR0
{
   float4 pixel = tex2D(s, Input.TexCoord.xy);
   float firstRow = 441 / 895.0f;
   float4 pixel1a = tex2D(s, float2(120/191.0f,firstRow));
   float4 pixel2a = tex2D(s, float2(121/191.0f,firstRow));
   float4 pixel3a = tex2D(s, float2(122/191.0f,firstRow));
   float4 pixel4a = tex2D(s, float2(123/191.0f,firstRow));
   float4 pixel5a = tex2D(s, float2(124/191.0f,firstRow));
   float4 pixel6a = tex2D(s, float2(125/191.0f,firstRow));
   float4 pixel7a = tex2D(s, float2(126/191.0f,firstRow));
   float4 pixel8a = tex2D(s, float2(127/191.0f,firstRow));
   
   float nextRow = (441 + (palette * 2)) / 895.0f;

   float4 pixel1b = tex2D(s, float2(120/191.0f,nextRow));
   float4 pixel2b = tex2D(s, float2(121/191.0f,nextRow));
   float4 pixel3b = tex2D(s, float2(122/191.0f,nextRow));
   float4 pixel4b = tex2D(s, float2(123/191.0f,nextRow));
   float4 pixel5b = tex2D(s, float2(124/191.0f,nextRow));
   float4 pixel6b = tex2D(s, float2(125/191.0f,nextRow));
   float4 pixel7b = tex2D(s, float2(126/191.0f,nextRow));
   float4 pixel8b = tex2D(s, float2(127/191.0f,nextRow));

   //if(areEqual(pixel, pixel1a) == 1) return pixel1b;
   if(areEqual(pixel, pixel2a) == 1) return pixel2b;
   else if(areEqual(pixel, pixel3a) == 1) return pixel3b;
   else if(areEqual(pixel, pixel4a) == 1) return pixel4b;
   else if(areEqual(pixel, pixel5a) == 1) return pixel5b;
   else if(areEqual(pixel, pixel6a) == 1) return pixel6b;
   else if(areEqual(pixel, pixel7a) == 1) return pixel7b;
   else if(areEqual(pixel, pixel8a) == 1) return pixel8b;
   return pixel;
}