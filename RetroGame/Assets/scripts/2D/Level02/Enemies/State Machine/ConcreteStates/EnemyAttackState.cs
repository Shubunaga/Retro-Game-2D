using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private Transform _playerTransform;
    
    private float _timer;
    private float _timeBetweenSummons = 2f;

    private float _exitTimer;
    private float _timeTillExit = 2f;
    private float _distanceToCountExit = 2f;
    //private float _spellSpeed;

    public EnemyAttackState(EnemySecond enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.MoveEnemy(Vector2.zero);

        //Debug.Log("Valor do timer: " + _timer);
        //Debug.Log("timer de summons: " + _timeBetweenSummons);

        if (_timer > _timeBetweenSummons)
        {
            _timer = 0f;
            //Vector2 dir = (_playerTransform.position - enemy.transform.position).normalized;
            Debug.Log("hello from the summon update");
            enemy.anim.SetTrigger("SummonAttack");
            GameObject.Instantiate(enemy.phantomPrefab, enemy.summonPointA.transform.position, Quaternion.identity);
            GameObject.Instantiate(enemy.phantomPrefab, enemy.summonPointB.transform.position, Quaternion.identity);
            //GameObject.Instantiate(enemy.phantomPrefab, enemy.summonPointB.transform.position, Quaternion.identity);
            
            //Rigidbody2D spell = GameObject.Instantiate(spell.SpellPrefab, enemy.transform.position, Quaternion.identity);
            //spell.velocity = dir * _spellSpeed;
        }

        if(Vector2.Distance(_playerTransform.position, enemy.transform.position) > _distanceToCountExit)
        {
            _exitTimer += Time.deltaTime;

            if(_exitTimer > _timeTillExit)
            {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
        }

        else
        {
            _exitTimer = 0f;
        }

        _timer += Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
