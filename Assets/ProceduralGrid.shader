Shader "Unlit/ShapezGrid_FinalFixed"
{
    Properties
    {
        _GridColor("Grid Color", Color) = (0.89, 0.91, 0.92, 1) // Light gray
        _BackgroundColor("Background Color", Color) = (0.93, 0.93, 0.95, 1) // Off-white
        _GridSpacing("Grid Spacing", Float) = 1.0
        _LineWidth("Line Width", Float) = 0.01
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

            float _GridSpacing;
            float _LineWidth;
            fixed4 _GridColor;
            fixed4 _BackgroundColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 gridPos = i.worldPos / _GridSpacing;
                float2 fraction = frac(gridPos);
                float2 grid = min(fraction, 1.0 - fraction) * _GridSpacing;
                
                // Draw grid lines
                float lineX = step(grid.x, _LineWidth);
                float lineY = step(grid.y, _LineWidth);
                float gridLine = max(lineX, lineY);

                // CORRECTED LERP ORDER (now shows grid lines on background)
                return lerp(_BackgroundColor, _GridColor, gridLine);
            }
            ENDCG
        }
    }
}///merge