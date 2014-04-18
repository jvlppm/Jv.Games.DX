//==================================================================
// Transforma a geometria para a perspectiva desejada
//
// Insere ondas radiais a partir de um ponto de origem. As ondas se 
// movem de acordo com um parâmetro de tempo.
//
// Altera também a luminosidade da cor do vértice de acordo com a 
// altura da onda
//==================================================================

// Parâmetro do efeito (variável global no shader)
uniform extern float4x4 gWVP;

uniform extern float gTime;
uniform extern float3 gSource;

// Estrutura
struct OutputVS
{
      float4 posH : POSITION0;
	  float4 color : COLOR0;
};

// Amplitude
static float a = 2.0f;

// Largura da onda (menor o número = mais grossa)
static float k = 0.2f;

// Frequência angular (velocidade das ondas)
static float w = 4.0f;

float RadialWaves(float3 position)
{
	//Calculamos a distância entre a origem das ondas e a posição do vértice
	float d = distance(gSource.xz, position.xz);

	//Calculamos a altura da onda
	return a*sin(k*d - gTime*w);
}

float4 GetIntensityFromHeight(float y)
{	
	float c = y / (2.0f*a) + 0.5f;
	return float4(c, c, c, 1.0f);
}

OutputVS ColorVS(float3 posL : POSITION0, float4 color : COLOR0)
{
      OutputVS outVS = (OutputVS)0;

	  //Altera a posição y de acordo com a onda.
	  posL.y = RadialWaves(posL) + posL.y;

	  //Altera a cor de acordo com a altura
      outVS.color = color * GetIntensityFromHeight(posL.y);

	  //Transforma a posição
      outVS.posH = mul(float4(posL, 1.0f), gWVP);      
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
            pixelShader  = compile ps_2_0 ColorPS();

			//Especifica o device state associado a essa passada.
            FillMode = WireFrame;
      }
}