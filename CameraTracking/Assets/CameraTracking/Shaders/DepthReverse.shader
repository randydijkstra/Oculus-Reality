Shader "DepthReverse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "black" {}
	}
	
	SubShader {
        Tags { "Queue" = "Geometry+10" } //render after opaque geometry
        ZTest GEqual
        Offset 1,1
		Pass {
			Lighting Off
			SeparateSpecular Off
			SetTexture [_MainTex] { combine texture }
		}
	}
	
	Fallback "VertexLit", 1
}