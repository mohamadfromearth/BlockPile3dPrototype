Shader "Custom/UnlitRectangularRippleWithFoam"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RippleSpeed ("Ripple Speed", Float) = 1.0
        _RippleIntensity ("Ripple Intensity", Float) = 0.05
        _RippleFrequency ("Ripple Frequency", Float) = 10.0

        // Foam Properties
        _FoamColor ("Foam Color", Color) = (1,1,1,0.5)
        _FoamIntensity ("Foam Intensity", Float) = 1.0
        _FoamThreshold ("Foam Threshold", Float) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Properties
            sampler2D _MainTex;
            float _RippleSpeed;
            float _RippleIntensity;
            float _RippleFrequency;

            // Foam Properties
            fixed4 _FoamColor;
            float _FoamIntensity;
            float _FoamThreshold;

            // Uniforms
            float4 _MainTex_ST;

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

            // Vertex Shader
            v2f vert (appdata v)
            {
                v2f o;
                // Pass through the vertex position
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Pass through the UV coordinates
                o.uv = v.uv;
                return o;
            }

            // Fragment Shader
            fixed4 frag (v2f i) : SV_Target
            {
                // Time variable
                float time = _Time.y * _RippleSpeed;

                // Center the UV coordinates
                float2 centeredUV = i.uv - 0.5;

                // Calculate distance from center for both X and Y
                float distX = abs(centeredUV.x) * _RippleFrequency;
                float distY = abs(centeredUV.y) * _RippleFrequency;

                // Generate ripple effect using sine functions
                float rippleX = sin(distX - time) * _RippleIntensity;
                float rippleY = sin(distY - time) * _RippleIntensity;

                // Combine the ripple effects
                float2 ripple = float2(rippleX, rippleY);

                // Apply the ripple to the UV coordinates
                float2 displacedUV = i.uv + ripple;

                // Sample the texture with the displaced UV
                fixed4 col = tex2D(_MainTex, displacedUV);

                // Calculate combined ripple amplitude for foam
                float rippleAmplitude = abs(rippleX) + abs(rippleY);

                // Determine foam visibility based on threshold
                float foamFactor = smoothstep(_FoamThreshold, _FoamThreshold + 0.1, rippleAmplitude);

                // Apply foam color
                fixed4 foam = _FoamColor * _FoamIntensity * foamFactor;

                // Combine the texture color with foam
                col = lerp(col, foam + col, foamFactor);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
}
