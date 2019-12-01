//Shader "Hidden/EagleRender"
//{
//	Properties
//	{
//		_EagleTexture("_EagleTexture", 2D) = "white" {}
//	}
//
//	SubShader
//	{
//		Pass
//		{
//			CGPROGRAM
//			#pragma vertex vert
//			#pragma fragment frag
//
//			#include "UnityCG.cginc"
//
//			struct appdata
//			{
//				float4 vertex : POSITION;
//				float2 uv : TEXCOORD0;
//			};
//
//			struct v2f
//			{
//				float2 uv : TEXCOORD0;
//				float4 vertex : SV_POSITION;
//				//float depth : DEPTH;
//			};
//
//			v2f vert(appdata v)
//			{
//				v2f o;
//				o.vertex = UnityObjectToClipPos(v.vertex);
//				o.uv = v.uv;
//				//o.depth = -mul(UNITY_MATRIX_MVP, v.vertex).z * _ProjectionParams.w;
//				return o;
//			}
//
//			sampler2D _EagleTexture;
//
//			fixed4 frag(v2f i) : SV_Target
//			{
//				fixed4 colFond = tex2D(_EagleTexture, i.uv);
//				return fixed4(1, 0, 0, 1);
//				//return colFond;
//			}
//
//		ENDCG
//	}
//	}
//}


Shader "Custom/EagleRender"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_EagleTex("_EagleTex", 2D) = "white" {}
		_ScreenPartitionWidth("ScreenPartitionWidth",  Range(0.0, 1.0)) = 0.5
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag


				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				uniform sampler2D _EagleTex;
				float4 _MainTex_ST;
				float _ScreenPartitionWidth;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture

					fixed4 col = tex2D(_MainTex, i.uv);
					fixed4 eagle = tex2D(_EagleTex, i.uv);

					if (eagle.r > 0.9f)
					{
						if (col.r > 0.9f)
						{
							return fixed4(0.0, 0.0, 0.0, 1.0);
						}
						else
						{
							return fixed4(1.0, 1.0, 1.0, 1.0);
						}
					}
					
					return col;

					////Apply the perception brightness proportion for each color chanel
					//float brightness = col.x * 0.3 + col.y * 0.59 + col.z *  0.11;

					////If the uv x coordinate is higher than _ScreenPartitionWidth we apply the b&w effect, if not, we apply the image render how it is.
					//if (i.uv.x > _ScreenPartitionWidth)
					//{
					//	//This condition is done in order to draw a vertical line which is the frontier between the image processed and the normal image
					//	  if (abs(i.uv.x - _ScreenPartitionWidth) < 0.005)
					//		return fixed4(0.0,0.0,0.0,1.0);

					//	  //return fixed4(brightness, brightness, brightness, 1.0);
					//	  return eagle;
					//}
					//else 
					//{
					//	return col;
					//}
			}
			ENDCG
		}
		}
}