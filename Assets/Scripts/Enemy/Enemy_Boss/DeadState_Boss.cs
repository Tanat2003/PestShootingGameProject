using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Boss : EnemyState
{
    private EnemyBoss enemy;
    private bool interactionDisable;
    public DeadState_Boss(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.abilityState.DisableFlameThrow();
        enemy.abilityState.DisableSpinZoneDamage();

        interactionDisable = false;

        
        stateTimer = 3;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        DisableInteraction();
    }
    private void DisableInteraction()
    {
        if (stateTimer < 0 && interactionDisable == false)
        {
            interactionDisable = true;
            enemy.ragdoll.RagdollActive(false);
            enemy.ragdoll.ColliderActive(false);
        }
    }
}
