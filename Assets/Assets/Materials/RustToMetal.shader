Shader "Custom/TweezerBlend"
{
    Properties
    {
        _RustTex ("Rust Texture", 2D) = "white" {}
        _MetalTex ("Metal Texture", 2D) = "white" {}
        _BlendValue ("Blend Value", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _RustTex;
        sampler2D _MetalTex;
        float _BlendValue;

        struct Input
        {
            float2 uv_RustTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 rustColor = tex2D(_RustTex, IN.uv_RustTex);
            float4 metalColor = tex2D(_MetalTex, IN.uv_RustTex);

            // Blend between the two materials based on _BlendValue
            o.Albedo = lerp(rustColor.rgb, metalColor.rgb, _BlendValue);
            o.Metallic = lerp(0.3, 1.0, _BlendValue); // Adjust for metallic look
            o.Smoothness = lerp(0.1, 0.8, _BlendValue); // Adjust glossiness
        }
        ENDCG
    }
}
