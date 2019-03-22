Shader "Custom/Tree"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex: vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 viewDir;
			float3 viewDir2;
			float3 worldPos;
			float3 worldNormal;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(float, _Attenuation)
        UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.viewDir2 = normalize(WorldSpaceViewDir(float4(v.vertex.xyz, 0)));
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float atten = UNITY_ACCESS_INSTANCED_PROP(Props, _Attenuation);
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = lerp(float3(0,0,0), c.rgb, atten);

			

			// look for places where the normal is offset is pointing slightly away from the view DIRECTIONAL
			// add some variation with SinTime from 0.0-0.15


			float d = dot(normalize(float3(IN.viewDir.x*2, IN.viewDir.y, -1)), IN.worldNormal);
			// float diff = saturate(pow(abs(d - 0.7), 2));

			// float d = max(max(dot(float3(0, 0, 1), IN.worldNormal), dot(float3(0, -1, 0), IN.worldNormal)), dot(float3(1, 0, 0), IN.worldNormal));
			float diff = saturate(pow(d,2));
			o.Emission = lerp(float3(0,0,0) , _Color.rgb, (1 - atten) * diff) * _Color.a;

			//o.Emission = float4(IN.viewDir.x, 0, -1, 1);// float4(d2, d2, d2, 0);

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
