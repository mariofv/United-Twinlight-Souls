Shader "Custom/FadeZFixed"
{
    // The _BaseMap variable is visible in the Material's Inspector, as a field
    // called Base Map.
    Properties
    { 
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _FadeTex ("Fade Albedo (RGB)", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _FadeProgress ("Fade Progress", Range(0,1)) = 1
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        LOD 200

        Pass {

            ZWrite On
            Cull Off // make double sided
            ColorMask 0 // don't draw any color

            HLSLPROGRAM
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4  frag(v2f i) : SV_Target
            {
                fixed4  col = tex2D(_MainTex, i.uv);
                clip(col.a - .97); // remove non-opaque pixels from writing to zbuffer
                return col;
            }
            ENDHLSL
        }

        Pass
        {
            // ---------- Start Pass 2 ----------
            ZWrite Off
            Cull Off // make double sided
            Blend SrcAlpha OneMinusSrcAlpha
            Tags { "LightMode" = "UniversalForward"}

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"         

            struct Attributes
            {
                float4 positionOS   : POSITION;
                // The uv variable contains the UV coordinate on the texture for the
                // given vertex.
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                // The uv variable contains the UV coordinate on the texture for the
                // given vertex.
                float2 uv           : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _FadeTex;
            sampler2D _NoiseTex;
            float _FadeProgress;
            fixed4 _Color;

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = UnityObjectToClipPos(IN.positionOS.xyz);
                // The TRANSFORM_TEX macro performs the tiling and offset
                // transformation.
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            fixed4  frag(Varyings IN) : SV_Target
            {
                fixed4 mainColor = tex2D (_MainTex, IN.uv);
                fixed4 fadedColor = tex2D (_FadeTex, IN.uv);
                fixed4 noiseColor = tex2D (_NoiseTex, IN.uv);

                float currentThreshold = noiseColor.x;
                float thresholdPassed = clamp(sign(currentThreshold - _FadeProgress), 0, 1);
                fixed4 finalColor = (mainColor * thresholdPassed + fadedColor * (1 - thresholdPassed));

                finalColor = finalColor * _Color;
                return finalColor;
            }
            ENDHLSL
        }
    }
}