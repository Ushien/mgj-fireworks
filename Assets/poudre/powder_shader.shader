Shader "Custom/powder_shader"
{
    Properties
    {
        _PointSize("Point Size", Float) = 5.0
        _color1("color1", Color) = (1, 0, 0, 1) // Red by default
        _color2("color2", Color) = (0, 1, 0, 1) // vert par défaut
        _color3("color3", Color) = (0, 0, 1, 1) // bleu par défaut
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }


        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            struct Particle {
                float3 position;
                float3 truePosition;
                float3 velocity;
                float life;
                int2 type;
            };

            float3 HSVtoRGB(float h, float s, float v)
            {
                float4 K = float4(1.0, 2.0/3.0, 1.0/3.0, 3.0);
                float3 p = abs(frac(h + K.xyz) * 6.0 - K.www);
                return v * lerp(K.xxx, saturate(p - K.xxx), s);
            }

            StructuredBuffer<Particle> particleBuffer;

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float _PointSize;
            float4 _color1; // Declare new color parameter
            float4 _color2;
            float4 _color3;
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

                // Modification de la couleur 4 (arc-en-ciel)
                // ------------------------------------------
                float hue = frac(_Time.y * 1); // Adjust speed here
                float3 rgb = HSVtoRGB(hue, 1.0, 1.0);
                float4 color4 = float4(rgb, 1.0);

                // on change la particule en fonction du type sélectionné
                // ------------------------------------------------------
                int type = particle.type.x;
                if (type == 0)
                    OUT.color = _color1;
                else if (type == 1)
                    OUT.color = _color2;
                else if (type == 2)
                    OUT.color = _color3;
                else if (type == 3)
                    OUT.color = color4;
                else
                    OUT.color = float4(1, 1, 1, 1); // default white


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
