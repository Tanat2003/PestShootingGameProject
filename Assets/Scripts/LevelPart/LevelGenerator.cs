using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    [Header("NavMesh")]
    [SerializeField] private NavMeshSurface navMeshSurface;

    [Header("LevelPart")]
    [SerializeField] private List<Transform> levelParts; //originalList
    private List<Transform> currentLevelParts; //newlvPart
    private List<Transform> generatedLevelParts = new List<Transform>();//��ʵ���źlevelpart����������ͧ��������
    [SerializeField] private Transform lastLevelPart;
    [SerializeField] private SnapPoint nextSnapPoint;
    private SnapPoint defaultSnapPoint;

    [Header("CoolDown")]
    [SerializeField] private float generationCooldown;
    private float cooldownTimer;
    private bool generationOver = true; //�͹�á�ѹ��false�͹Initialize


    public List<Enemy> enemys; //���enable�ѵ�����ʵ���ѧ�ҡ������ҧlevelPart�ء�ѹ����(�ԴEnable���͹�á)


    private Mission_Manager missionManager;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {



        enemys = new List<Enemy>();
        defaultSnapPoint = nextSnapPoint;

        missionManager = Mission_Manager.instance;

    }
    private void Update()
    {
        if (generationOver)
        {
            return;
        }
        cooldownTimer -= Time.time;
        if (cooldownTimer < 0)
        {
            if (currentLevelParts.Count > 0)
            {
                cooldownTimer = generationCooldown;
                GenerateNextLevelPart();
            }
            else if (generationOver == false)
            {
                FinishGeneration();
                GameManager.instance.EnableRice();
            }
        }
    }

    [ContextMenu("Restart Generation")]
    public void InitializeGeneration() //���ҧlevelpart����������ѹ���Ż�ѹ
    {
        nextSnapPoint = defaultSnapPoint;
        generationOver = false;
        currentLevelParts = new List<Transform>(levelParts);
        DestroyOldLevelPart();

    }

    private void DestroyOldLevelPart()
    {
        foreach (Enemy enemy in enemys) //źEnemy�������͡�ҡlevelPart
        {
            Destroy(enemy.gameObject);
        }


        foreach (Transform t in generatedLevelParts)
        {
            Destroy(t.gameObject);
        }

        generatedLevelParts = new List<Transform>();
        enemys = new List<Enemy>();
    }

    private void FinishGeneration()
    {
        generationOver = true;
        GenerateNextLevelPart();

        navMeshSurface.BuildNavMesh(); //���ҧnavmesh���levelpart�������ҵ�͡ѹ

        foreach (Enemy enemy in enemys) //Enable�ѵ����ѧ�ҡ���Bake NavMesh�������ͻ�ͧ�ѹerror����ѵ���������ö�Թ��Navmesh��
        {
            enemy.transform.parent = null;
            enemy.gameObject.SetActive(true);
        }

        missionManager.StartMission();
        missionManager.SetSkyForMission();
        foreach (Enemy enemy in enemys)
        {
            enemy.GetComponent<VisionFade>()?.SetDark(false);

        }



    }

    [ContextMenu("Create next level part")]
    private void GenerateNextLevelPart()
    {
        Transform newPart = null;
        if (generationOver)
        {
            newPart = Instantiate(lastLevelPart);
        }
        else
        {
            newPart = Instantiate(ChooseRandomPart());
        }
        generatedLevelParts.Add(newPart);

        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();


        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);
        if (levelPartScript.InterSectionDetected())
        {
            InitializeGeneration();
            return;
        }
        nextSnapPoint = levelPartScript.GetExitPoint(); //��˹�nextpoint��ExitPoint�ͧpart���

        enemys.AddRange(levelPartScript.Myenemy()); //�����ѵ�������levelPartŧ�����
    }
    private Transform ChooseRandomPart()
    {   //����SnapPart����ѧ��������ҧ 
        int randomIndex = Random.Range(0, currentLevelParts.Count);
        Transform choosenPart = currentLevelParts[randomIndex];

        currentLevelParts.RemoveAt(randomIndex);

        return choosenPart;
    }


    public void DisableUselessEnemy(List<Enemy_Name> enemyToEnable = null)
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();
        if (enemyToEnable != null)
        {
            foreach (var enemy in enemys)
            {

                if (!enemyToEnable.Contains(enemy.enemyName))
                {

                    enemy.gameObject.SetActive(false);
                    enemiesToRemove.Add(enemy);



                }
                else
                {

                    enemy.gameObject.SetActive(true);
                }
            }
            foreach (Enemy enemy in enemiesToRemove)
            {
                enemys.Remove(enemy);
            }

        }

        else
        {
            foreach (var enemy in enemys)
            {
                enemy.gameObject.SetActive(false);
                enemiesToRemove.Add(enemy);

            }
            foreach (Enemy enemy in enemiesToRemove)
            {
                enemys.Remove(enemy);
            }

        }

    }
    public List<Enemy> GetEnemyList() => enemys;
}
