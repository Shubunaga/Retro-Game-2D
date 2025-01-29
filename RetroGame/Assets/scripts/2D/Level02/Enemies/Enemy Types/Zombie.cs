using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    #region Movimentação do jogador
     [SerializeField]
    private int velocity;

    [SerializeField]
    private float minimumDistance;
    
    [SerializeField]
    Rigidbody2D rig;

    [SerializeField]
    private SpriteRenderer spriteRen;

    //[SerializeField]
    //private Animator anim;

    #endregion Movimentação do jogador

    private Transform target;
    
    [SerializeField]
    private float visionRadius;

    [SerializeField]
    private LayerMask LayerVisionArea;

    protected override void Update() {
        SearchPlayer();
        if(target != null){
            Move();
        } else { //não há um alvo
            StopMoving();
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, visionRadius);   
    }

    private void SearchPlayer(){
        Collider2D colisor = Physics2D.OverlapCircle(transform.position, visionRadius, LayerVisionArea);
        if (colisor != null && colisor.transform.CompareTag("Player")){
            target = colisor.transform;
        } else {
            target = null;
        }
    }

    private void Move()
    {
        Vector2 targetPosition = target.position;
        Vector2 actualPosition = transform.position;

        float distance = Vector2.Distance(actualPosition, targetPosition);
        if(distance >= minimumDistance){
            // Movimento do inimigo
            Vector2 direction = targetPosition - actualPosition;
            direction = direction.normalized;

            rig.velocity = velocity * direction;

            if(rig.velocity.x > 0) { //direita
                spriteRen.flipX = false;
            } else if (rig.velocity.y < 0){ //esquerda
                spriteRen.flipX = true;
            }
            //this.animator.SetBool("moving", true); //lembrar de atribuir o inimigo no inspetor
        } else {
            StopMoving();
        }
    }

    private void StopMoving(){
        rig.velocity = Vector2.zero; //(0, 0)
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

}
