Shader "Custom/FadeShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _FadeTex ("Fade Albedo (RGB)", 2D) = "white" {}
        _Emission("Emission", 2D) = "white" {}
        _EmissionFade("Emission Fade", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _FadeProgress ("Fade Progress", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _FadeTex;
        sampler2D _Emission;
        sampler2D _EmissionFade;
        sampler2D _NoiseTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;
        float _FadeProgress;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 mainColor = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 fadedColor = tex2D (_FadeTex, IN.uv_MainTex);
            fixed4 noiseColor = tex2D (_NoiseTex, IN.uv_MainTex);

            float currentThreshold = noiseColor.x;
            float thresholdPassed = clamp(sign(currentThreshold - _FadeProgress), 0, 1);
            fixed4 finalColor = (mainColor * thresholdPassed + fadedColor * (1 - thresholdPassed));
            
            fixed4 emissionColor = tex2D (_Emission, IN.uv_MainTex);
            fixed4 emissionFadedColor = tex2D (_EmissionFade, IN.uv_MainTex);
            fixed4 finalEmissionColor = (emissionColor * thresholdPassed + emissionFadedColor * (1 - thresholdPassed));

            finalColor = finalColor * _Color;

            o.Albedo = finalColor.rgb;
            o.Metallic = 0;
            o.Smoothness = 0;
            o.Emission = finalEmissionColor.rgb;
            o.Alpha = finalColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
