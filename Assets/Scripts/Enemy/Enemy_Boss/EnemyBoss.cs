using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Unity.VisualScripting;
using UnityEngine;

using static UnityEngine.EventSystems.EventTrigger;

public enum BossWeaponType { FireThrow, Hammer, SummonMagic, Capoeira, AxeThrowMagic }
public class EnemyBoss : Enemy
{
    public IdleState_Boss idleStateBoss { get; private set; }
    public MoveState_Boss moveStateBoss { get; private set; }
    public AttackState_Boss attackState { get; private set; }
    public SpeacialAttack1State_Boss speacialAttack1State { get; private set; }
    public AbilityState_Boss abilityState { get; private set; }
    public EnemyBoss_Visual bossVisual { get; private set; }
    public DeadState_Boss deadState { get; private set; }
    

    [Header("Boss Detail")]
    public BossWeaponType bossWeaponType;
    public float actionCoolDown = 10; //§Ÿ≈¥“«πÏ√–À«Ë“ß °‘≈∫Õ °—∫°√–‚¥¥‚®¡µ’¢Õß∫Õ 
    public float attackRange;
    [SerializeField] private int howManyDamageBossGetToDropItem;
    private int currentGetDamaged;


    [Header("Ability")]
    public float abilityCooldown;
    private float lastTimeUsedAbility;
    [SerializeField] private float minAbilityDistance; //ºŸÈ‡≈ËπµÈÕßÕ¬ŸË„°≈È·§Ë‰Àπ∂÷ß®–„™È °‘≈‰¥È

    [Header("Boss Skill Setting")]
    [Space]
    [Header("FlameThrow")]
    [SerializeField] private ParticleSystem flameThrow;
    public float flameThrowDuration;
    public float flameDamageCooldown; //§Ÿ≈¥“«πÏ¥“‡¡®µÕπ∑’ËºŸÈ‡≈Ëπ‚¥π‰ø
    public int flameDamage;
    public bool flameThrowActive { get; private set; }

    [Header("Hammer")]
    public GameObject hammerFxPrefab;
    public int hammerActiveDamage;//¥“‡¡®¢Õß °‘≈
    [SerializeField] private float hammerCheckRadius; //¢π“¥¢Õß«ß °‘≈

    [Header("SummonMagic")]
    [SerializeField] private GameObject summonedprefab;
    [SerializeField] private ParticleSystem summonFX;

    [Header("Capoeira")]
    public float spinDuration;
    public float spindDamageCooldown;
    public int spinDamage;
    [SerializeField] private ParticleSystem healingFX;
    [SerializeField] private float healingDelay;
    [SerializeField] private int healPerDelay;

    [Header("ThrowAxeMagic")]
    [SerializeField] private float throwFlySpeed;
    [SerializeField] private float throwTimer;
    [SerializeField] private int throwDamage;
    [SerializeField] private GameObject throwPrefab;
    [SerializeField] private ParticleSystem[] throwStartPoint;

    public bool spinActive {  get; private set; }

    [Header("Jump attack/SpeacialAttack1")]
    public float travelTimeToTarget = 1;
    public float jumpAttackCooldown = 2;
    public int jumpAttackDamage;
    private float lastTimeJump;
    public float minJumpDistanceRequired;//ºŸÈ‡≈ËπµÈÕßÕ¬ŸË„°≈È·§Ë‰Àπ∂÷ß®–°√–‚¥¥‰¥È
    public Transform impactPoint;
    [Space]
    public float impactRadius = 2.5f;
    public float impactPower = 5;
    [SerializeField] private float upwardsMultiplier = 10;

    [Header("Attack Data")]
    public int meleeAttackDamage;
    [SerializeField] private Transform[] damagePoints;
    [SerializeField] private float attackRadius; //hitbox¢Õßattack∑’Ë‚®¡µ’ºŸÈ‡≈Ëπ
    [SerializeField] private GameObject meleeAttackFX;

    [Space]
    [SerializeField] private LayerMask whatToIgnore;

    protected override void Awake()
    {
        base.Awake();
        

        idleStateBoss = new IdleState_Boss(this, stateMachine, "Idle");
        moveStateBoss = new MoveState_Boss(this, stateMachine, "Move");
        attackState = new AttackState_Boss(this, stateMachine, "Attack");
        speacialAttack1State = new SpeacialAttack1State_Boss(this, stateMachine, "SpeacialAttack1");
        abilityState = new AbilityState_Boss(this, stateMachine, "Ability");
        deadState = new DeadState_Boss(this, stateMachine, "Idle");

        bossVisual = GetComponent<EnemyBoss_Visual>();
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleStateBoss);


    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        if (ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }
        MeleeAttackCheck(damagePoints, attackRadius, meleeAttackFX,meleeAttackDamage);

    }
    public override void EnterBattleMode()
    {
        if (inBattleMode)
        {
            return;
        }
        base.EnterBattleMode();
        stateMachine.ChangeState(moveStateBoss);
    }

    public override void Die()
    {
        base.Die();
        if (stateMachine.currentState != deadState)
        {
            stateMachine.ChangeState(deadState);

        }
    }
    public bool CanDoAbility()
    {
        bool playerWithInDistance =
            Vector3.Distance(transform.position, player.position) < minAbilityDistance;
        if (playerWithInDistance == flameThrow)
        {
            return false;
        }

        if (Time.time > lastTimeUsedAbility + abilityCooldown)
        {

            return true;
        }
        return false;
    }
    public void ActivateFlameThrow(bool activate)
    {
        flameThrowActive = activate;
        if (activate == false)
        {
            flameThrow.Stop();
            animator.SetTrigger("StopAbility");
            return;
        }

        //°”Àπ¥‡«≈“‡≈Ëπparticle∑—Èßµ—«∑’Ë‡ªÁπChildren
        var mainMoudle = flameThrow.main;
        var extraModule = flameThrow.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        mainMoudle.duration = flameThrowDuration;
        extraModule.duration = flameThrowDuration;

        flameThrow.Clear();
        flameThrow.Play();
    }
    public void ShouldDoAbility()
    {
        if (CanDoAbility())
        {
            if (bossWeaponType == BossWeaponType.Capoeira)
            {
                if (health.currentHealth <= health.minHealthToHealth)
                {
                    stateMachine.ChangeState(abilityState);
                }
            }
            else
            {
                    stateMachine.ChangeState(abilityState);

            }

        }
            


         
    }
    #region CapeoriaSkill
    public void ActivateSpinDamageZone(bool activate)
    {
        spinActive = activate;
        if(activate == false)
        {
            animator.SetTrigger("StopAbility");
            return;
        }
        var mainMoudule = healingFX.main;
        var extraModule = healingFX.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        mainMoudule.duration = spinDuration;
        extraModule.duration = spinDuration;

        healingFX.Clear();
        healingFX.Play();
        StartCoroutine(BossIsRecoveryHP(spinDuration,healingDelay));
    }

    private IEnumerator BossIsRecoveryHP(float duration,float delay)
    {
        float time = 0;
        while (time < duration)
        {

            yield return new WaitForSeconds(delay);
            health.IncreaseHealth(healPerDelay);
            UpdateEnemyHealthBar(health.currentHealth, health.maxHealth);
            time += delay;

        }
        
    }

    #endregion
    public void ActivateHammer()
    {
        GameObject newActivation = Object_Pool.instance.GetObject(hammerFxPrefab, impactPoint);
        Object_Pool.instance.ReturnObject(newActivation, 1);
        MassDamage(damagePoints[0].position, hammerCheckRadius,hammerActiveDamage);
    }
    public void ActivateSummonMagic()
    {
        summonFX.Play();
        for (int i = 0; i < partrolPoints.Length; i++)
        {
            Object_Pool.instance.GetObject(summonedprefab, partrolPoints[i]);
        }
    }
    
    public void ActivateAxeThrowMagic()
    {
        for (int i = 0; i < throwStartPoint.Length; i++)
        {
            throwStartPoint[i].Play();
            GameObject newItemThrow = Object_Pool.instance.GetObject(throwPrefab, throwStartPoint[i].transform);
            newItemThrow.transform.position = throwStartPoint[i].transform.position;
            newItemThrow.GetComponent<EnemyThrow>().
                EnemyThrowSetup(throwFlySpeed, player, throwTimer, throwDamage);
        }

    }
    public void SetAbilityCooldown() => lastTimeUsedAbility = Time.time; //‡´Á∑§Ÿ≈Ï¥“«À≈—ß‡≈ËπÕπ‘‡¡™—Ëπ‡ √Á®
    public void JumpImpact()
    {
        Transform impactPoint = this.impactPoint;
        if (impactPoint == null)
        {
            impactPoint = transform;
        }
        MassDamage(impactPoint.position, impactRadius,jumpAttackDamage);

    }
    private void MassDamage(Vector3 impactPoint, float impactRadius,int damage) //§«∫§ÿ¡¥“‡¡®°“√‚®¡µ’¢Õß»—µ√Ÿ∑∑’Ë‡ªÁπ«ß°«È“ß
    {
        HashSet<GameObject> uniqueEntitie = new HashSet<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(impactPoint, impactRadius, ~whatIsAlly);
        foreach (Collider hit in colliders)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if (damagable != null) //‡™Á§«Ë“¿“¬„πæ◊Èπ∑’Ë∑’Ë°√–‚¥¥≈ß¡“obj‰Àπ¡’Idamagable∫È“ß
            {
                GameObject rootEntitie = hit.transform.root.gameObject;
                if (uniqueEntitie.Add(rootEntitie) == false)
                {
                    continue;
                }
                damagable.TakeDamage(damage);
            }

            ApplyPhysicalForce(impactPoint, impactRadius, hit);
        }

    }

    private void ApplyPhysicalForce(Vector3 impactPoint, float impactRadius, Collider hit)
    {
        //Physic effect
        Rigidbody rb = hit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(impactPower, impactPoint, impactRadius, upwardsMultiplier, ForceMode.Impulse);
        }
    }

    public bool CanDoJumpAttack()
    {
        float distanceToplayer = Vector3.Distance(transform.position, player.position);
        if (distanceToplayer < minJumpDistanceRequired) //ºŸÈ‡≈ËπÕ¬ŸË„°≈È‚¥¥‰¡Ë‰ª‰¡Ë®”‡ªÁπµÈÕß°√–‚¥¥
        {
            return false;
        }
        if (Time.time > lastTimeJump + jumpAttackCooldown && PlayerInClearSight())
        {

            return true;
        }
        return false;
    }
    public void SetJumpAttackCooldown() => lastTimeJump = Time.time;

    public bool PlayerInClearSight()
    {
        Vector3 enemyPos = transform.position + new Vector3(0, 3.07f, 0); //+∫«°¥È«¬§«“¡ Ÿß¢Õß∫Õ 
        Vector3 playerPos = player.position + Vector3.up;
        Vector3 directionToPlayer = (playerPos - enemyPos).normalized;
        //„ÀÈµ√«®®—∫„π√–¬–∑“ß«Ë“¡’LayerMaskÕ–‰√¢«“ßÕ¬ŸË¡—È¬·µË „ÀÈignore LayerMask∑’Ë‡ªÁπenemy
        if (Physics.Raycast(enemyPos, directionToPlayer, out RaycastHit hit, 100, ~whatToIgnore))
        {
            //∫“ß∑’‡«≈“raycast®“°∑’Ë ŸßÊ‰¡Ë‚¥πCollider¢ÕßºŸÈ‡≈Ëπ‡≈¬µÈÕß‡™Á§«Ë“parent¢Õß¡—π¡’LayerMask player¡—È¬
            if (hit.transform.root == player.root)
            {
                return true;
            }
        }
        return false;
    }
    public override void GetHit(int damage)
    {
        currentGetDamaged += damage;
        base.GetHit(damage);
        
        //∫Õ ‚¥π¬‘ß®– ÿË¡¥√Õª‰Õ‡∑¡ÕÕ°¡“
        if(currentGetDamaged >= howManyDamageBossGetToDropItem)
        {
            currentGetDamaged = 0;
            float randomValue = UnityEngine.Random.Range(0f,1f);
            int randomIndex = UnityEngine.Random.Range(0, partrolPoints.Length);
            if (randomValue >= .8f)
            {
                Object_Pool.instance.GetObject(healthBox, partrolPoints[randomIndex]);                
            }
            else if (randomValue >= .5f)
            {
                Object_Pool.instance.GetObject(ammoDrop, partrolPoints[randomIndex]);
            }
            


        }

        ShouldDoAbility();
    }
    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, player.position) < attackRange;
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minJumpDistanceRequired);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, impactRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minAbilityDistance);

        if (damagePoints.Length > 0)
        {
            foreach (var point in damagePoints)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(point.position, attackRadius);
            }
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(damagePoints[0].position, hammerCheckRadius);
        }

    }

}
