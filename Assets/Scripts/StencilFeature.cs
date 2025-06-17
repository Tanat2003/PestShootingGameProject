using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class StencilFeature : MonoBehaviour
{
    class StencilPass : ScriptableRenderPass
    {
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // ใช้สำหรับเขียน Stencil Buffer
        }
    }

    StencilPass m_StencilPass;

  
}
