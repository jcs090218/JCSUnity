Shader "SpriteMask/ClearStencil"
{
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }
 
        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend One OneMinusSrcAlpha
        ColorMask 0
 
        Pass
        {
            Stencil
            {
                Ref 0
                Comp Always
                Pass Zero
            }
        }
    }
}

