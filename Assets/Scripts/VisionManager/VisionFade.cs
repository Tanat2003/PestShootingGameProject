using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionFade : MonoBehaviour
{
    [Header("MatterialForGameScenceObjSetting")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material darkMaterial;
    [Space]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private SkinnedMeshRenderer skinMeshRenderer;
    [SerializeField] private bool IsEnemy;
   
    
    private void Start()
    {
        
        
    }
    

    public void SetDark(bool isDark)
    {
       
        if (skinMeshRenderer != null && !IsEnemy)
        {

            skinMeshRenderer.material = isDark ? darkMaterial : normalMaterial;
        }
        if (meshRenderer != null && !IsEnemy)
        {

            meshRenderer.material = isDark ? darkMaterial : normalMaterial;
        }
        if (IsEnemy)
        {
            if (isDark == false)
            {
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                skinMeshRenderer.GetPropertyBlock(mpb);

                Color c = Color.white;
                if (skinMeshRenderer.sharedMaterial.HasProperty("_BaseColor"))
                    c = skinMeshRenderer.sharedMaterial.GetColor("_BaseColor");

                c.a = 1f;
                mpb.SetColor("_BaseColor", c);
                skinMeshRenderer.SetPropertyBlock(mpb);
            }else if(isDark == true)
            {
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                skinMeshRenderer.GetPropertyBlock(mpb);

                Color c = Color.white;
                if (skinMeshRenderer.sharedMaterial.HasProperty("_BaseColor"))
                    c = skinMeshRenderer.sharedMaterial.GetColor("_BaseColor");

                c.a = 0f;
                mpb.SetColor("_BaseColor", c);
                skinMeshRenderer.SetPropertyBlock(mpb);
            }
            
        }
    }
}
