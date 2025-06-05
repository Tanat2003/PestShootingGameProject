using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission_Manager : MonoBehaviour
{
    public static Mission_Manager instance;
    
    public Mission currentMission;//Clone¢Í§ScriptableµÑÇ¨ÃÔ§ à¾×èÍãËéÃÕà«ç·¤èÒºÒ§µÑÇ¢éÒÁ«Õ¹
    public bool startMission = false;




    private void Awake()
    {
        instance = this;
    }
   
    private void Update()
    {
        currentMission?.UpdateMission();
    }
    public void SelectMission(Mission newMission)
    {
        currentMission = Instantiate(newMission);
        
        
    }
    public void StartMission() => currentMission.StartMission();
    public void SetSkyForMission()
    {
        RenderSettings.skybox = currentMission.skyBoxMaterial;
    }    
    public bool MissionCompleted() => currentMission.MissionCompleted();





}




