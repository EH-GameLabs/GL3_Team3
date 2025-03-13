Shader "Custom/WireframeAllEdgesYellow"
{
    Properties
    {
        _LineColor ("Line Color", Color) = (1,1,0,1)  // Giallo
        _LineWidth ("Line Width", Range(0.001, 0.1)) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            // Imposta target a 3.0 (necessario per geometry shader, se ne usi uno)
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            // Struttura d'ingresso: la mesh deve avere anche le coordinate barycentriche
            struct appdata
            {
                float4 vertex : POSITION;
                float3 bary   : TEXCOORD0; // Coordinate barycentriche (da generare/assegnare)
            };

            // Struttura passata al fragment shader
            struct v2f
            {
                float4 pos   : SV_POSITION;
                float3 bary  : TEXCOORD0;
            };

            fixed4 _LineColor;
            float _LineWidth;

            // Vertex Shader: passiamo la posizione clip space e le coordinate barycentriche
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.bary = v.bary;
                return o;
            }

            // Fragment Shader: calcola la minima distanza dall'edge in base alle coordinate barycentriche
            fixed4 frag (v2f i) : SV_Target
            {
                // Trova la minima delle coordinate barycentriche, che rappresenta la distanza relativa dal bordo
                float edgeDistance = min(min(i.bary.x, i.bary.y), i.bary.z);
                // smoothstep crea una transizione morbida tra linee e area interna
                float lineMask = 1.0 - smoothstep(0.0, _LineWidth, edgeDistance);
                // Ritorna il colore wireframe (linea) dove lineMask è 1, altrimenti un colore di sfondo (qui nero)
                return lerp(float4(0,0,0,1), _LineColor, lineMask);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
