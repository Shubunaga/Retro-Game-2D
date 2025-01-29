using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
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

    //Getting Hit variables
    private float StunTimer;
    private bool isGettingHit;
    public Animator bossAnim;

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindWithTag("Player").transform;
        rb = GameObject.Find("Enemy Test").GetComponent<Rigidbody2D>();
        bossAnim = GameObject.Find("Enemy Test").GetComponentInChildren<Animator>();
        bossSpecialBar = GameObject.Find("Enemy Test").GetComponent<Life>();

        randomSpeed = Random.Range(1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGettingHit == false)
        { //preciso de um get bool aqui do attack data

            bossAnim.SetBool("isIddle", false); //andando

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
                    randomBehavior = Random.Range(1, 5);

                    //DEFINITIVO
                    //randomBehavior = Random.Range(1, 6);

                    lastState = randomBehavior; //usado para mudança de velocidade

                    switch (randomBehavior)
                    {
                        case 0:
                            //dash pra trás
                            rbOriginalGravity = rb.gravityScale;

                            currentTimeDash = timeDash;
                            bossAnim.SetTrigger("bossBackdash");
                            timeDash -= Time.deltaTime;

                            if (timeDash <= 0)
                            {
                                //bossAnim.SetBool("Backdash", false);
                                Debug.Log("finalizou");
                                rb.gravityScale = rbOriginalGravity;
                            }

                            break;
                        case 01:
                            bossAnim.SetTrigger("attack1");
                            new WaitForSeconds(1);
                            break;
                        case 02:
                            bossAnim.SetTrigger("attack2");  //colocar set trigger pra animação do ataque 2 com um código pra ele lá no bossCombat
                            new WaitForSeconds(1);
                            break;
                        case 03:
                            bossAnim.SetTrigger("bossCombo");
                            new WaitForSeconds(1);
                            break;
                        case 04: //está funcionando mas lembre-se que precisa da barrinha dele estar cheia!
                            if (bossSpecialBar.special >= 100)
                            {
                                Debug.Log("Boss Soltou seu special");
                                bossSpecialBar.special = 0f;
                                //animator.SetTrigger("bossSpecialAttack");
                                new WaitForSeconds(4);
                            }
                            break;
                        case 05:
                            bossAnim.SetTrigger("bossWalkBack");
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

            bossAnim.ResetTrigger("attack1");
            bossAnim.ResetTrigger("attack2");
            bossAnim.ResetTrigger("bossCombo");
            bossAnim.ResetTrigger("bossWalkBack");
            bossAnim.ResetTrigger("bossBackdash");
            bossAnim.ResetTrigger("bossSpecialAttack");

        }
        else if (isGettingHit == true)
        {
            Debug.Log("entramos no true");
            bossAnim.SetBool("isIddle", true);
        }

    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("PlayerAttack"))
        {
            //função de ficar parado
            GettingHit();
        }
    }

    void GettingHit()
    {
        isGettingHit = true;
        //StunTimer += 1.5f;

        if (StunTimer < 1.5f)
        {
            StunTimer += Time.deltaTime;
            rb.velocity = Vector2.zero;
        }
        if (StunTimer > 1.5f)
        {
            isGettingHit = false;
            return;
        }

    }
}
