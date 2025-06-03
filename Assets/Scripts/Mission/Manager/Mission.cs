using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public abstract class Mission : ScriptableObject //Abstracr class ‡√“ “¡“√∂ √È“ßAbstractMethod‰¥È
{   
    public string missionName;

    [Header("Dialog")]
    [TextArea]
    public string missionDescription;
    [TextArea]
    public string missionCompleteDescription;
    public string[] dialogPlayer;
    public string[] dialogOwner; // ∫∑æŸ¥¢Õß B
    [Space]
    
    [Header("Enemy To Enable")]
    public List<Enemy_Name> enemyToEnable;
    [Header("SkyBox")]
    public Material skyBoxMaterial;

    


    
    
    
    

    public abstract void StartMission();
    public abstract bool MissionCompleted();
    public abstract void ResetMissionValue();
    public virtual void UpdateMission()
    {

    }
    
   

   
    

}
