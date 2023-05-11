// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sketchy Outline Shader"
{
	Properties
	{
		_Color0("Color 0", Color) = (0,0,0,0)
		_NoiseScale("Noise Scale", Float) = 10
		_OutlineMinimum("Outline Minimum", Range( 0 , 1)) = 0
		_OutlineWidth("Outline Width", Float) = 0.05
		_OutlineMaximum("Outline Maximum", Range( 0 , 1)) = 0.75
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Blend("Blend", Range( 0 , 1)) = 0
		_Blend12("Blend12", Float) = 0.15
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		
		struct Input
		{
			half filler;
		};
		uniform float4 _Color0;
		uniform float _NoiseScale;
		uniform float _OutlineMinimum;
		uniform float _OutlineMaximum;
		uniform float _OutlineWidth;
		
		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float simplePerlin2D24 = snoise( ase_vertexNormal.xy*_NoiseScale );
			simplePerlin2D24 = simplePerlin2D24*0.5 + 0.5;
			float clampResult29 = clamp( simplePerlin2D24 , _OutlineMinimum , _OutlineMaximum );
			float outlineVar = ( clampResult29 * _OutlineWidth );
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _Color0.rgb;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Blend12;
		uniform float _Blend;


		float2 voronoihash34( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi34( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash34( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			
			 		}
			 	}
			}
			return F1;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode32 = tex2D( _TextureSample0, uv_TextureSample0 );
			float4 color57 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float time34 = 0.0;
			float2 voronoiSmoothId34 = 0;
			float2 coords34 = i.uv_texcoord * 6.74;
			float2 id34 = 0;
			float2 uv34 = 0;
			float voroi34 = voronoi34( coords34, time34, id34, uv34, 0, voronoiSmoothId34 );
			float4 temp_cast_0 = (step( voroi34 , _Blend12 )).xxxx;
			float4 temp_output_1_0_g2 = temp_cast_0;
			float4 color56 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float4 temp_output_2_0_g2 = color56;
			float temp_output_11_0_g2 = distance( temp_output_1_0_g2 , temp_output_2_0_g2 );
			float4 lerpResult21_g2 = lerp( color57 , temp_output_1_0_g2 , saturate( ( ( temp_output_11_0_g2 - 0.0 ) / max( 0.0 , 1E-05 ) ) ));
			float4 color50 = IsGammaSpace() ? float4(0.6226415,0,0,0) : float4(0.3456162,0,0,0);
			float4 lerpResult44 = lerp( tex2DNode32 , ( tex2DNode32 + ( lerpResult21_g2 * color50 ) ) , _Blend);
			o.Albedo = lerpResult44.rgb;
			float temp_output_39_0 = _Blend;
			o.Smoothness = temp_output_39_0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
2816;398;1554;900;934.908;287.049;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;35;-1867.186,-396.9466;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;6.74;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;34;-1536.526,-406.1126;Inherit;True;0;0;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;49;-1598.748,-5.028316;Inherit;False;Property;_Blend12;Blend12;7;0;Create;True;0;0;0;False;0;False;0.15;0.87;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;47;-1922.252,377.0863;Inherit;False;1809.343;924.2194;Outline;10;26;25;24;31;29;28;7;30;27;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;57;-1149.08,11.0354;Inherit;False;Constant;_Color4;Color 4;7;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;56;-1379.303,-28.9852;Inherit;False;Constant;_Color3;Color 3;7;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;48;-1321.561,-307.6734;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;26;-1839.913,714.5193;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-1872.252,1002.379;Inherit;False;Property;_NoiseScale;Noise Scale;1;0;Create;True;0;0;0;False;0;False;10;5.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1469.065,1042.306;Inherit;False;Property;_OutlineMaximum;Outline Maximum;4;0;Create;True;0;0;0;False;0;False;0.75;0.052;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1627.276,427.0863;Inherit;False;Property;_OutlineMinimum;Outline Minimum;2;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;55;-977.2539,-263.7542;Inherit;True;Replace Color;-1;;2;896dccb3016c847439def376a728b869;1,12,0;5;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;24;-1673.52,853.1702;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;50;-807.825,171.144;Inherit;False;Constant;_Color1;Color 1;7;0;Create;True;0;0;0;False;0;False;0.6226415,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-651.5021,-123.7312;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1151.19,1040.724;Inherit;False;Property;_OutlineWidth;Outline Width;3;0;Create;True;0;0;0;False;0;False;0.05;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;29;-1156.165,776.1281;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;32;-808.8307,-690.6114;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;None;9830136ddd4f8c84cbd28494342bff10;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-415.6667,-328.2172;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-786.7089,714.3203;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;-1248.911,562.0775;Inherit;False;Property;_Color0;Color 0;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-454.2686,94.79311;Inherit;False;Property;_Blend;Blend;6;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OutlineNode;5;-338.9092,784.7524;Inherit;False;0;True;None;0;0;Front;True;True;True;True;0;False;-1;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;44;-58.27043,-266.3926;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;172.257,-21.54509;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Sketchy Outline Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.12;True;True;0;False;Opaque;;Geometry;ForwardOnly;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.02;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;34;2;35;0
WireConnection;48;0;34;0
WireConnection;48;1;49;0
WireConnection;55;1;48;0
WireConnection;55;2;56;0
WireConnection;55;3;57;0
WireConnection;24;0;26;0
WireConnection;24;1;25;0
WireConnection;58;0;55;0
WireConnection;58;1;50;0
WireConnection;29;0;24;0
WireConnection;29;1;30;0
WireConnection;29;2;31;0
WireConnection;42;0;32;0
WireConnection;42;1;58;0
WireConnection;27;0;29;0
WireConnection;27;1;28;0
WireConnection;5;0;7;0
WireConnection;5;1;27;0
WireConnection;44;0;32;0
WireConnection;44;1;42;0
WireConnection;44;2;39;0
WireConnection;0;0;44;0
WireConnection;0;4;39;0
WireConnection;0;11;5;0
ASEEND*/
//CHKSM=6A11084DCCC0E7501E2BFF8069C63DACD78DB720