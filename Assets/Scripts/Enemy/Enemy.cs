using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public enum Enemy_Name
{
    GoldenAppleSnail, FiledRat, FiledCrab, Planthopper, RiceStemBorerWorm,
    RiceWeevil, RiceMoth, FlourBeetle, Bird, GrassHopper,RatKing, RiceStemBorerWormKing
}
[RequireComponent(typeof(NavMeshAgent))]

public class Enemy : MonoBehaviour
{

    public EnemyStateMachine stateMachine { get; private set; }
    public EnemyState idleState { get; private set; }
    public EnemyState moveState { get; private set; }
    public Animator animator { get; private set; }
    public Transform player { get; private set; }
    public Enemy_Health health { get; private set; }
    public EnemyVisual visual { get; private set; }
    public Enemy_DropController dropController { get; private set; }
    public bool inBattleMode { get; private set; }
    protected bool isMeleeAttackReady;

    public AudioManager audioManager { get; private set; }

    public LayerMask whatIsPlayer;
    public LayerMask whatIsAlly;
    public LayerMask whatIsCar;

    [Header("Enemy Document(Not About GamePlay")]
    public Enemy_Name enemyName;
    public string enemyDetail;

    public Ragdoll ragdoll { get; private set; }
    


    [Space]

    [Header("StateInfo")]
    public float idleTime;
    public float agressionRange;

    [Header("MoveInfo")]
    public float moveSpeed;
    public float chaseSpeed;
    
    private Vector3[] patrolPointPosition;
    public Transform[] partrolPoints; //จุดหมายที่aiจะเดินไป
    private bool manualMovement;
    private bool manualRotation;

    private int currentPartrolIndex;
    [Header("EnemyDropItem")]
    [SerializeField] private GameObject buffDrop;
    [SerializeField] private GameObject weaponDrop;
    public GameObject ammoDrop;
    public  GameObject healthBox;
    [SerializeField] private GameObject orbDrop;

    [Header("EnemyHealthBar")]
    [SerializeField] private Image healthBar;
    private Object_Pool pool;
    public NavMeshAgent agent { get; private set; }


    protected virtual void Awake()
    {
        ragdoll = GetComponent<Ragdoll>();
        stateMachine = new EnemyStateMachine();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        visual = GetComponent<EnemyVisual>();
        dropController = GetComponent<Enemy_DropController>();
        health = GetComponent<Enemy_Health>();
        
        player = GameObject.Find("Player").GetComponent<Transform>();
        //?.transform.Find("player_main");

        



    }
    protected virtual void Start()
    {
        InitializePatrolPoints();
        audioManager = AudioManager.instance;
        agent.updateRotation = false;
        
        pool = Object_Pool.instance;
    }

    protected virtual void Update()
    {

    }
    
    protected virtual void FixedUpdate()
    {
        
        
    }

    #region PatrolPoint Method
    public Vector3 GetPartrolDestination()
    {
        Vector3 destination = patrolPointPosition[currentPartrolIndex];
        currentPartrolIndex++; //เพิ่มไว้รอใช้ชี้ตำแหน่งถัดไป
        if (currentPartrolIndex >= partrolPoints.Length)
        {
            currentPartrolIndex = 0;
        }
        return destination;
    }
    private void InitializePatrolPoints() //เอาตำแหน่งที่ให้aiเดินออกจากตัวaiเพื่อไม่ให้เวลาเดินแล้ว
                                          //ตำแหน่งเดินตามไปด้วย
    {

        patrolPointPosition = new Vector3[partrolPoints.Length];

        for (int i = 0; i < partrolPoints.Length; i++)
        {
            patrolPointPosition[i] = partrolPoints[i].position;
            partrolPoints[i].gameObject.SetActive(false);

        }

    }

    
    #endregion

    #region HitMethod
    public virtual void GetHit(int damage) //เช็คว่าenemyโดนยิง
    {
        EnterBattleMode(); //ถ้าเราเรียกเมธอดในสคริตป์ที่มันอยู่มันจะไม่เรียกตัวเมธอดที่ถูกoverride
        health.ReduceHealth(damage);
        UpdateEnemyHealthBar(health.currentHealth, health.maxHealth);
        if (health.ShouldDie())
        {
            
            Die();
        }
        PlayBGMReadyTofight();
    }
    public void UpdateEnemyHealthBar(float currentHealth,float maxHealth)
    {

        healthBar.fillAmount = currentHealth / maxHealth;
    }

    private void PlayBGMReadyTofight()
    {
        if (GameManager.instance.player.inbattle == true)
            return;
        
        audioManager.PlayBGM(10);
        GameManager.instance.player.inbattle = true;
    }

    public virtual void Die()
    {
        ragdoll.RagdollActive(true);
        animator.enabled = false;
        agent.isStopped = true;
        agent.enabled = false;

        StartCoroutine(SetEnemyActiveFalseAndDropItem());
        

        Mission_ObjectHuntTarget huntTarget = GetComponent<Mission_ObjectHuntTarget>();
        huntTarget?.InvokeOnTargetKill();
        GameDataManager.instance.EnemyKilled
            (Mission_Manager.instance.currentMission.missionName);


    }

    

    private IEnumerator SetEnemyActiveFalseAndDropItem()
    {
        yield return new WaitForSeconds(2.5f);
        transform.gameObject.SetActive(false);
        if (enemyName == Enemy_Name.FiledCrab)
        {
            Object_Pool.instance.GetObject(buffDrop, transform,true);

        }else if(enemyName == Enemy_Name.FlourBeetle)
        {

            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= .6f)
            {
                Object_Pool.instance.GetObject(buffDrop, transform, true);
            }
            else if (randomValue >= .6f)
            {
                Object_Pool.instance.GetObject(orbDrop, transform, true);
            }
        }
        else
        {
            RandomDropItem();
        }
    }

    private void RandomDropItem()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue >= .8f)
            return;
        if (randomValue <= .4f)//40% 0.0-0.4
        {
            pool.GetObject(ammoDrop, transform, true);
            return;
        }else if(randomValue <=.6f)//20% 0.4-0.6
        {
            pool.GetObject(weaponDrop, transform, true);
            return;
        }else if(randomValue <= .8f)//20% 0.6 - 0.8
        {
            pool.GetObject(healthBox, transform);

        }
        
    }

    public virtual void MeleeAttackCheck(Transform[] damagePoints, float attackCheckRadius, GameObject fx, int damage) //เช็คว่าอาวุธของEnemyโจมตีโดนผู้เล่นรึยังตอนเล่นอนิเมชั่นโจมตี
    {
        if (isMeleeAttackReady == false)
        {
            return;
        }
        foreach (Transform attackPoint in damagePoints)
        {
            LayerMask canTakeDamage = whatIsPlayer | whatIsCar;
            // เก็บColliderเป็นอาเรย์ โดยใช้Physic Overlap(attackPointกับ Radiusของอาวุธ ว่าไปโดนเลเยอร์ผุ้เล่นมั้ย)
            Collider[] detectHits =
                Physics.OverlapSphere(attackPoint.position, attackCheckRadius, canTakeDamage);
            //ถ้าเราได้Colliderก็หยุดloop แล้วเรียกใช้TakeDamageในIdamagable
            for (int i = 0; i < detectHits.Length; i++)
            {
                IDamagable damagable = detectHits[i].GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(damage);
                    isMeleeAttackReady = false;
                    GameObject newAttackFx =
                        Object_Pool.instance.GetObject(fx, attackPoint);
                    Object_Pool.instance.ReturnObject(newAttackFx, 1);
                    return;
                }
            }


        }

    }
    public void EnableMeleeAttackCheck(bool enable) => isMeleeAttackReady = enable;

    public virtual void BulletImpact(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        if (health.ShouldDie())
        {
            StartCoroutine(HitImpactCourutine(force, hitPoint, rb));


        }
        //ใช้Corutineเพื่อให้มีดีเลย์นิดหน่อยกันบัค
    }

    private IEnumerator HitImpactCourutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(.1f);
        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);   //เพิ่มแรงกระแทกเข้าไปจุดที่เรายิงโดน 
    }
    #endregion

    #region Enter&ShouldEnterBattle
    protected bool ShouldEnterBattleMode()
    {
        bool inAgressionRange = Vector3.Distance(transform.position, player.position) < agressionRange;
        if (inAgressionRange && !inBattleMode)
        {
            EnterBattleMode();
            return true;
        }
        return false;
    }
    public virtual void EnterBattleMode()
    {
        inBattleMode = true;
    }
    #endregion

    #region Animation Method
    public virtual void AbilityTrigger()
    {
        stateMachine.currentState.AbilityTrigger();
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();




    public void ActivateManualMovement(bool manualMovement) => this.manualMovement = manualMovement;
    public void ActivateManualRotation(bool manualRotation) => this.manualRotation = manualRotation;
    public bool ManualMovementActive() => manualMovement;
    public bool ManualRotationActive() => manualMovement;
    #endregion


    public void FaceTarget(Vector3 target)
    {
        float turnSpeed = 10;
        //คำนวณทิศทาง
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0;

        
        if (direction == Vector3.zero) return;

        
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );
    }
    
    protected virtual void OnDrawGizmos() //วาดขนาดระยะที่จะให้aiใช้detectผู้เล่น ถ้าเข้ามาในระยะให้Aiวิ่งไปหา
    {
        Gizmos.DrawWireSphere(transform.position, agressionRange);

    }

}
