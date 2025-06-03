using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_MissionSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionDescription;
    [SerializeField] private UI_MissionSelectButton[] missions;
    public Mission_CompletedCheck missionCheck;
    public static UI_MissionSelection instance;

    private void Start()
    {
        instance = this;
    }

    public void UpdateMissionDescription(string text)
    {   if(missionDescription != null)
            missionDescription.text = text;
    }
   
    public void CheckAvailableLevel()
    {
        
        for (int i = 0; i < missions.Length; i++)
        {
            if (i == 0) // ด่านแรกเปิดไว้เสมอ
            {
                missions[i].gameObject.SetActive(true);
                continue;
            }
            

            // ตรวจสอบว่า Mission ก่อนหน้าถูกเคลียร์หรือไม่
            if (missionCheck.missionCompleted[i-1]==true)
            {
                missions[i].gameObject.SetActive(true);
            }
            else
            {
                missions[i].gameObject.SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        CheckAvailableLevel();
    }


}
