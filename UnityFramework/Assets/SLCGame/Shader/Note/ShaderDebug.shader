Shader "ShaderNote/ShaderDebug"
{
	 
	SubShader
	{ 
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				fixed4 color : COLOR0;
			}; 
	 
			
			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				//可视化法线
				o.color  fixed4((v.normal * 0.5) + fixed3(0.5, .5, 0.5), 1.0);

				o.color = fixed4(v.tangent.xyz*0.5 + fixed3(0.5, 0.5, 0.5), 1.0);

				fixed3 binormal = cross(v.normal, v.tangent.xyz)*v.tangent.w;
				o.color = fixed4(binormal*0.5 + fixed3(0.5, 0.5, .5), 1.0);

				o.color = fixed4(v.texcoord.xy, 0.0, 1.0);
				o.color = fixed4(v.texcoord1.xy, 0.0, 1.0);

				o.color = frac(v.texcoord); 
				if (any(saturate(v.texcoord) - v.texcoord))
				{
					o.color.b = 0.5;
				}
				

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
