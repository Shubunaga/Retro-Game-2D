using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Animations;
using UnityEngine;

public class AttackData : MonoBehaviour
{
    public AudioPlayer audioPlayer;

    private float damage;
    private bool slowDown;
    private AudioClip hitSound;

    //public LayerMask bossLayer;
    //public BoxCollider2D playerPunchesSize; //se não der assim, dar um jeito de pegar o raio do alcance
    //public Transform playerHitPoint; 

    //public Component attackArea;
    
    //public LayerMask bossLayer;
    //public Rigidbody2D bossRb;
    private Animator bossAnim;

    public void SetAttack(Hit hit)
    {
        damage = hit.damage;
        slowDown = hit.slowDown;
        hitSound = hit.hitSound;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collider2D[] hitBoss = Physics2D.OverlapBoxAll((Vector2)attackArea.transform.position,
        //    new Vector2(attackArea.transform.GetComponent<BoxCollider2D>().size.x, attackArea.transform.GetComponent<BoxCollider2D>().size.y),
        //                attackAngle, bossLayer);

        Life enemy = collision.GetComponent<Life>();


        if (enemy != null && collision.gameObject.CompareTag("Enemy"))
        {

            bossAnim = GameObject.Find("Enemy Test").GetComponentInChildren<Animator>();
            bossAnim.SetTrigger("SufferedDamage");

            //Vector2 repulsionDirection = (Vector2)bossRb.position - (Vector2)attackArea.transform.position;
            //repulsionDirection.Normalize();
            
            //repulsionDirection.x += 5f;
            //repulsionDirection.y += 2f;
            //Collider2D[] hitBoss= Physics2D.OverlapCircleAll(punchAttack.position, punchRange, playerLayer);
            enemy.TakeDamage(damage);
            audioPlayer.PlaySound(hitSound);

            ComboManager.instance.SetCombo();
            //bossRb.AddForce(repulsionDirection * 5, ForceMode2D.Force);


            if (slowDown)
                SlowDownEffect.instance.SetSlowDown();
        }
    }





    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Life enemy = collision.GetComponent<Life>();
    //    if (enemy != null && collision.gameObject.CompareTag("Enemy"))
    //    {
    //        Collider2D[] hitBoss = Physics2D.OverlapBox(playerHitPoint.GetComponent<BoxCollider2D>(), playerPunchesSize, playerPunchesSize.;  //(playerHitPoint.position, playerPunchesSize, bossLayer); 

    //        foreach (Collider2D bossColl in hitBoss)
    //        {
    //            Rigidbody2D bossRb = bossColl.GetComponent<Rigidbody2D>();
    //            if (bossRb != null)
    //            {
    //                Vector2 repulsionDirection = (Vector2)bossRb.position - (Vector2)playerHitPoint.position;
    //                repulsionDirection.Normalize();

    //                enemy.TakeDamage(damage);
    //                audioPlayer.PlaySound(hitSound); //botei na main camera porque no player ficava o ícone de áudio
    //                                                 //preciso descobrir o pq (descobri, é só pira do editor mesmo, in game não vai ter ela)            
    //                                                 //dano e corrotina
    //                bossRb.AddForce(repulsionDirection * 2f, ForceMode2D.Force);
    //            }
    //        }






    //        if (slowDown)
    //            SlowDownEffect.instance.SetSlowDown();

    //        ComboManager.instance.SetCombo();
    //    }

    //}

}
