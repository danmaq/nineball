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

float4x4 World;
float4x4 View;
float4x4 Projection;

//* �������������Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q_*
//* Structures  ����������������������������������������������������������-*

struct VS_OUTPUT {
	float4 pos : POSITION0;
	float size : PSIZE0;
};

//* ���������������Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q_*
//* Vertex shader ��������������������������������������������������������-*

VS_OUTPUT VertexShader(float4 position : POSITION0, float size : PSIZE0)
{
	VS_OUTPUT output;
    float4 worldPosition = mul(position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.pos = mul(viewPosition, Projection);
    output.size = size;
    return output;
}

//* ���������������Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q_*
//* Pixel shader  ��������������������������������������������������������-*

float4 PixelShader(VS_OUTPUT input) : COLOR0
{
    return float4(1, 1, 1, 1);
}

//* �����������Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q�Q_*
//* Technique ������������������������������������������������������������-*

technique XORTechnique
{
    pass Pass1
    {
		Lighting = FALSE;
		ZWriteEnable = FALSE;
		AlphaBlendEnable = TRUE;
		SrcBlend = INVDESTCOLOR;
		DestBlend = ZERO;
		
        VertexShader = compile vs_1_1 VertexShader();
        PixelShader = compile ps_1_1 PixelShader();
    }
}
