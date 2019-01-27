Shader "Rim" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _Color ("Color", Color) = (1, 1, 1, 1)
      _BumpMap ("Bumpmap", 2D) = "bump" {}
      _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
      _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
	  _Transparency("Transparency", Range(0.0,1)) = 0.25
    }
    SubShader {
		
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }


      CGPROGRAM
      #pragma surface surf Lambert alpha
      struct Input {
          float2 uv_MainTex;
          float2 uv_BumpMap;
          float3 viewDir;
      };

      sampler2D _MainTex;
      sampler2D _BumpMap;
      float4 _RimColor;
      float _RimPower;
      fixed4 _Color;
	  float _Transparency;


      void surf (Input IN, inout SurfaceOutput o) {

          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color.rgb;
          o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
          half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          o.Emission = _RimColor.rgb * pow (rim, _RimPower);
		  o.Alpha = _Transparency;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }