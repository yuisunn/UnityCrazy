// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "ShaderNote/ShaderLight" {
	Properties{
		_Color("Diffuse", Color) = (1,1,1,1)

	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members pos)
#pragma exclude_renderers d3d11 xbox360
			#pragma vertex vert
			#pragma fragment frag

			#include "Lighting.cginc"

			fixed4 _diffuse;
			
			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0
			};

			struct v2f
			{
				float4 pos ： SV_POSITION;
				fixed3 color： COLOR;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;  //wrap mode  repeat 纹理坐标超过1 整数部分被舍弃  clamp 1，0就是最大最小 超过了还按他们算
				//贴图滤波
				//1point 只采集周围一个像素
				//2bilinear 采集周围4个像素
				//3triliner
				//纹理大小
				//纹理大小超过max textrue size 就会被缩放到这个大小 
				//texture大小永远是2的几次幂 否则占用更多 内存
			};


			v2f vert(a2v v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				fixed3 worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));


			}
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
