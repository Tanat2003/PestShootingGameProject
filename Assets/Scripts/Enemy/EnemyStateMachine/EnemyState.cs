using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState 
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;

    protected string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() //�������State
    {
        enemyBase.animator.SetBool(animBoolName,true);

        triggerCalled = false;

        enemyBase.FaceTarget(enemyBase.player.position);
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        
        
    }

    public virtual void Exit()//�����͡�ҡ�ʵ�
    {
        enemyBase.animator.SetBool(animBoolName,false);
    }
    public void AnimationTrigger() => triggerCalled = true;

    public virtual void AbilityTrigger()
    {

    }

    protected Vector3 GetNextPathPoint() //Debug�͹ai�Թ�ͧ���������¶Ѵ价�����ѹ�ͧ�ҧpath�����������͹
    {
        NavMeshAgent agent = enemyBase.agent;
        NavMeshPath path = agent.path;
        if (path.corners.Length < 2)
        {
            return agent.destination;
        }
        for (int i = 0; i < path.corners.Length; i++) //path.corners ���Query Nodes�͹ai�Թ�����������èЪ�����
        {
            if (Vector3.Distance(agent.transform.position, path.corners[i]) < 1)
            {
                return path.corners[i + 1];
            }
        }
        return agent.destination;
    }


}
