Shader "Infinity Code/Online Maps/Tileset" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_TrafficTex ("Traffic Texture", 2D) = "black" {}
}

SubShader {
	Tags {"Queue"="Transparent-100" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200

CGPROGRAM
#pragma surface surf Lambert alpha

sampler2D _MainTex;
sampler2D _TrafficTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
	float2 uv_TrafficTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 t = tex2D(_TrafficTex, IN.uv_TrafficTex); 
	fixed3 t2 = t.rgb;
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	fixed3 ct = lerp(c.rgb, t2, t.a);
	ct = ct * _Color;
	o.Albedo = ct;
	o.Alpha = c.a * _Color.a;
}
ENDCG
}

Fallback "Transparent/VertexLit"
}
