#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
uniform float2 SpriteDimensions;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
	
    if (color.a != 0)
    {
        float stepX = 0.0f;
        float stepY = 0.0f;
        int inlineThickness = 2;
        for(int i = 0; i < inlineThickness; i++)
        {
            stepX += 0.001f;
            stepY += 0.002f;
            float4 pixelUp = tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(0, stepY));
            float4 pixelDown = tex2D(SpriteTextureSampler, input.TextureCoordinates - float2(0, stepY));
            float4 pixelRight = tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(stepX, 0));
            float4 pixelLeft = tex2D(SpriteTextureSampler, input.TextureCoordinates - float2(stepX, 0));
		
            if (pixelUp.a * pixelDown.a * pixelRight.a * pixelLeft.a == 0.0)
            {
                return float4(0, 0, 0, 1);
            }
        }

        int shadowThickness = 3;
        for(int j = 0; j < shadowThickness; j++)
        {
            stepX += 0.001f;
            stepY += 0.002f;
            float4 pixelUp = tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(0, stepY));
            float4 pixelUpRight = tex2D(SpriteTextureSampler, input.TextureCoordinates + float2(stepX, stepY));
            if (pixelUp.a * pixelUpRight.a  == 0.0)
            {
                return float4(0, 0, 0, 1);
            }
        }
    }
    
    return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};