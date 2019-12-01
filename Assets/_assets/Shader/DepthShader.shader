Shader "Custom/DepthShader"
{
	SubShader
	{
		Tags { "RenderType" = "Opaque" }

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

			uniform sampler2D _CameraDepthTexture;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 projPos : TEXCOORD1;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.projPos = ComputeScreenPos(o.pos);

				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				float depth = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)).r);
				
				half4 c;

				c.r = depth * 3;
				c.g = depth * 3;
				c.b = depth * 3;
				c.a = 1;

				return c;
			}

			ENDCG
		}
	}
	FallBack "VertexLit"
}