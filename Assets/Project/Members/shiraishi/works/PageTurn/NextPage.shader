// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MBL/NextPage"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_PageTex("PageTexture", 2D) = "white" {}
		_AlphaMask("AlphaMask", Range(0, 1)) = 0.1
		_Flip("Flip",Range(-1, 1)) = 0
		_PageAngle("Page Angle", Range(0, 6.283)) = 0 // ラジアン単位（0～2pi）
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 puv : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _PageTex;
			float4 _PageTex_ST;
			float _AlphaMask;
			float _Flip;
			float _PageAngle;

			float l2(float y)
			{
				return 1 - _Flip + 0.1 * cos(y * 2);
			}

			float l1(float x)
			{
				return _Flip + 0.1 * sin(x * 3);
			}

			float l0(float x)
			{
				return x - _Flip;
			}

			float2 Rotate(float2 uv, float angle)
			{
				float s = sin(angle);
				float c = cos(angle);
				float2 center = float2(0.5, 0.5);
				uv -= center;
				float2 rotated = float2(
				    uv.x * c - uv.y * s,
				    uv.x * s + uv.y * c
				);
				return rotated + center;
			}


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.puv = TRANSFORM_TEX(v.uv, _PageTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
			    float2 uv_rot = Rotate(i.uv, _PageAngle);
			
			    float4 content_col = tex2D(_MainTex, i.uv);
			    float4 page_col = tex2D(_PageTex, i.puv);
			
			    float l0_y = l0(uv_rot.x);
			    clip(uv_rot.y - l0_y);
			
			    if (uv_rot.x > l1(uv_rot.y) && uv_rot.y < l2(uv_rot.x))
			        content_col = float4(0.5, 0.5, 0.5, 1);
			
			    if (content_col.a < _AlphaMask)
			        return page_col;
			
			    return content_col * page_col;
			}
			ENDCG
		}
	}
}