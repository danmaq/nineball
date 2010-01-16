////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	�Ԃ��� ���� �����Q�[��
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

uniform extern float4x4 WVPMatrix;

uniform extern texture SpriteTexture;

struct PS_INPUT {
	float4 color : COLOR0;
#ifdef XBOX
	float2 TexCoord : SPRITETEXCOORD;
#else
	float2 TexCoord : TEXCOORD;
#endif
};

struct VS_OUTPUT {
	float4 color : COLOR0;
	float4 pos : POSITION0;
	float size : PSIZE0;
};

sampler Sampler = sampler_state { Texture = <SpriteTexture>; };

float4 PixelShader( PS_INPUT input ) : COLOR0 {
	float2 texCoord;
	float index = input.color.a;
	index *= 16;
	int x = index % 4;
	int y = index / 4;
	input.color.a = 1.0;
	float xx = 1.0f/4.0f*(float)x+input.TexCoord.x/4.0f;
	float yy = 1.0f/4.0f*(float)y+input.TexCoord.y/4.0f;
	texCoord.x = xx;
	texCoord.y = yy;
	return tex2D( Sampler, texCoord );
}

VS_OUTPUT VertexShader(
	float4 pos : POSITION0, int index : INDEX, float4 color : COLOR0, float size : PSIZE0
) {
	VS_OUTPUT vsout;
	vsout.pos = mul( pos, WVPMatrix );
	vsout.color = color;
	vsout.size = size;
	return vsout;
}

technique partPointSpriteTechnique {
    pass P0 {
        VertexShader = compile vs_1_1 VertexShader();
        PixelShader = compile ps_2_0 PixelShader();
    }
}
