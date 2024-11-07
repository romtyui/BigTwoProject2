Shader "Custom/LeafShader"
{
    Properties

    {

        _MainTex ("纹理贴图", 2D) = "white" {}

        _Color("基本颜色",color) = (1,1,1,1)

        _ShadowColor("暗部颜色",color) = (1,1,1,1)
    }
    SubShader
    {
         Properties

    {

        _MainTex ("纹理贴图", 2D) = "white" {}

        _Color("基本颜色",color) = (1,1,1,1)

        _ShadowColor("暗部颜色",color) = (1,1,1,1)

    }

    SubShader

    {

        Tags { "RenderPipe"="UniversalPipeline" }

        LOD 100



        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"



        struct appdata

        {

            float4 vertex : POSITION;

            float2 uv : TEXCOORD0;

            float3 normal:NORMAL;

        };



        struct v2f

        {

            float2 uv : TEXCOORD0;

            float4 pos : SV_POSITION;

            float3 normalWS:TEXCOORD2;

            float3 posWS:TEXCOORD3;

        };

        CBUFFER_START(UnityPerMaterial)

        TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);

        half4 _Color;

        half4 _ShadowColor;

        CBUFFER_END

        ENDHLSL

       

        pass{

            Name "BaseColor"

            Tags{"LightMode"="UniversalForward" "RenderType"="TransparentCutOut" "Queue"="AlphaTest" }

            Cull Off

           

            HLSLPROGRAM

            #pragma vertex vert

            #pragma fragment frag



            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS  _MAIN_LIGHT_SHADOWS_CASCADE

            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            #pragma multi_compile _ Anti_Aliasing_ON



            v2f vert (appdata v)

            {

                v2f o;

                o.pos = TransformObjectToHClip(v.vertex);

                o.uv = v.uv;

                o.posWS=mul(unity_ObjectToWorld,v.vertex);

                o.normalWS = TransformObjectToWorldNormal(v.normal);

                return o;

            }

            half4 frag (v2f i) : SV_Target

            {

                //获取主光

                float4 shadowcoords=TransformWorldToShadowCoord(i.posWS);

                Light mainlight=GetMainLight(shadowcoords);

                half3 L=normalize(mainlight.direction);

                half3 N=normalize(i.normalWS);

                half ln=saturate(dot(L,N));

                half shadow=MainLightRealtimeShadow(shadowcoords)*ln;    //接收阴影

                half alpha=SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv).w;

                half4 col=lerp(_ShadowColor,_Color,shadow);

                clip(alpha-0.1);

                return col;

            }

            ENDHLSL

        }


pass { //投射阴影，向自身和周围物体投射

            Name "ShadowCast"

 

            Tags{ "LightMode" = "ShadowCaster" }



            Cull Off

           

            HLSLPROGRAM

            #pragma vertex vertshadow

            #pragma fragment fragshadow

 



            v2f vertshadow(appdata v)

            {

                v2f o;

                o.posWS= TransformObjectToWorld(v.vertex);

                o.uv=v.uv;

                Light mainlight=GetMainLight();

                o.normalWS= TransformObjectToWorldNormal(v.normal);

                o.pos=TransformWorldToHClip(ApplyShadowBias(o.posWS,o.normalWS,mainlight.direction));

                return o;

            }

            half4 fragshadow(v2f i) : SV_Target

            {

                float alpha=SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv).w;

                clip(alpha-0.1);

                return 0;

            }

            ENDHLSL

        }

    } 
}
