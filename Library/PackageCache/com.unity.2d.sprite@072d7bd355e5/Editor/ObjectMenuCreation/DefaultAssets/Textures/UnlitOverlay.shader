Shader "Custom/UnlitOverlay"
{
    Properties
    {
        _Color("Base Tint", Color) = (1,1,1,1)
    }
        SubShader
    {
        Tags { "Queue" = "Overlay" }
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // this uniform can still tint everything if you like
            fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;    // ← grab the GL.Color() value
            };
            struct v2f
            {
                float4 pos   : SV_POSITION;
                fixed4 color : COLOR;    // ← pass it to the fragment
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color * _Color;  // multiply by _Color if you want a global tint
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;  // use the interpolated line color
            }
            ENDCG
        }
    }
}
