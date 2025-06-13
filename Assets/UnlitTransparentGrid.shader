Shader "Unlit/ShapezGrid_NoGridLines"
{
    Properties
    {
        _MainTex("Background Texture", 2D) = "white" {}
        _TextureTiling("Texture Tiling", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 worldPos : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _TextureTiling;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.worldPos * _TextureTiling;
                fixed4 texColor = tex2D(_MainTex, uv);
                return texColor;
            }
            ENDCG
        }
    }
}