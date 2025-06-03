using UnityEngine;

public class AbilityState_Boss : EnemyState
{
    private EnemyBoss enemy;
    public AbilityState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
    }

    public override void Enter()
    {

        base.Enter();
        enemy.agent.isStopped = true;

        if (enemy.bossWeaponType == BossWeaponType.FireThrow)
        {
            stateTimer = enemy.flameThrowDuration;

        }
        else if (enemy.bossWeaponType == BossWeaponType.Capoeira)
        {
            stateTimer = enemy.spinDuration;
        }
        enemy.bossVisual.EnableWeaponTrail(true);
    }


    public override void Update()
    {
        base.Update();
        enemy.FaceTarget(enemy.player.position);

        if (stateTimer < 0 && enemy.bossWeaponType == BossWeaponType.FireThrow)
        {
            DisableFlameThrow();
        }

        if (stateTimer < 0 && enemy.bossWeaponType == BossWeaponType.Capoeira)
        {
            DisableSpinZoneDamage();
        }

        if (triggerCalled)
        {

            stateMachine.ChangeState(enemy.moveStateBoss);
        }
    }
    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        if (enemy.bossWeaponType == BossWeaponType.FireThrow)
        {
            enemy.ActivateFlameThrow(true);
            enemy.bossVisual.DischargeBattery();

        }
        if (enemy.bossWeaponType == BossWeaponType.Hammer)
        {
            Debug.Log("AbilityHasTrigger");
            enemy.ActivateHammer();
        }
        if (enemy.bossWeaponType == BossWeaponType.SummonMagic)
        {
            enemy.ActivateSummonMagic();
        }
        if (enemy.bossWeaponType == BossWeaponType.Capoeira)
        {
            enemy.ActivateSpinDamageZone(true);
        }
    }
    public void DisableFlameThrow() //ãªéËÂØ´Ê¡ÔÅ¾è¹ä¿µÍ¹à¢éÒÊÙèÊàµ¨µÒÂ
    {
        if (enemy.bossWeaponType != BossWeaponType.FireThrow)
        {
            return;
        }
        enemy.ActivateFlameThrow(false);
    }

    public void DisableSpinZoneDamage()
    {
        if (enemy.bossWeaponType != BossWeaponType.Capoeira)
            return;

        enemy.ActivateSpinDamageZone(false);
    }
    public override void Exit()
    {
        base.Exit();

        enemy.SetAbilityCooldown();
        enemy.bossVisual.ResetBatteries();
        enemy.bossVisual.EnableWeaponTrail(false);
    }
}
