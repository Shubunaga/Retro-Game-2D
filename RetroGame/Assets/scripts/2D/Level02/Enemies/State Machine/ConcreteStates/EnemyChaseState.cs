using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Transform _playerTransform;
    private float _movementSpeed = 1.75f;
    public EnemyChaseState(EnemySecond enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void AnimationTriggerEvent(EnemySecond.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.anim.SetBool("Running", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        //enemy.MoveEnemy(Vector2.zero); usado para teste
        enemy.anim.SetBool("Running", true);
        Vector2 moveDirection = (_playerTransform.position - enemy.transform.position).normalized;
        enemy.MoveEnemy(moveDirection * _movementSpeed);

        if(enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
        else if(enemy.IsWithinPhysicalATKdistance) 
        {
            enemy.StateMachine.ChangeState(enemy.PhysicalATKState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
