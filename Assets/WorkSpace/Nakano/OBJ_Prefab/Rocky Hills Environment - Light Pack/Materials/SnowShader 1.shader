Shader "Example/Diffuse Bump" 
    {
        Properties
        {
            _MainTex("Texture", 2D) = "white" {}
            _BumpMap("Bumpmap", 2D) = "bump" {}
            _Color("Color", color) = (1, 1, 1, 1)
            _Snow("Snow", Range(0, 2)) = 0.0
        }

        SubShader
        {
          Tags { "RenderType" = "Opaque" }

          CGPROGRAM

          #pragma surface surf Lambert

          struct Input 
          {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 worldNormal; INTERNAL_DATA
          };

          sampler2D _MainTex;
          sampler2D _BumpMap;
          fixed4 _Color;
          half _Snow;

          void surf(Input IN, inout SurfaceOutput o) 
          {
              o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

              float d = dot(WorldNormalVector(IN, o.Normal), fixed3(0, 1, 0));
              fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
              fixed4 white = fixed4(1, 1, 1, 1);
              c = lerp(c, white, d * _Snow);

              o.Albedo = c.rgb;
              o.Alpha = 1;
          }
          ENDCG
    }
        Fallback "Diffuse"
}