Shader "SLCGame/Brightness Saturation And Contrast" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Brightness("Brightness", Float) = 1
		_Saturation("Saturation", Float) = 1
		_Contrast("Contrast", Float) = 1
	}
		SubShader{
			Pass{
			ZTest Always 
			Cull Off 
			ZWrite Off

			CGPROGRAM
	#pragma vertex vert  
	#pragma fragment frag  

	#include "UnityCG.cginc"  

			sampler2D _MainTex;
			half _Brightness;
			half _Saturation;
			half _Contrast;

			struct v2f {
				float4 pos : SV_POSITION;
				half2 uv: TEXCOORD0;
			};

			v2f vert(appdata_img v) {
				v2f o;

				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				o.uv = v.texcoord;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target{
				fixed4 renderTex = tex2D(_MainTex, i.uv);

				//亮度
				fixed3 finalColor = renderTex.rgb * _Brightness;

				//对比度
				fixed luminance = 0.2125 * renderTex.r + 0.7154 * renderTex.g + 0.0721 * renderTex.b;
				fixed3 luminanceColor = fixed3(luminance, luminance, luminance);
				finalColor = lerp(luminanceColor, finalColor, _Saturation);

				//饱和度
				fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
				finalColor = lerp(avgColor, finalColor, _Contrast);

				return fixed4(finalColor, renderTex.a);
			}

			ENDCG
		}
	}

		Fallback Off
}
