Shader "__XR__/Normals Texture"
{
    Properties
    {
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType" = "Opaque" 
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float3 viewNormal : NORMAL;

				UNITY_VERTEX_OUTPUT_STEREO  //uint stereoEyeIndex : SV_RenderTargetIndex;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
				
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(0);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.viewNormal = COMPUTE_VIEW_NORMAL;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                return float4(i.viewNormal, 0);
            }
            ENDCG
        }
    }
}
