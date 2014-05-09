//==================================================================
// Transforma a geometria para a perspectiva desejada
//
// Define a cor do pixel de acordo com a texture especificada.
//==================================================================

// Parâmetro do efeito (variável global no shader)
uniform extern float4x4 gWVP;
uniform extern texture gTexture;

// Estrutura
struct OutputVS
{
	float4 pos : POSITION0;
	float2 textCoord : TEXCOORD0;
};

sampler Samp = sampler_state       //sampler for doing the texture-lookup
{
	Texture = < gTexture > ;       //apply a texture to the sampler
	MinFilter = Anisotropic;
	MagFilter = None;
	MipFilter = Linear;
	MaxAnisotropy = 8;
	AddressU = WRAP;
	AddressV = WRAP;
};

OutputVS ColorVS(float3 posL : POSITION0, float2 texCoord : TEXCOORD0)
{
	OutputVS outVS = (OutputVS)0;
	outVS.textCoord = texCoord;

	//Transforma a posição
	outVS.pos = mul(float4(posL, 1.0f), gWVP);

	return outVS;
}

//Retorna a cor alterada
float4 ColorPS(float2 texCoord : TEXCOORD0) : COLOR
{
	//Altera a cor de acordo com a altura
	return tex2D(Samp, texCoord);
}

technique TransformTech
{
	pass P0
	{
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;
		BlendOp = Add;

		// Especifica o vertex e pixel shader associado a essa passada.            
		vertexShader = compile vs_2_0 ColorVS();
		pixelShader = compile ps_2_0 ColorPS();

		//Especifica o device state associado a essa passada.
		FillMode = Solid;
	}
}