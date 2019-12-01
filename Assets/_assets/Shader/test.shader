
Shader "Custom/test"
{
	Properties
	{
		_FoamStrength("Foam Strenght", Range(0, 50)) = 0
		_SpecularEdge("Specular Edge", Vector) = (1, 1, 1, 1)
		_SpecularDepth("Specular Depth", Vector) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue" = "Transparent" 
		}

		Stencil
		{
			Ref 0
			Comp Equal
		}

		Pass
		{

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform float _FoamStrength;
			uniform sampler2D _CameraDepthTexture; //Depth Texture
			uniform fixed4 _SpecularEdge, _SpecularDepth;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 screenPos : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.pos);
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				float sceneZ = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r);
				float objectZ = i.screenPos.z;

				if (sceneZ < 0.1)
				{
					return half4(0, 1, 0, 1);
				}

				return half4(sceneZ, sceneZ, sceneZ, 1);
				//float intensityFactor = 1 - saturate((sceneZ - objectZ) / _FoamStrength);
				//return lerp(_SpecularEdge, _SpecularDepth, intensityFactor);
			}


			ENDCG
		}
	}
		FallBack "VertexLit"
}