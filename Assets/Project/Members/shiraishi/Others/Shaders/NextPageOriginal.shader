Shader "Custom/NextPageOriginal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _FoldOrigin ("Fold Origin", Vector) = (0,0,0,0)
        _FlipProgress ("Flip Progress", Range(0,1)) = 0.0
        _MarkedColor ("Marked Color", Color) = (1,0.5,0.5,1)
        [Toggle]_ShowBlueLine ("Show Blue Line", Float) = 1
        [Toggle]_ShowGreenLine ("Show Green Line", Float) = 1
        [Toggle]_ShowMarkedArea ("Show Marked Area", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _MarkedColor;
            float4 _FoldOrigin;
            float _FlipProgress;
            float _ShowBlueLine;
            float _ShowGreenLine;
            float _ShowMarkedArea;

            void GetLineBoxIntersection(float2 origin, float2 dir, out float2 pt1, out float2 pt2)
            {
                float2 boundsMin = float2(0, 0);
                float2 boundsMax = float2(1, 1);

                float2 p1 = origin, p2 = origin;
                int found = 0;

                float2 normals[4] = { float2(1, 0), float2(-1, 0), float2(0, 1), float2(0, -1) };
                float2 positions[4] = { boundsMin, boundsMax, boundsMin, boundsMax };

                for (int i = 0; i < 4; i++)
                {
                    float n = dot(dir, normals[i]);
                    if (abs(n) < 1e-6) continue;
                    float d = dot(positions[i] - origin, normals[i]) / n;
                    float2 ipt = origin + dir * d;
                    if (ipt.x >= 0.0 && ipt.x <= 1.0 && ipt.y >= 0.0 && ipt.y <= 1.0)
                    {
                        if (found == 0) p1 = ipt;
                        else p2 = ipt;
                        found++;
                    }
                }
                pt1 = p1;
                pt2 = p2;
            }

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 p = i.uv;

                // 青線計算
                float2 origin = _FoldOrigin.xy;
                float2 center = float2(0.5, 0.5);
                float2 dir = center - origin;
                dir = normalize(dir);

                float2 ptA, ptB;
                GetLineBoxIntersection(origin, dir, ptA, ptB);

                float2 seg = ptB - ptA;
                float segLen = length(seg);

                float2 v = p - ptA;
                float t = clamp(dot(v, seg) / (segLen * segLen), 0.0, 1.0);
                float2 nearest = ptA + seg * t;
                float distToBlue = length(p - nearest);

                // 緑の折り返し線
                float2 progressPos = origin + dir * (length(center - origin) * _FlipProgress);
                float2 normal = float2(-dir.y, dir.x);
                float distToGreen = abs(dot(p - progressPos, normal));
                float greenLineWidth = 0.008;

                fixed4 baseCol = tex2D(_MainTex, p) * _Color;

                // 緑の折り返し線
                if (_ShowGreenLine > 0.5 && distToGreen < greenLineWidth)
                {
                    baseCol.rgb = float3(0.1, 1.0, 0.2); // 緑
                }
                // 青線
                else if (_ShowBlueLine > 0.5 && distToBlue < 0.008)
                {
                    baseCol.rgb = float3(0.1, 0.5, 1.0); // 青
                }
                // マーカー色変化エリア
                else if (_ShowMarkedArea > 0.5 && dot(p - progressPos, normal) <= 0)
                {
                    baseCol.rgb = lerp(baseCol.rgb, _MarkedColor.rgb, 0.7);
                }
                // それ以外は元色

                return baseCol;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
}
