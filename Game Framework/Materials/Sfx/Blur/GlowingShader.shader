Shader "Unlit/GlowingShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		singleStepOffset("step", float) = 0.003
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
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
						float2 uv0 : TEXCOORD0;
						float2 uv1 : TEXCOORD1;
						float2 uv2 : TEXCOORD2;
						float2 uv3 : TEXCOORD3;
						float2 uv4 : TEXCOORD4;
						float4 vertex : SV_POSITION;
					};

					sampler2D _MainTex;
					float4 _MainTex_ST;
					float singleStepOffset;

					v2f vert(appdata v)
					{
						v2f o;
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.uv0 = TRANSFORM_TEX(v.uv, _MainTex);
						o.uv1 = v.uv + singleStepOffset * 1.407333;
						o.uv2 = v.uv - singleStepOffset * 1.407333;
						o.uv3 = v.uv + singleStepOffset * 3.294215;
						o.uv4 = v.uv - singleStepOffset * 3.294215;

						return o;
					}

					   fixed4 frag(v2f i) : SV_Target
					   {
						   //1
						   //fixed4 px = tex2D(_MainTex, i.uv);
						   //px.a = 0.8 - px.a;
						   //return px;

						   //2
						   //fixed4 p0 = tex2D(_MainTex, i.uv + fixed2(-0.002, 0.001));
						   //fixed4 p1 = tex2D(_MainTex, i.uv + fixed2(0.0, 0.0));
						   //fixed4 p6 = tex2D(_MainTex, i.uv + fixed2(0.001, -0.002));
						   //
						   //float alpha = (p0.a * 0.7 + p6.a * 1.3) * 0.5;
						   //
						   //return fixed4(0, 0, 0, 0.8 - alpha);

						   //3
						   fixed4 color = tex2D(_MainTex, i.uv0) * 0.204164;
						   color += tex2D(_MainTex, i.uv1) * 0.304005;
						   color += tex2D(_MainTex, i.uv2) * 0.304005;
						   color += tex2D(_MainTex, i.uv3) * 0.093913;
						   color += tex2D(_MainTex, i.uv4) * 0.093913;
						   return fixed4(0, 0, 0, 0.8 - color.a);
					   }
					ENDCG
				}



		}
}
