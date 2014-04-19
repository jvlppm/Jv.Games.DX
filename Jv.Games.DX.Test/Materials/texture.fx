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
    float4 color : COLOR0;
};

texture Tex;
sampler Samp = sampler_state    //sampler for doing the texture-lookup
{
    Texture = <gTexture>;       //apply a texture to the sampler
    MipFilter = LINEAR;         //sampler states
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

OutputVS ColorVS(float3 posL : POSITION0, float4 texCoord : TEXCOORD0)
{
    OutputVS outVS = (OutputVS)0;

    //Altera a cor de acordo com a altura
    outVS.color = tex2D(Samp, texCoord);

    //Transforma a posição
    outVS.pos = mul(float4(posL, 1.0f), gWVP);

    return outVS;
}

//Retorna a cor alterada
float4 ColorPS(float4 inColor : COLOR0) : COLOR
{
    return inColor;
}

technique TransformTech
{
    pass P0
    {
        // Especifica o vertex e pixel shader associado a essa passada.            
        vertexShader = compile vs_2_0 ColorVS();
        pixelShader = compile ps_2_0 ColorPS();

        //Especifica o device state associado a essa passada.
        FillMode = Solid;
    }
}