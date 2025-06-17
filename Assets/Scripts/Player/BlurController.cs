using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurController : MonoBehaviour
{
    [SerializeField] private Material blurMaterial;
    [SerializeField] private float blurRadius = 7f;
    [SerializeField] private int blurIterations = 3;

    private Camera mainCamera;
    private RenderTexture renderTexture;

    private void Start()
    {
        mainCamera = Camera.main;
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        blurMaterial.SetFloat("_BlurRadius", blurRadius);
    }
    private void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (blurMaterial == null) return;

        RenderTexture temp = RenderTexture.GetTemporary(src.width, src.height);
        Graphics.Blit(src, temp);

        for (int i = 0; i < blurIterations; i++)
        {
            RenderTexture temp2 = RenderTexture.GetTemporary(temp.width, temp.height);
            Graphics.Blit(temp, temp2, blurMaterial);
            RenderTexture.ReleaseTemporary(temp);
            temp = temp2;
        }

        Graphics.Blit(temp, dest);
        RenderTexture.ReleaseTemporary(temp);
    }

}
