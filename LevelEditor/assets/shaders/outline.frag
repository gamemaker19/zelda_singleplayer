texture al_tex;
sampler2D s = sampler_state {
   texture = <al_tex>;
};
float3 tint;
int width;
int height;
float4 ps_main(VS_OUTPUT Input) : COLOR0
{
   float4 pixel = tex2D(s, Input.TexCoord.xy);
   if(pixel.a > 0) {
      return pixel;
   }
   
   float2 pos = Input.TexCoord.xy;
   float x = 1.0f / width;
   float y = 1.0f / height;

   float4 pixelTop = tex2D(s, float2(pos.x, pos.y - y));
   float4 pixelBot = tex2D(s, float2(pos.x, pos.y + y));
   float4 pixelLeft = tex2D(s, float2(pos.x - x, pos.y));
   float4 pixelRight = tex2D(s, float2(pos.x + x, pos.y));

   float4 pixelTop2 = tex2D(s, float2(pos.x, pos.y - y*2));
   float4 pixelBot2 = tex2D(s, float2(pos.x, pos.y + y*2));
   float4 pixelLeft2 = tex2D(s, float2(pos.x - x*2, pos.y));
   float4 pixelRight2 = tex2D(s, float2(pos.x + x*2, pos.y));

   float alpha = pixelTop.a + pixelBot.a + pixelLeft.a + pixelRight.a;
   if(alpha > 0) {
      return float4(tint.r, tint.g, tint.b, 1);
   }
   float alpha2 = pixelTop2.a + pixelBot2.a + pixelLeft2.a + pixelRight2.a;
   if(alpha2 > 0) {
      //return float4(tint.r * 0.5f, tint.g * 0.5f, tint.b * 0.5f, 0.5f);
   }
   return float4(0, 0, 0, 0);
}