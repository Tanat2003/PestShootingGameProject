using UnityEngine;

public class AttackState_Boss : EnemyState
{
    private EnemyBoss enemy;
    public float lastTimeAttack {  get; private set; }  
    public AttackState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
    }

    public override void Enter()
    {
        base.Enter();
        
        enemy.bossVisual.EnableWeaponTrail(true);
        enemy.animator.SetFloat("AttackIndex", Random.Range(0, 2));
        enemy.agent.isStopped = true;
        stateTimer = 1f;
        
    }

    public override void Exit()
    {
        base.Exit();
        lastTimeAttack = Time.time;
        enemy.bossVisual.EnableWeaponTrail(false);
    }

    public override void Update()
    {
        base.Update();

        enemy.ShouldDoAbility();

        if (triggerCalled)
        {
            
            if (enemy.PlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.idleStateBoss);
            }
            else
            {
                stateMachine.ChangeState(enemy.moveStateBoss);
            }
        }
    }
    
}
