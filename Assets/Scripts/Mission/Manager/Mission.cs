using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Mission : ScriptableObject //Abstracr class ‡√“ “¡“√∂ √È“ßAbstractMethod‰¥È
{
    public string missionName;
    [Header("MissionDetail")]
    [TextArea]
    public string missionDescription;
    [TextArea]
    public string missionCompleteDescription;
    [Space]
    [Header("OwnerDialog")]
    public string[] dialogOwner;
    public string[] dialogPlayerWithOwner;
    [Header("BossDialog")]
    public string[] dialogBoss;
    public string[] dialogPlayerWithBoss;
   
    [Space]

    [Header("Enemy To Enable")]
    public List<Enemy_Name> enemyToEnable;
    [Header("Boss Setting")]
    public GameObject bossToSpawn;
    public float timeToSpawnBoss;
    public bool bossSpawned = false;
    private string bossTimertext;


    private float timeTospawnDefault;
    [Header("SkyBox")]
    public Material skyBoxMaterial;









    public virtual void StartMission()
    {
        timeTospawnDefault = timeToSpawnBoss;

    }
    public abstract bool MissionCompleted();
    public virtual void ResetMissionValue()
    {
        bossSpawned = false;
        Mission_Manager.instance.startMission = false;
    }

    public virtual void UpdateMission()
    {

        if(ShouldCountDownBossSpawn())
        {
            timeToSpawnBoss -= Time.deltaTime;
            UpdateBossWaringText();
        }
        else if(ShouldSpawnBoss())
        {
            bossSpawned = true;
            timeToSpawnBoss = timeTospawnDefault;
            SpawnBoss();
        }

    }

    private void SpawnBoss()
    {
        GameObject boss = Object_Pool.instance.GetObject(bossToSpawn, GameManager.instance.
            player.GetComponentInChildren<BossSpawnPoint>().GetClearSpawnPoint());
        boss.transform.parent = null;


        UI.instance.uiInGame.UpdateBossWaringInfo(null);
        StartBossDialogue(boss);
    }

    private void StartBossDialogue(GameObject boss)
    {
        
        CameraManager.instance.ChangeCameraTarget(boss.transform);
        
        UI.instance.uiInGame.SetBossDialog
            (Mission_Manager.instance.currentMission.dialogBoss,
            Mission_Manager.instance.currentMission.dialogPlayerWithBoss);

        
        UI.instance.uiInGame.StartBossDialogueWithDelay();
    }

    private void UpdateBossWaringText()
    {
        bossTimertext = System.TimeSpan.FromSeconds(timeToSpawnBoss).ToString("mm' :  'ss");
        string textToShow = "‚ª√¥√–«—ß∫Õ „π  " + bossTimertext;
        UI.instance.uiInGame.UpdateBossWaringInfo(textToShow);
    }

    private bool ShouldCountDownBossSpawn() => bossToSpawn != null && timeToSpawnBoss > 0 && !bossSpawned && Mission_Manager.instance.startMission == true && GameManager.instance.player.health.currentHealth > 1;
    private bool ShouldSpawnBoss() => timeToSpawnBoss <= 0 && bossToSpawn != null;

}

