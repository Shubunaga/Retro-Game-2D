using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PattrolingEnemy : Enemy
{
   public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;

    //Gizmos movement
    public GameObject pointA, pointB, pointC, pointD;
    public Transform[] patrolPoints;
    public int targetPoint;
    private Transform currentPoint;
    private Rigidbody2D rig;
    
    public ShootingScript bullet;

    private void Update()
    {
        PatrollingBehavior();
    }


    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("PlayerBullets"))
        {
            //damage
            life -= 1;

            if(life <= 0)
            {
                AudioController.current.PlayMusic(AudioController.current.phantomDeath);
                Destroy(collision.gameObject);
                Destroy(this.gameObject, 0.05f);
            }
            
        }
    }

    void PatrollingBehavior() //LEMBRAR DE FAZER UMA CONDICIONAL PRA PATRULHA SER ATIVADA OU DESATIVADA NO UPDATE
    {
       if(transform.position == patrolPoints[targetPoint].position)
       {
            increaseTargetInt();
       }
       transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, moveSpeed * Time.deltaTime);
    }

    void increaseTargetInt(){
        targetPoint++;
        if(targetPoint >= patrolPoints.Length){
            targetPoint = 0;
        }
    }

//
    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.2f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.2f);
        Gizmos.DrawWireSphere(pointC.transform.position, 0.2f);
        Gizmos.DrawWireSphere(pointD.transform.position, 0.2f);

        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        Gizmos.DrawLine(pointB.transform.position, pointC.transform.position);
        Gizmos.DrawLine(pointC.transform.position, pointD.transform.position);
        Gizmos.DrawLine(pointD.transform.position, pointA.transform.position);
    }
}
