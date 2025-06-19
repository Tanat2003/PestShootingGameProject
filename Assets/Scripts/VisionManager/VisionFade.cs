using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionFade : MonoBehaviour
{
    [Header("SettingFadeForManyObjectWithOneMaterial")]
    [SerializeField] private Material normalMaterial;   
    [SerializeField] private MeshRenderer[] allMeshToFade;
    [SerializeField] private SkinnedMeshRenderer[] allSkinMeshToFade;
    [Space]
    [Header("SettingForManyObjectsThatHaveManyMaterial")]
    [SerializeField] private Material[] normalMaterials; //normalMaterials[i] = all[i]

    [Space]
    [Header("SettingFadeForEnemy")]
    [SerializeField] private bool isEnemy;
    [SerializeField] private Material enemyCrystalMaterial;
    [SerializeField]  private SkinnedMeshRenderer enemySkinnedMesh;
    private Enemy_Crystal[] enemyCrystals;
    
    [Space]
    [SerializeField] private Material darkMaterial;
   
    
    private void Start()
    {
        enemyCrystals = GetComponentsInChildren<Enemy_Crystal>();
       
        
    }
    

    public void SetDark(bool isDark)
    {
        
        //‡ª‘¥ª‘¥Mat¢ÕßMesh∑—ÈßÀ¡¥(Õ“«ÿ∏»—µ√Ÿ)
        SetAllTargetTo(isDark);
        if (isEnemy) //‡ªÁπenemy
        {
            if (isDark == false)
            {
                enemySkinnedMesh.material.EnableKeyword("_EMISSION");
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                enemySkinnedMesh.GetPropertyBlock(mpb);

                Color c = Color.white;
                if (enemySkinnedMesh.sharedMaterial.HasProperty("_BaseColor"))
                    c = enemySkinnedMesh.sharedMaterial.GetColor("_BaseColor");

                c.a = 1f;
                mpb.SetColor("_BaseColor", c);
                enemySkinnedMesh.SetPropertyBlock(mpb);
                SetEnemyCrstalsDarK(isDark);

                
            }
            else if (isDark == true)
            {
                enemySkinnedMesh.material.DisableKeyword("_EMISSION");
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                enemySkinnedMesh.GetPropertyBlock(mpb);

                Color c = Color.white;
                if (enemySkinnedMesh.sharedMaterial.HasProperty("_BaseColor"))
                    c = enemySkinnedMesh.sharedMaterial.GetColor("_BaseColor");

                c.a = 0f;
                mpb.SetColor("_BaseColor", c);
                enemySkinnedMesh.SetPropertyBlock(mpb);
                
                SetEnemyCrstalsDarK(isDark);
                //for (int i = 0; i < allSkinMeshToFade.Length; i++)
                //{
                //    allSkinMeshToFade[i].material = darkMaterial;
                //}
                //for (int i = 0; i < allMeshToFade.Length; i++)
                //{
                //    allMeshToFade[i].material = darkMaterial;
                //}
            }




        }
    }
    private void SetEnemyCrstalsDarK(bool isDark)
    {
        if(enemyCrystals != null)
        {
            for (int i = 0; i < enemyCrystals.Length; i++)
            {
                enemyCrystals[i].GetComponent<MeshRenderer>().material = isDark? darkMaterial : normalMaterial;


            }
        }
    }
    private void SetAllTargetTo(bool isDark)
    {
        if (allSkinMeshToFade != null )
        {
            for (int i = 0; i < allSkinMeshToFade.Length; i++)
            {
                allSkinMeshToFade[i].material = isDark ? darkMaterial : normalMaterial;
            }

        }
        if (allMeshToFade != null )
        {

            for (int i = 0; i < allMeshToFade.Length; i++)
            {
                allMeshToFade[i].material = isDark ? darkMaterial : normalMaterial;
            }
        }
        //if (normalMaterials != null && allMeshToFade != null)
        //{
        //    for (int i = 0; i < allMeshToFade.Length; i++)
        //    {
        //        allMeshToFade[i].material = isDark ? darkMaterial : normalMaterials[i];


        //    }
        //}
        //if (normalMaterials != null && allSkinMeshToFade != null)
        //{
        //    for (int i = 0; i < allMeshToFade.Length; i++)
        //    {
        //        allSkinMeshToFade[i].material = isDark ? darkMaterial : normalMaterials[i];


        //    }
        //}
    }
}
