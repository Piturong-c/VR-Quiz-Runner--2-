Shader "Custom/Land"
{
    Properties
    {
        _StartPos("StartPos",float) = (0,0,0)
        _State("Environment State", int) = 0
        _JungleGrass("Jungle Grass", Color) = (1,1,1,1)
        _JungleDirt("Jungle Dirt", Color) = (1,1,1,1)
        _JungleRock("Jungle Rock",Color) = (0.3,0.3,0.3)
        _DesertGlass("Desert Glass", Color) = (1,1,1,1)
        _DesertDirt("Desert Dirt", Color) = (1,1,1,1)
        _DesertRock("Desert Rock",Color) = (0.3,0.3,0.3)
        
        _Threshold("Threshold", Range(0,1)) = 0.98
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _LavaThreshold("LavaThreshold",float) = -0.5
        _LavaAngle("LavaAngle",float) = -0.5
        _LavaColor("LavaColor",Color) = (0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 worldPos;
        };
        
        float3 _StartPos;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        int _State;
        fixed4 _JungleGrass;
        fixed4 _JungleDirt;
        fixed4 _JungleRock;
        fixed4 _DesertGrass;
        fixed4 _DesertDirt;
        fixed4 _DesertRock;
        half _Threshold;
        
        
        float _LavaThreshold;
        float _LavaAngle;
        float3 _LavaColor;
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            float4 grass = _JungleGrass;
            float4 dirt = _JungleDirt;
            
            if(_State == 1)
            {
                grass = _DesertGrass;
                dirt = _DesertDirt;
            }
            
            if(dot(IN.worldNormal,float3(0,1,0)) >= _Threshold)
            {
                o.Albedo = grass;
                o.Albedo *= clamp(IN.worldPos.y - _StartPos.y,0,1);
            }
            else
            {
                if(IN.worldPos.y >= _LavaThreshold)
                {
                    o.Albedo = dirt;
                    o.Albedo *= clamp(IN.worldPos.y - _StartPos.y,0,1);
                }
                else
                {
                    if(dot(IN.worldNormal,float3(0,_LavaAngle,0)) >= 0.85)
                    {
                        o.Albedo = _LavaColor;
                    }else{
                        o.Albedo = _LavaColor* dot(IN.worldNormal,float3(0,_LavaAngle,0));
                    }
                }
            }
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
