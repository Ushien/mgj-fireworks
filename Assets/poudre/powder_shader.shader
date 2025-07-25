Shader "Custom/powder_shader"
{
    Properties
    {
        _PointSize("Point Size", Float) = 5.0
        _ColorTint("Color Tint", Color) = (1, 0, 0, 1) // Red by default
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            struct Particle {
                float3 position;
                float3 truePosition;
                float3 velocity;
                float life;
            };

            StructuredBuffer<Particle> particleBuffer;

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float _PointSize;
            float4 _ColorTint; // Declare new color parameter
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                uint instanceID : SV_InstanceID;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 color : COLOR;
                float size : PSIZE;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                Particle particle = particleBuffer[IN.instanceID];

                // Use the general color parameter instead of life
                OUT.color = _ColorTint;

                OUT.positionHCS = TransformObjectToHClip(particle.position);
                OUT.size = _PointSize;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return IN.color;
            }

            ENDHLSL
        }
    }
}
