
// Calculate normal vector from 2 channel tangent space input, transform it to view space by using interpolated vertex normal/tangent/binormal
float3 normal2dTangentToViewSpace3d(float2 normalTangent2d, float3 tangent, float3 binormal, float3 normal, float normalMultiplier)
{
	float3 texNormal;
	texNormal.x = (texNCombine1.a * 2) - 1;
	texNormal.y = (texNCombine2.a * 2) - 1;
	texNormal.z = sqrt(saturate(1 - texNormal.x*texNormal.x - texNormal.y*texNormal.y));
	return normalize((texNormal.x) * tangent - (texNormal.y) * binormal + texNormal.z * normalP * (1.0f/normal));
}


// Calculate normal vector from 2 channel tangent space input, transform it to view space by using interpolated vertex normal/tangent/binormal
float2 normal2dTangentToViewSpace2d(float2 normalTangent2d, float3 tangent, float3 binormal, float3 normal, float normalMultiplier)
{
	float3 texNormal;
	texNormal.xy = normalTangent2d;
	texNormal.z = sqrt(saturate(1 - texNormal.x*texNormal.x - texNormal.y*texNormal.y));
	return normalize((texNormal.x) * tangent - (texNormal.y) * binormal + texNormal.z * normal * (1.0f/normalMultiplier)).xy;
}


void packUnitVector(inout float3 value)
{
	value = value * 0.5f + 0.5f;
}


void unpackUnitVector(inout float3 value)
{
	value = value * 2.0f - 1.0f;
}


float4 calculateVirtualTextureIndirection(float2 textureCoodinate)
{
	// Calculate virtual texture coordinate using indirection texture (1555/8888 packed)
	float3 indPix = floor(tex2Dbias(mapIndirectionSampler, float4(textureCoodinate, 0, 7)).xyz*255);
	float tileScale = exp2(indPix.z);
	float2 tileCoord = frac(textureCoodinate * tileScale + (1.0/512.0)) * (128.0/(128.0+8.0*2.0)) + (8.0/(128.0+8.0*2.0));		// EXP1, MUL2, FRAC2		(ATI)
	//float2 tileCoord = frac(textureCoodinate * tileScale) * (128.0/(128.0+8.0*2.0)) + (8.0/(128.0+8.0*2.0));					// EXP1, MUL2, FRAC2		(NVIDIA)
	return float4((tileCoord + indPix.xy) * vtScale, 0, 0);																		// MAD2
}


// Output pixel to the g-buffers
//outStruct.rtColorEmi = float4(texNCombine1.rgb * color.rgb, emissive * 0.5);
//outStruct.rtNormalSpec = float4(texNormal2, texNCombine2.bg * float2(spec, glossiness) * 0.5f);

// Output pixel to the g-buffers (light prepass version)
outStruct.rtColorSpec = float4(texNCombine1.rgb * color.rgb, texNCombine2.b * spec * 0.5);
outStruct.rtNormalGlos = float4(texNormal3, texNCombine2.g * glossiness * 0.5);


// Parallax with slope info (fast version with height + slope texture)
float3 pixelDirection = normalize(psIn.positionVS);
float2 texHeightSlope = height.Sample(heightSampler, textureCoordinate).xy;		// HEIGHT, SLOPE
float slope = texHeightSlope.y * 2.0 - 1.0;
float texShift = (texHeightSlope.x - 0.5) * displacement * -0.1 * slope;
texShift *= (1.0f/512.0f);
textureCoordinate.x += dot(tangent, pixelDirection) * texShift;		// TODO: Pixel direction calculations to vertex shader
textureCoordinate.y -= dot(binormal, pixelDirection) * texShift;

float3 calculateParallaxIndirection(inout float4 cacheTex, float3 positionVS, )
{
	// Parallax with slope info (fast version with height + slope texture)
	float3 pixelDirection = normalize(positionVS);
	float2 texHeightSlope = tex2Dlod(mapVMat3Sampler, cacheTex).xy;		// HEIGHT, SLOPE
	float slope = texHeightSlope.y * 2.0 - 1.0;
	float texShift = (texHeightSlope.x - 0.5) * displacement * -0.1 * slope;
	texShift *= tileScale * vtScale * (1.0f/512.0f);
	cacheTex.x += dot(tangent, pixelDirection) * texShift;		// TODO: Pixel direction calculations to vertex shader
	cacheTex.y -= dot(binormal, pixelDirection) * texShift;
}

// Sample material from the shifted coordinate (vt indirection + parallax shift)
float4 texNCombine1 = tex2Dlod(mapVMat1Sampler, cacheTex);			 	// RGB, NX
float4 texNCombine2 = tex2Dlod(mapVMat2Sampler, cacheTex);				// EX, Gl, Sp, NY


// Sample material from the shifted coordinate (parallax shift)
float4 texNCombine1 = ncombine1.Sample(ncombine1Sampler, textureCoordinate);		// RGB, NX
float4 texNCombine2 = ncombine2.Sample(ncombine2Sampler, textureCoordinate);		// EX, Gl, Sp, NY

