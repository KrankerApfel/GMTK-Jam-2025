Shader "Custom/Galaxy_Blue_Cartoon_SoftContrast"
{
    Properties
    {
        _MainTex("Audio Texture (optional)", 2D) = "white" {}
        _Resolution("Resolution", Vector) = (512,512,0,0)
        _Darkness("Darkness Factor", Range(0, 1)) = 0.6
        _ToonSteps("Toon Levels", Range(2, 8)) = 4
        _OutlineStrength("Edge Highlight", Range(0, 5)) = 1.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Resolution;
            float _Darkness;
            float _ToonSteps;
            float _OutlineStrength;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float field(float3 p, float s, float iTime)
            {
                float strength = 7.0 + 0.03 * log(1e-6 + frac(sin(iTime) * 4373.11));
                float accum = s / 4.0;
                float prev = 0.0;
                float tw = 0.0;
                for (int i = 0; i < 26; ++i)
                {
                    float mag = dot(p, p);
                    mag = max(mag, 1e-5);
                    p = abs(p) / mag + float3(-0.5, -0.4, -1.5);
                    float w = exp(-float(i) / 7.0);
                    accum += w * exp(-strength * pow(abs(mag - prev), 2.2));
                    tw += w;
                    prev = mag;
                }
                return max(0.0, 5.0 * accum / tw - 0.7);
            }

            float field2(float3 p, float s, float iTime)
            {
                float strength = 7.0 + 0.03 * log(1e-6 + frac(sin(iTime) * 4373.11));
                float accum = s / 4.0;
                float prev = 0.0;
                float tw = 0.0;
                for (int i = 0; i < 18; ++i)
                {
                    float mag = dot(p, p);
                    mag = max(mag, 1e-5);
                    p = abs(p) / mag + float3(-0.5, -0.4, -1.5);
                    float w = exp(-float(i) / 7.0);
                    accum += w * exp(-strength * pow(abs(mag - prev), 2.2));
                    tw += w;
                    prev = mag;
                }
                return max(0.0, 5.0 * accum / tw - 0.7);
            }

            float3 nrand3(float2 co)
            {
                float3 a = frac(cos(co.x * 8.3e-3 + co.y) * float3(1.3e5, 4.7e5, 2.9e5));
                float3 b = frac(sin(co.x * 0.3e-3 + co.y) * float3(8.1e5, 1.0e5, 0.1e5));
                return lerp(a, b, 0.5);
            }

            float3 Toonify(float3 color, float steps)
            {
                float3 quantized = floor(color * steps) / steps;
                return lerp(color, quantized, 0.5); // 50% aplats, 50% lissage
            }

            float EdgeHighlight(float brightness, float2 uv)
            {
                float remapped = saturate(brightness * 0.6 + 0.4); 
                float edge = lerp(0.7, 1.0, remapped);
                return edge;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 fragCoord = i.uv * _Resolution.xy;
                float2 uv = 2.0 * fragCoord / _Resolution.xy - 1.0;
                float2 uvs = uv * _Resolution.xy / max(_Resolution.x, _Resolution.y);

                float freqs[4];
                freqs[0] = 0.3;
                freqs[1] = 0.4;
                freqs[2] = 0.5;
                freqs[3] = 0.6;

                float3 p = float3(uvs / 4.0, 0) + float3(1.0, -1.3, 0.0);
                p += 0.2 * float3(sin(_Time.y / 16.0), sin(_Time.y / 12.0), sin(_Time.y / 128.0));
                float t = field(p, freqs[2], _Time.y);

                float v = (1.0 - exp((abs(uv.x) - 1.0) * 6.0)) * (1.0 - exp((abs(uv.y) - 1.0) * 6.0));

                float3 p2 = float3(uvs / (4.0 + sin(_Time.y * 0.11) * 0.2 + 0.2 + sin(_Time.y * 0.15) * 0.3 + 0.4), 1.5) + float3(2.0, -1.3, -1.0);
                p2 += 0.25 * float3(sin(_Time.y / 16.0), sin(_Time.y / 12.0), sin(_Time.y / 128.0));
                float t2 = field2(p2, freqs[3], _Time.y);

                float3 nebulaColor = float3(0.4 * t2 * t2 * t2, 0.3 * t2 * t2, 0.7 * t2 * freqs[0]);
                nebulaColor = Toonify(nebulaColor, _ToonSteps);
                float4 c2 = lerp(0.4, 1.0, v) * float4(nebulaColor, 0.5 * t2);

                float2 seed = floor(p.xy * _Resolution.x);
                float3 rnd = nrand3(seed);
                float valStar = pow(rnd.y, 40.0);
                float4 starcolor = float4(valStar, valStar, valStar + 0.1, valStar) * 0.3;

                float2 seed2 = floor(p2.xy * _Resolution.x);
                float3 rnd2 = nrand3(seed2);
                valStar = pow(rnd2.y, 40.0);
                starcolor += float4(valStar, valStar, valStar + 0.1, valStar) * 0.3;

                float3 galaxyColor = float3(0.3 * freqs[2] * t * t * t, 0.2 * freqs[1] * t * t, 0.6 * freqs[3] * t);
                galaxyColor = Toonify(galaxyColor, _ToonSteps);
                float4 col = lerp(freqs[3] - 0.3, 1.0, v) * float4(galaxyColor, 1.0);

                float3 finalColor = col.rgb + c2.rgb + starcolor.rgb;

                float brightness = dot(finalColor, float3(0.333, 0.333, 0.333));
                float edgeDarken = EdgeHighlight(brightness, i.uv);
                finalColor *= edgeDarken;

                return float4(finalColor, 1.0) * _Darkness;
            }

            ENDCG
        }
    }
}
