using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MissionSelectButton : UI_Button
{
    private UI_MissionSelection missionUI;
    [Header("MissionInfo")]
    public Mission myMission;
    [SerializeField] private TextMeshProUGUI missionName ;
    
    
    public override void Start()
    {
        base.Start();
        missionUI = GetComponentInParent<UI_MissionSelection>();
        missionName = GetComponentInChildren<TextMeshProUGUI>();
        missionName.text = myMission.missionName;
        
    }
    private void OnValidate()
    {
        gameObject.name = "Button - Select Mission : " + myMission.missionName;
        
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        missionUI.UpdateMissionDescription(myMission.missionDescription);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        missionUI.UpdateMissionDescription("เลือกภารกิจที่ต้องการ");
    }
    public override void OnPointerDown(PointerEventData eventData)
    {

        
        
        base.OnPointerDown(eventData);
        Mission_Manager.instance.SelectMission(myMission);
        UI.instance.uiInGame.SetDialog
            (Mission_Manager.instance.currentMission.dialogOwner,
            Mission_Manager.instance.currentMission.dialogPlayerWithOwner,DialogueWith.Owner);
            

    }
   
   
}
