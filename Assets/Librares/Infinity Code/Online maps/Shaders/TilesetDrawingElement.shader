Shader "Infinity Code/Online Maps/Tileset DrawingElement" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
	Tags {"Queue"="Transparent-50" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200

CGPROGRAM
#pragma surface surf Lambert alpha

sampler2D _MainTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
	float2 uv_TrafficTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	fixed3 ct = c.rgb;
	ct = ct * _Color;
	o.Albedo = ct;
	o.Alpha = c.a * _Color.a;
}
ENDCG
}

Fallback "Transparent/VertexLit"
}
