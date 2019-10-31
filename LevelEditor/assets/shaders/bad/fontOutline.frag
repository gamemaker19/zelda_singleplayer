texture al_tex;
sampler2D s = sampler_state {
   texture = <al_tex>;
};
float3 tint;
float4 ps_main(VS_OUTPUT Input) : COLOR0
{
   float4 pixel = tex2D(s, Input.TexCoord.xy);
   if(pixel.a > 0) {
      return pixel;
   }
   
   float2 pos = Input.TexCoord.xy;

   float4 pixelTop = tex2D(s, float2(pos.x, pos.y - 1));
   float4 pixelBot = tex2D(s, float2(pos.x, pos.y + 1));
   float4 pixelLeft = tex2D(s, float2(pos.x - 1, pos.y));
   float4 pixelRight = tex2D(s, float2(pos.x + 1, pos.y));

   float alpha = pixelTop.a + pixelBot.a + pixelLeft.a + pixelRight.a;
   if(alpha > 0) {
      return float4(0, 0, 1, 1);
   }
   return float4(0, 0, 0, 0);
}