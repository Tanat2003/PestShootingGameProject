using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Timer Mission",menuName ="Mission/ Timer")]
public class Mission_Timer : Mission
{
    
    [SerializeField] private float time;
    private float timeLeft;
    private string timerText;
    private bool startTimer = false;


    public override void StartMission()
    {
        timeLeft = time;
        LevelGenerator.Instance.DisableUselessEnemy(enemyToEnable);
        startTimer = true;
    }
    public override void UpdateMission()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0 && startTimer == true)
        {
            GameManager.instance.GameOver();
        }
        UpdateMissionUI();
    }

    private void UpdateMissionUI()
    {
        timerText = System.TimeSpan.FromSeconds(timeLeft).ToString("mm' :  'ss");
        string missionText = "มุ่งหน้าไปโรงสีข้าวก่อนเวลาหมด";
        string missionDetail = "เวลาที่เหลือ : " + timerText;
        UI.instance.uiInGame.UpdateMissionInfo(missionText, missionDetail);
    }

    public override bool MissionCompleted()
    {
        UI_MissionSelection.instance.missionCheck.missionCompleted[3] = true; //เรารู้ว่าภารกิจไหนเป็นด่านเท่าไหร่เลยเช็คแบบนี้เลย


        return timeLeft > 0;
    }

    public override void ResetMissionValue()
    {
        
    }
}
