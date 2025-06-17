using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionFade : MonoBehaviour
{
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material darkMaterial;

    private SkinnedMeshRenderer skinMeshRenderer;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        skinMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetDark(bool isDark)
    {
        if (skinMeshRenderer != null)
        {
            if (isDark == true)
            {
               skinMeshRenderer.material = darkMaterial;
            }
            else
            {
                skinMeshRenderer.material = normalMaterial;
            }
            //skinMeshRenderer.material = isDark ? darkMaterial : normalMaterial;
        }
        if(meshRenderer != null)
        {
            if(isDark == true)
            {
                meshRenderer.material =darkMaterial;
            }
            else
            {
                meshRenderer.material = normalMaterial;
            }
            //meshRenderer.material = isDark ? darkMaterial: normalMaterial;
        }
    }
}
