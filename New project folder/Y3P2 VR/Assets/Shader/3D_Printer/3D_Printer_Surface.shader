Shader "Custom/3D_Print" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color",Color) = (1,1,1,1)
		_PrintColor("Print Color", Color) = (1,1,1,1)
		_CreationY("CreationY", float) = 0
		_BuildGap("Gap",float) = 0
	}

	SubShader {
		Tags { "RenderType" = "Opaque" }
		Cull Off
		CGPROGRAM

		#pragma surface surf Custom fullforwardshadows
		#include "UnityPBSLighting.cginc"
		
		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 viewDir;
		};

		sampler2D _MainTex;
		float _CreationY;
		float _BuildGap;
		float4 _PrintColor;
		float4 _Color;

		int building;
		float3 viewDir;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			viewDir = IN.viewDir;

			if(IN.worldPos.y > _CreationY +  _BuildGap){
				discard;
			}else if(IN.worldPos.y < _CreationY) {
				fixed4 c = tex2D(_MainTex,IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
				building = 0;
			} else {
				o.Albedo = _PrintColor.rgb;
				o.Alpha = _PrintColor.a;
				building = 1;
			}
		}

		inline half4 LightingCustom(SurfaceOutputStandard s, half3 lightDir, UnityGI gi)
		{
			if (building)
				return _PrintColor;

			if ( dot(s.Normal, viewDir) < 0)
				return _PrintColor;
				
			return LightingStandard(s, lightDir, gi);
		}

		inline void LightingCustom_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
		{
			LightingStandard_GI(s, data, gi);		
		}

		ENDCG
	} 
	Fallback "Diffuse"
}