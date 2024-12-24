Shader "Custom/UnlitProceduralRippleWithGradient"
{
    Properties
    {
        // Ripple Properties
        _RippleSpeed ("Ripple Speed", Float) = 1.0
        _RippleIntensity ("Ripple Intensity", Float) = 0.05
        _RippleFrequency ("Ripple Frequency", Float) = 10.0

        // Ripple Shape: 0 = Circle, 1 = Square
        _RippleShape ("Ripple Shape", Float) = 0.0

        // Gradient Colors
        _GradientStart ("Gradient Start Color", Color) = (1,1,1,1)
        _GradientEnd ("Gradient End Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Lighting Off
        Cull Off
        Fog { Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Properties
            float _RippleSpeed;
            float _RippleIntensity;
            float _RippleFrequency;
            float _RippleShape;

            fixed4 _GradientStart;
            fixed4 _GradientEnd;

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

            // Vertex Shader: Pass through
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Fragment Shader: Procedural ripples with gradient
            fixed4 frag (v2f i) : SV_Target
            {
                // Time variable
                float time = _Time.y * _RippleSpeed;

                // Center the UV coordinates
                float2 centeredUV = i.uv - 0.5;

                // Calculate distance based on shape
                float distance;

                if (_RippleShape < 0.5)
                {
                    // Circular
                    distance = length(centeredUV) * _RippleFrequency;
                }
                else
                {
                    // Square
                    distance = max(abs(centeredUV.x), abs(centeredUV.y)) * _RippleFrequency;
                }

                // Ripple pattern
                float ripple = sin(distance - time);

                // Edge detection
                float edge = smoothstep(0.0, _RippleIntensity, abs(ripple));

                // Alpha based on edge
                float alpha = edge;

                // Gradient based on distance
                float gradientFactor = distance - floor(distance);
                gradientFactor = clamp(gradientFactor * 2.0, 0.0, 1.0); // Adjust for smoother gradient

                // Interpolate between start and end colors
                fixed4 rippleColor = lerp(_GradientStart, _GradientEnd, gradientFactor);

                // Apply alpha
                rippleColor.a = alpha * _GradientStart.a;

                return rippleColor;
            }
            ENDCG
        }
    }
    FallBack "Transparent/VertexLit"
}
