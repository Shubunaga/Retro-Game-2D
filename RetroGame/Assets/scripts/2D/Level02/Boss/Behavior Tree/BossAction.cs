using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Rendering;
using UnityEngine;

public class BossAction : Enemy
{
    [SerializeField] float lineOfSight;
    [SerializeField] float summoningATKRange;
    [SerializeField] float physicalATKRange;
    private Rigidbody2D rbody;
    private bool summoning;
    public Transform atkPoint;
    [SerializeField] float physicalAtKRadious;

    public LayerMask enemyLayer;

    public float summonRate = 1f;
    //private float nextSummon = 4f; 
    public Transform sPointA, sPointB, sPointC;

    public GameObject phantoms;//instantiate in the summoning attack
    public Transform player;
    public Animator anim;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rbody = GetComponent<Rigidbody2D> ();
    }

    //update novo
    protected override void Update()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        if(Vector3.Distance(player.position, 
                            transform.position) <= lineOfSight
            && Vector3.Distance(player.position, 
                                transform.position) > summoningATKRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            //anim.SetFloat("moveSpeed", moveSpeed);
            anim.SetBool("Running", true);
        }

        if(Vector2.Distance(player.position, transform.position) <= summoningATKRange && summoning == false)
        {
            //StartCoroutine(SummonAtkCO());
            InvokeRepeating("SummonAtk", 0f,5f);
            CheckDistance();//tentativa da misera
        }

        
    }


    //private IEnumerator SummonAtkCO()
    private void SummonAtk()
    {
        rbody.velocity = Vector2.zero;
        anim.SetTrigger("SummonAttack");
        //anim.SetBool("Running", false);
        //anim.SetFloat("moveSpeed", 0);

        if(!summoning)
        {
            AudioController.current.PlayMusic(AudioController.current.bossAttack);
            Instantiate(phantoms, sPointA.position, Quaternion.identity);
            Instantiate(phantoms, sPointB.position, Quaternion.identity);
            Instantiate(phantoms, sPointC.position, Quaternion.identity);
            summoning = true;
        }
        
        //yield return new WaitForSeconds(3f);
        //summoning = false;
    }
    
    private void PhysicalAtK()
    {
        rbody.velocity = Vector2.zero;
        anim.SetTrigger("PhysicalAttack");
        Physics2D.OverlapCircleAll(atkPoint.position, physicalAtKRadious, enemyLayer);

    }

    //--------------------GIZMOS-----------------//
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireSphere(transform.position, summoningATKRange);
        Gizmos.DrawWireSphere(transform.position, physicalATKRange);
    }
    
    private void takeDMG()
    {
        life -= 1;
        AudioController.current.PlayMusic(AudioController.current.bossHit);
    }
}

 // Update antigo
    // void Update()
    // {
    //     float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
    //     if(distanceFromPlayer < lineOfSight && distanceFromPlayer > summoningATKRange)
    //     {
    //         transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            
    //     }else if(distanceFromPlayer <= summoningATKRange && nextSummon < Time.deltaTime)
    //     {
    //         transform.position = Vector2.zero;
    //         Instantiate(phantoms, sPointA.position, Quaternion.identity);
    //         Instantiate(phantoms, sPointB.position, Quaternion.identity);
    //         Instantiate(phantoms, sPointC.position, Quaternion.identity);
    //         nextSummon = Time.deltaTime + summonRate;
    //     }
        
    // }
