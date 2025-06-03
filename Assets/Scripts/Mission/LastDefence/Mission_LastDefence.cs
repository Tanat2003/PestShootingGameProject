using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Defense Mission", menuName = ("Mission/DefenseMission"))]
public class Mission_LastDefence : Mission
{
    private float defenceTimer;
    [SerializeField] private bool defenceBegan;
    [SerializeField] private float defenceDuration;




    [Header("RespawnDetail")]
    [SerializeField] private int amountOfRespawnPoints = 2;
    [SerializeField] private List<Transform> respawnPoints;
    [SerializeField] private int enemysPerWave;
    [SerializeField] private GameObject[] possibleEnemy;
    private Vector3 defencePoint;
    [Header("Cooldown")]

    [SerializeField] private float waveCooldown;
    [SerializeField] private float waveTimer;

    private string defenceTimerText;


    public override void StartMission()
    {
        LevelGenerator.Instance.DisableUselessEnemy(enemyToEnable);


        Mission_ObjectStartDefence.startDefence += StartDefenceEvent;
        defencePoint = FindObjectOfType<Mission_ObjectDefenceZone>().transform.position;
        respawnPoints = new List<Transform>(ClosesetPoint(amountOfRespawnPoints));
        UI.instance.uiInGame.UpdateMissionInfo("จงปกป้องไร่นี้ด้วยชีวิตของคุณ");
    }
    public override bool MissionCompleted()
    {
        if (defenceBegan == false)
        {
            StartDefenceEvent();
            return false;
        }
        return defenceTimer < 0;
    }
    public override void UpdateMission()
    {

        if (defenceBegan == false)
        {
            return;
        }

        waveTimer -= Time.deltaTime;
        if (defenceTimer > 0)
            defenceTimer -= Time.deltaTime;

        if (defenceTimer <= 0)
        {
            UI_MissionSelection.instance.missionCheck.missionCompleted[0] = true;
            Mission_ObjectStartDefence.startDefence -= StartDefenceEvent;
            GameManager.instance.GameCompleted();
        }


        if (waveTimer < 0)
        {
            CreateNewEnemy(enemysPerWave);
            waveTimer = waveCooldown;
        }
        UpdateMissionUI();
    }
    public override void ResetMissionValue()
    {

        defenceBegan = false;



    }

    private void UpdateMissionUI()
    {
        defenceTimerText = System.TimeSpan.FromSeconds(defenceTimer).ToString("mm' :  'ss");
        string missionText = "ศัตรูพืชจะวิ่งเข้ามาหาโปรดระวังไว้";
        string missionDetail = "เวลาที่เหลือ : " + defenceTimerText;
        UI.instance.uiInGame.UpdateMissionInfo(missionText, missionDetail);
    }

    private void CreateNewEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (respawnPoints[i] == null)
                return;

            int randomEnemyIndex = Random.Range(0, possibleEnemy.Length);
            int randomRespawnIndex = Random.Range(0, respawnPoints.Count);

            Transform randomRespawnPoint = respawnPoints[randomRespawnIndex];
            GameObject randomEnemy = possibleEnemy[randomEnemyIndex];


            Object_Pool.instance.GetObject(randomEnemy, randomRespawnPoint,true);

            randomEnemy.GetComponent<Enemy>().agressionRange = 200;
            
            
            MinimapController.instance.GetEnemyPosition();
        }
    }


    private List<Transform> ClosesetPoint(int amount)
    {
        //หาObjที่มีสคิรปต์นี้แล้วเรียงลำดับจากระยะทางจากpoint.tranform(ตัวobjที่มีสคิรปต์)จนถึงdefencPoint
        //เอาข้อมูลจำนวน amount แล้วเปลี่ยนข้อมูลเป็นtransform จากนั้นเปลี่ยนเป็นList
        return FindObjectsOfType<Mission_ObjectEnemyRespawnPoint>()
            .OrderBy(point => Vector3.Distance(point.transform.position, defencePoint))
            .Take(amount)
            .Select(point => point.transform)
            .ToList();

    }

   


    private void StartDefenceEvent()
    {
        if (defenceBegan == true)
            return;
        waveTimer = .5f;
        defenceTimer = defenceDuration;
        defenceBegan = true;
    }

}
