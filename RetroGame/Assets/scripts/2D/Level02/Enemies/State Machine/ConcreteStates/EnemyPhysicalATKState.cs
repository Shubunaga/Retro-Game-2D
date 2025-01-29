using Schema.Builtin.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhysicalATKState : EnemyState
{
    private Transform _playerTransform;
    //[SerializeField] private GameObject spellCollisor;

    private float _timer;
    private float _timeBetweenATKs = 1f;

    private float _exitTimer;
    private float _timeTillExit = 0.6f;
    private float _distanceToCountExit = 0.03f;

    public EnemyPhysicalATKState(EnemySecond enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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
        Debug.Log("Valor do timer: " + _timer);
        Debug.Log("timer de summons: " + _timeBetweenATKs);

        //spellCollisor.SetActive(false);//tentar pegar o colisor das magias e desativar ele
        //enemy.anim.ResetTrigger("SummonAttack");

        if (_timer > _timeBetweenATKs)
        {
            _timer = 0f;

            Debug.Log("Hello from the physical atk");
            //Vector2 dir = (_playerTransform.position - enemy.transform.position).normalized;

            enemy.anim.SetTrigger("PhysicalAttack");
            
            //como vou fazer ele dar dano? overlap circle?
            //ps,~não tá entrando aqui, nem o debug log é mostrado
        }

        if (Vector2.Distance(_playerTransform.position, enemy.transform.position) > _distanceToCountExit)
        {
            _exitTimer += Time.deltaTime;

            if (_exitTimer > _timeTillExit)
            {
                Debug.Log("entramos aqui nesta merda");
                enemy.StateMachine.ChangeState(enemy.ChaseState);
                //spellCollisor.SetActive(true);
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
