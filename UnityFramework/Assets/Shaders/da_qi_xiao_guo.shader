Shader "Game/Atmosphere"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5, 0.5, 1.0, 1)
		_Falloff("Rim Falloff", Range(4.0, 25.0)) = 6.0
	}
		SubShader
	{
		LOD 250
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
	}
		Cull Front
		ZWrite Off
		Blend SrcAlpha One // NOTE: Comment this out and add *alpha* to #pragma below to disable additive blending
		Fog{ Color(0,0,0,0) }
		CGPROGRAM
#pragma surface surf Lambert vertex:vert
		fixed4 _Color;
	float _Falloff;
	struct Input
	{
		float3 worldView; // World-space view direction (direction from vertex to camera)
		float3 worldNormal; // Built-in, automatically written to if not overwritten
	};
	void vert(inout appdata_full v, out Input o)
	{
		float4 pos = mul(_Object2World, v.vertex);
		float3 worldPos = pos.xyz / pos.w;
		o.worldView = worldPos - _WorldSpaceCameraPos;
	}
	void surf(Input IN, inout SurfaceOutput o)
	{
		float3 worldNormal = normalize(IN.worldNormal);
		float3 worldView = normalize(IN.worldView);
		float tint = dot(worldNormal, worldView);
		tint = clamp(tint * 2.0, 0.0, 1.0);
		tint = pow(tint, _Falloff);
		o.Albedo = _Color.rgb;
		o.Alpha = _Color.a * tint;
	}
	ENDCG
	}
		Fallback Off
}