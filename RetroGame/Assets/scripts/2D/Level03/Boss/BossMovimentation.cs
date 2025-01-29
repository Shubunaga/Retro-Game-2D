using Schema.Builtin.Nodes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BossMovimentation : StateMachineBehaviour
{
    public float speed = 2f;
    public float attackRange = 3f;
    public int randomBehavior;
    public int randomSpeed;
    [SerializeField] float timeDash;// = 0.4f;
    private float currentTimeDash;
    private float rbOriginalGravity;

    private float timeToAction = 3.5f;
    private float tempoPassado = 0f;
    
    int lastState;
    //private BossCombat bossCombat;
    private Life bossSpecialBar;

    Transform player;
    Rigidbody2D rb;

    //Getting beat variables
    private bool isGettingHit;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = animator.GetComponentInParent<Rigidbody2D>();
        bossSpecialBar = GameObject.Find("Enemy Test").GetComponent<Life>();

        randomSpeed = Random.Range(1, 3);
        if(randomSpeed == 1) { speed = 2f; } else { speed = 3f; }
        //bossCombat = animator.GetComponentInParent<BossCombat>();
        //animator.SetBool("isIddle", false);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(isGettingHit == false) { 
                                    
            animator.SetBool("isIddle", false); //andando

            tempoPassado += Time.fixedDeltaTime;
            //regulagem de velocidade
            if (tempoPassado > 2.5f) { speed = 2f; }

            //Back walking behavior
            if (lastState == 5)
            {
                Vector2 target = new Vector2(player.position.x, rb.position.y);
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, -speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);

                tempoPassado += Time.fixedDeltaTime;
                if (tempoPassado >= timeToAction)
                {
                    newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
                    rb.MovePosition(newPos);
                }
            }
            else
            {
                Vector2 target = new Vector2(player.position.x, rb.position.y);
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }



            if (Vector2.Distance(player.position, rb.position) <= attackRange)// && i >= timeToAction)
            {
                //tempoPassado += Time.fixedDeltaTime;
                if (tempoPassado > timeToAction)
                {
                    //if(tempoPassado > 1.5f) { speed = 2f; }
                    //Testes
                    //randomBehavior = Random.Range(4, 5);

                    //DEFINITIVO
                    randomBehavior = Random.Range(1, 6);

                    lastState = randomBehavior; //usado para mudança de velocidade

                    switch (randomBehavior)
                    {
                        case 0:
                            //dash pra trás
                            rbOriginalGravity = rb.gravityScale;

                            currentTimeDash = timeDash;
                            animator.SetTrigger("bossBackdash");
                            timeDash -= Time.deltaTime;

                            if (timeDash <= 0)
                            {
                                //bossAnim.SetBool("Backdash", false);
                                Debug.Log("finalizou");
                                rb.gravityScale = rbOriginalGravity;
                            }

                            break;
                        case 01:
                            animator.SetTrigger("attack1");
                            new WaitForSeconds(1);
                            break;
                        case 02:
                            animator.SetTrigger("attack2");  //colocar set trigger pra animação do ataque 2 com um código pra ele lá no bossCombat
                            new WaitForSeconds(1);
                            break;
                        case 03:
                            if (bossSpecialBar.special >= 100)
                            {
                                animator.SetTrigger("bossCombo2");
                                bossSpecialBar.special = 0;
                                break;
                            }
                            else
                            {
                                animator.SetTrigger("bossCombo");
                                new WaitForSeconds(1);
                            }
                            break;
                        case 04: //está funcionando mas lembre-se que precisa da barrinha dele estar cheia!
                            if (bossSpecialBar.special >= 100)
                            {
                                animator.SetTrigger("bossSpecialAttack");
                                bossSpecialBar.special = 0;
                                //animator.SetTrigger("bossSpecialAttack");
                                //new WaitForSeconds(4);
                            }
                            break;
                        case 05:
                            animator.SetTrigger("bossWalkBack");
                            tempoPassado = 0;
                            break;
                    }
                    //if (randomBehavior == 5)
                    //{
                    //    rb.MovePosition(-newPos);

                    //    timeToAction += 2f;
                    //    tempoPassado = 0;
                    //}
                    //else tempoPassado = 0f;
                    tempoPassado = 0;

                }
                //bossCombat.bossPunch1();//tá acertando múltiplas vezes por estar no método UPDATE, preciso fazer ele acertar só uma vez
            }
        } else if(isGettingHit == true)
        {
            Debug.Log("entramos no true");
            animator.SetBool("isIddle", true);

            if (isGettingHit == false) return;
    
        }

    }

    // if(Input.GetKeyDown(KeyCode.M)) { animator.SetBool("isIddle", true); } 


    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("attack1");
        animator.ResetTrigger("attack2");
        animator.ResetTrigger("bossCombo");
        animator.ResetTrigger("bossWalkBack");
        animator.ResetTrigger("bossBackdash");
        animator.ResetTrigger("bossSpecialAttack");
    }

    //void OnTriggerEnter2D(Collider2D coll)
    //{
        

    //    if (coll.gameObject.CompareTag("PlayerAttack"))
    //    {
    //        //função de ficar parado
    //        Debug.Log("mizera");
    //        GettingHit();
    //    }


    //}

    //void GettingHit() //chamado por mensagem lá no AttackData
    //{
    //    Debug.Log("mizzera");
    //    //StunTimer += 1.5f;

    //    for(float i = 0; i < StunTimer; StunTimer += Time.deltaTime)
    //    {
    //        isGettingHit = true;
    //        rb.velocity = Vector2.zero;

    //        tempoPassado = 0;
            
    //        if (StunTimer > 1.5f)
    //        {
    //            isGettingHit = false;
    //            return;
    //        }
    //    }
        
        
    //}

    //NÃO USAR ON COLLISION EXIT PQ VAI RESETAR A BOOL ANTES DO RECUO DO GOLPE ACABAR, preciso resetar o contador depois do stun
}
