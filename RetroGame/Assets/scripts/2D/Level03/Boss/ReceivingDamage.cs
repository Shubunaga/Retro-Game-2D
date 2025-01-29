using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline.Actions;
using UnityEngine;

public class ReceivingDamage : StateMachineBehaviour
{
    private PlayerWalk playerDirection;

    public float repulsionY = 0.9f;
    public float repulsionX = -4f;
    public float repulsionForce = 200f;

    private float StunTimer = 1.5f;

    Transform player;
    Rigidbody2D rb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //entra na animação de apanhar e pega os dados necessários
        
        ///////////////////////
        ///
        //player = GameObject.Find("Player").transform;
        rb = animator.GetComponentInParent<Rigidbody2D>();

        //Vector2 repulsionDirection = (Vector2)rb.position - (Vector2)player.position;
        //repulsionDirection.Normalize();

        //repulsionDirection.x += repulsionX;
        //repulsionDirection.y += repulsionY;

        //rb.AddForce(-repulsionDirection * repulsionForce, ForceMode2D.Force);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        GettingHit();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //saindo da animação de apanhar
        animator.ResetTrigger("SufferedDamage");
    }

    void GettingHit() //chamado por mensagem lá no AttackData
    {        
        //StunTimer += 1.5f;

        for (float i = 0; i < StunTimer; StunTimer += Time.deltaTime)
        {
            rb.velocity = Vector2.zero;

            

            if (StunTimer > 1.5f)
            {
                return;
            }
        }


    }
    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
