using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="New KillHunt Mission",menuName =("Mission/EnemyHunt Mission"))]
public class Mission_EnemyHunt : Mission
{
    
    public List<Enemy_Name> enemyTarget;
    private int targetToKillLeft;
    private int defaultTargetToKillLeft;


    public override void StartMission()
    {
        base.StartMission();
        LevelGenerator.Instance.DisableUselessEnemy(enemyToEnable);
        

        Mission_ObjectHuntTarget.OnTargetKill += ReduceKillTargetAmount;

        SetEnemyTarget();
        
        UpdateMissionUI();

        
    }
   
    private void SetEnemyTarget()
    {
        List<Enemy> amountEnemy = new List<Enemy>();
        
        foreach (Enemy enemy in LevelGenerator.Instance.GetEnemyList())
        {

            if (enemyTarget.Contains(enemy.enemyName))
            {
                amountEnemy.Add(enemy);
                
                enemy.AddComponent<Mission_ObjectHuntTarget>();
                
            }
            
        }
        targetToKillLeft = amountEnemy.Count;
        defaultTargetToKillLeft = targetToKillLeft;

        
    }

    public override bool MissionCompleted()
    {
        return targetToKillLeft <= 0;
    }
    public override void ResetMissionValue()
    {
        if(defaultTargetToKillLeft != 0)
            targetToKillLeft = defaultTargetToKillLeft;
    }
    private void ReduceKillTargetAmount()
    {
        targetToKillLeft--;
        UpdateMissionUI();

        if(targetToKillLeft <= 0)
        {
            UI.instance.uiInGame.UpdateMissionInfo("ภารกิจเสร็จสิ้น",null);

            if(enemyToEnable.Count != enemyTarget.Count) //ศัตรูที่เปิด != ศัตรูที่ต้องกำจัด แปลว่าเป็นภารกิจกำจัดศัตรูเฉพาะตัว
            {
                UI_MissionSelection.instance.missionCheck.missionCompleted[1] = true; //เช็คboolตำแหน่งที่เป็นภารกิจล่าเฉพาะตัว

            }
            else
            {
                UI_MissionSelection.instance.missionCheck.missionCompleted[2] = true;
            }

            Mission_ObjectHuntTarget.OnTargetKill -= ReduceKillTargetAmount;
            GameManager.instance.GameCompleted();
        }
    }
    private void UpdateMissionUI()
    {
        //HashSetคือลิสต์ที่ไม่เอาชื่อซ้ำ
        HashSet<Enemy_Name> uniqueEnemyNames = new HashSet<Enemy_Name>(enemyTarget);

        // แปลง enum เป็นสตริง
        List<string> namesList = new List<string>();
        foreach (Enemy_Name name in uniqueEnemyNames)
        {
            namesList.Add(name.ToString());
        }
        string namesString = string.Join(", ", namesList);

        // แสดงชื่อศัตรูใน UI
        string missionText = "คุณต้องกำจัดศัตรูพืช : " + namesString;
        
        string missionDetail = "ศัตรูพืชที่เหลือ : " + targetToKillLeft;
        UI.instance.uiInGame.UpdateMissionInfo(missionText, missionDetail);
    }
    public override void UpdateMission()
    {
        base.UpdateMission();
    }
}
