Shader "Custom/Fish"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Speed ("Speed", int) = 1
        _Amplitude ("Amplitude", float) = 50

        _ThresholdLow ("Threshold Low", float) = 0
        _ThresholdHigh ("Threshold High", float) = 0.1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        int _Speed;
        half _Amplitude;
        half _ThresholdHigh;
        half _ThresholdLow;

        void vert (inout appdata_full v) {
            
            half value = sin((v.vertex.x*_Amplitude) + (_Time.y * _Speed))*0.01;

            half valueLerp = max(_ThresholdLow, min(v.vertex.x, _ThresholdHigh));
            valueLerp = (valueLerp - _ThresholdLow) / (_ThresholdHigh - _ThresholdLow);
            v.vertex.z += lerp(0, value, 1-valueLerp);
        }

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
