using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPhase03 : PhysicsObject
{
    public Animator anim;

    public float maxSpeed = 5;
    public float jumpForce = 10;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (move.x > 0) { anim.Play("run"); transform.eulerAngles = new Vector3(0f, 0f, 0f); } 
        else if (move.x < 0) { anim.Play("run"); transform.eulerAngles = new Vector3(0f, 180f, 0f); }
        //else if(move.x == 0) anim.Play("Idle");

        if(Input.GetKeyDown(KeyCode.W) && grounded)
        {
            velocity.y = jumpForce;
        }
        else if(Input.GetKeyUp(KeyCode.W))
        {
             if(velocity.y > 0) { }
            {
                velocity.y *= 0.5f;
            }
        }

        targetVelocity = move * maxSpeed;
    }
}
