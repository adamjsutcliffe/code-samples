﻿Shader "Hidden/SoftGuiMaskShader"
{
    Properties
    {
        _Color("Tint", Color) = (1.000000,1.000000,1.000000,1.000000)
        _MainTex("Texture", 2D) = "white" {}
        _MaskTex("Mask", 2D) = "white" {}
        _TilingAndOffset("TilingAndOffset", Vector) = (0,0,0,0)
            
        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0.000000
    }

    SubShader
    {
        Tags
        { 
            "Queue" = "Transparent" 
            "IgnoreProjector" = "true" 
            "RenderType" = "Transparent" 
            "PreviewType" = "Plane" 
            "CanUseSpriteAtlas" = "true" 
        }

        // Stencil props
        Stencil {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Pass
        {
            // No culling or depth
            Cull Off
            Lighting Off
            ZWrite Off
      
	  		// MUST HAVE when rendering UI via render texture
			ZTest Always 
			Fog { Mode off }  
			// END OF MUST HAVE

            // Blending with the background
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]    

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityUI.cginc"
            #include "UnityCG.cginc"

            #define TRANSFORM_TEX2(tex,name) ()
            #define TRANSFORM_TEX3(tex,name) ()

            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            float4  _MainTex_ST;
            float4  _MaskTex_ST;
            sampler2D _MainTex;
            sampler2D _MaskTex;

            float4 _TilingAndOffset;
            float4 _Color;
            float4 _ClipRect;


            struct appdata
            {
                float4 vertex : POSITION;
                float4 color    : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR0;
                float2 uv : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = v.vertex;
                o.color = v.color;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv.xy * _TilingAndOffset.xy + _TilingAndOffset.zw);
                fixed4 mask = tex2D(_MaskTex, i.uv.xy * _MaskTex_ST.xy + _MaskTex_ST.zw);
                
                // applies custom mask set
                color.a = mask.a;

                // applies tint and global UI alpha
                color = color * _Color * i.color.a;

                // applies any 2D mask in parent GO
                //color.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);

                return color;
            }
            ENDCG
        }
    }
}
