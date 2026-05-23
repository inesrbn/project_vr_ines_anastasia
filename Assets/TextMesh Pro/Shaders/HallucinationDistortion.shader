Shader "Custom/HallucinationDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DistortionIntensity ("Distortion Intensity", Range(0, 1)) = 0.0
        _DistortionSpeed ("Distortion Speed", Range(0, 10)) = 1.0
        _NoiseScale ("Noise Scale", Range(0.1, 50)) = 10.0
        _ColorTint ("Color Tint", Color) = (1, 1, 1, 1)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Cull Off
        ZWrite On
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _DistortionIntensity;
            float _DistortionSpeed;
            float _NoiseScale;
            float4 _ColorTint;
            
            // Fonction Perlin Noise simplifiée
            float2 hash22(float2 p)
            {
                float3 p3 = frac(float3(p.xyx) * float3(.1031, .1030, .0973));
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.xx + p3.yz) * p3.zy);
            }
            
            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);
                
                float2 a = hash22(i + float2(0.0, 0.0));
                float2 b = hash22(i + float2(1.0, 0.0));
                float2 c = hash22(i + float2(0.0, 1.0));
                float2 d = hash22(i + float2(1.0, 1.0));
                
                return lerp(lerp(a.x, b.x, f.x), lerp(c.x, d.x, f.x), f.y);
            }
            
            float fbm(float2 p)
            {
                float value = 0.0;
                float amplitude = 0.5;
                for (int i = 0; i < 4; i++)
                {
                    value += amplitude * noise(p);
                    p *= 2.0;
                    amplitude *= 0.5;
                }
                return value;
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Calculer le bruit de Perlin animé
                float2 noiseCoord = i.uv * _NoiseScale + _Time.y * _DistortionSpeed * 0.1;
                float noiseValue = fbm(noiseCoord);
                
                // Créer la distorsion UV basée sur le bruit
                float2 distortion = (noiseValue - 0.5) * 2.0 * _DistortionIntensity * 0.1;
                float2 distortedUV = i.uv + distortion;
                
                // Échantillonner la texture avec distorsion
                fixed4 col = tex2D(_MainTex, distortedUV);
                
                // Ajouter une légère teinte basée sur le bruit pour l'effet hallucination
                float colorShift = (noiseValue - 0.5) * _DistortionIntensity * 0.2;
                col.rgb += colorShift * float3(0.1, -0.05, 0.1); // Léger décalage vers le bleu/violet
                
                // Appliquer la teinte de couleur
                col.rgb *= _ColorTint.rgb;
                
                return col;
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}


