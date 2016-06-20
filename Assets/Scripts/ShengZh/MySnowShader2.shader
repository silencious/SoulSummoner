Shader "ShengZh/MySnowShader2" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
		_MainTex ("Base (RGB) RefStrGloss (A)", 2D) = "white" {}
		_Cube ("Reflection Cubemap", Cube) = "" { TexGen CubeReflect }
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Snow ("Snow Level", Range(0,1)) = 0
        _SnowColor ("Snow Color", Color) = (1.0,1.0,1.0,1.0)
        _SnowDirection ("Snow Direction", Vector) = (0,1,0)
        _SnowDepth ("Snow Depth", Range(0,0.3)) = 0.1
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 400
		CGPROGRAM
		#pragma surface surf BlinnPhong vertex:vert
		#pragma target 3.0
		#pragma exclude_renderers d3d11_9x
		sampler2D _MainTex;
		sampler2D _BumpMap;
		samplerCUBE _Cube;

		fixed4 _Color;
		fixed4 _ReflectColor;
		half _Shininess;
		
		float _Snow;
        float4 _SnowColor;
        float4 _SnowDirection;
        float _SnowDepth;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldRefl;
			float3 worldNormal;
			INTERNAL_DATA
		};

		void surf (Input IN, inout SurfaceOutput o) {
		    //This is a shader modified by szh
			//Merge a snow shader from internet and the built-in Reflect BumpSpecular Shader
			
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 c = tex * _Color;
			o.Albedo = c.rgb;
	
			o.Gloss = tex.a;
			o.Specular = _Shininess;
	
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	
			float3 worldRefl = WorldReflectionVector (IN, o.Normal);
			fixed4 reflcol = texCUBE (_Cube, worldRefl);
			reflcol *= tex.a;
			o.Emission = reflcol.rgb * _ReflectColor.rgb;

				//得到世界坐标系下的真正法向量（而非凹凸贴图产生的法向量）和雪落
					//下相反方向的点乘结果，即两者余弦值，并和_Snow（积雪程度）比较
					if(dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz)>lerp(1,-1,_Snow))
						//此处我们可以看出_Snow参数只是一个插值项，当上述夹角余弦值大于
						//lerp(1,-1,_Snow)=1-2*_Snow时，即表示此处积雪覆盖，所以此值越大，
						//积雪程度程度越大。此时给覆盖积雪的区域填充雪的颜色
						   o.Albedo = _SnowColor.rgb;
					else
						//否则使用物体原先颜色，表示未覆盖积雪 
						o.Albedo = c.rgb;

			o.Alpha = reflcol.a * _ReflectColor.a;
		}

		void vert (inout appdata_full v) {
					  //将_SnowDirection转化到模型的局部坐标系下
					 float4 sn = mul(UNITY_MATRIX_IT_MV, _SnowDirection);
					 if(dot(v.normal, sn.xyz) >= lerp(1,-1, (_Snow*2)/3))
					 {
						v.vertex.xyz += (sn.xyz + v.normal) * _SnowDepth * _Snow;
					}
		}
		ENDCG
	}

	FallBack "Reflective/Bumped Diffuse"
}
