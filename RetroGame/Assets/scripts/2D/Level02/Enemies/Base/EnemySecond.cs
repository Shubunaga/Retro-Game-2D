using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySecond : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 15f;
    public float CurrentHealth { get; set; }
    public Rigidbody2D rbody { get; set; }
    public bool IsFacingRight { get; set; } = true;
    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }

    public bool IsWithinPhysicalATKdistance { get; set; } //physical atk

    #region State Machine Variables
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; } //AQUI DOIDO, FALTA O DE PHYSICAL ATK
    public EnemyPhysicalATKState PhysicalATKState { get; set; }

    //public GameObject WinningScreen;
    #endregion

    #region Summoning Attack Variables

    public GameObject phantomPrefab;
    public Animator anim;

    public GameObject summonPointA;
    public GameObject summonPointB;
    public GameObject summonPointC;
    #endregion

    #region Idle Variables
    public float RandomMovementRange = 5f;
    public float RandomMovementSpeed = 2f;
    #endregion

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        PhysicalATKState = new EnemyPhysicalATKState(this, StateMachine);
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;

        rbody = GetComponent<Rigidbody2D> ();
        anim = GetComponentInChildren<Animator> ();
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();  
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Health/Die Functions
    public void Damage(float damageAmout)
    {
        CurrentHealth -= damageAmout;
        anim.SetTrigger("Damage");
        AudioController.current.PlayMusic(AudioController.current.bossHit);
        

        if (CurrentHealth <= 0) {
            anim.SetBool("isDead", true);
            AudioController.current.PlayMusic(AudioController.current.bossDeath);
            Die();
        }
    }

    public void Die()
    {
        //Destroy();
        Destroy(this.gameObject, 2f);
        Time.timeScale = 0f;
        //WinningScreen.SetActive(true);
        //EnvironmentController.instance.winningScreen.SetActive(true);
        SceneManager.LoadScene("VictoryLevel02", LoadSceneMode.Single);
        //EnvironmentController.instance.ShowWinningScreen();

        //tela de parabéns aqui e som do boss morrendo
    }

    #endregion

    #region Movement

    public void MoveEnemy(Vector2 velocity)
    {
        rbody.velocity = velocity;
        CheckForLeftOrRightFacing(velocity);
    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if (IsFacingRight && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }

        else if (!IsFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
    }
    #endregion

    #region Animation Triggers
    
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        //TODO: Fill once we have state machine - filled
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootsteps,
        //PhysicalAtk,
        //SummonAtk,
    }
    #endregion

    //private void OnDrawGizmosSelected()
    //{
        
    //}

    #region Distance checks
    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }

    public void SetStrikingDistanceBool(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance;
    }

    public void SetPhysicalATKdistanceBool(bool isWithinPhysicalATKdistance)
    {
        IsWithinPhysicalATKdistance = isWithinPhysicalATKdistance;
    }
    #endregion

    #region Collision detection

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("PlayerBullets"))
        {
            Damage(1f);
            Debug.Log("Vida do boss: " + CurrentHealth);
        }
    }

    #endregion
}
