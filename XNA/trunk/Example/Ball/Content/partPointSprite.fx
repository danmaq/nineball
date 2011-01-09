////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	�Ԃ��� ���� �����Q�[��
//		Copyright (c) 1994-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

//* ���������������Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q_*
//* Global fields ��������������������������������������������������������-*

uniform extern float4x4 WVPMatrix;

uniform extern texture SpriteTexture;

//* �������������Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q_*
//* Structures  ����������������������������������������������������������-*

struct PS_INPUT {
	float4 color : COLOR0;
#ifdef XBOX360
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

//* ���������Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q_*
//* Samplar ��������������������������������������������������������������-*

sampler Sampler = sampler_state { Texture = <SpriteTexture>; };

//* ���������������Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q_*
//* Vertex shader ��������������������������������������������������������-*

VS_OUTPUT VertexShader(
	float4 pos : POSITION0, int index : INDEX, float4 color : COLOR0, float size : PSIZE0
) {
	VS_OUTPUT vsout;
	vsout.pos = mul( pos, WVPMatrix );
	vsout.color = color;
	vsout.size = size;
	return vsout;
}

//* ���������������Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q_*
//* Pixel shader  ��������������������������������������������������������-*

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

//* �����������Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q_*
//* Technique ������������������������������������������������������������-*

technique partPointSpriteTechnique {
    pass P0 {
        VertexShader = compile vs_1_1 VertexShader();
        PixelShader = compile ps_2_0 PixelShader();
    }
}
