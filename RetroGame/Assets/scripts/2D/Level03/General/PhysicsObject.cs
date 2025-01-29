using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class PhysicsObject : MonoBehaviour
{
    public float gravityModifier = 1;
    public float minGroundY = 0.5f;

    protected Rigidbody2D rbody;
    protected Vector2 velocity;
    protected Vector2 targetVelocity;

    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] raycastHit2Ds = new RaycastHit2D[10];
    protected bool grounded;
    protected Vector2 groundNormal;

    protected const float minMoveDistance = 0.001f;
    protected const float colRadius = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity() { }
    private void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 moveNormal = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveNormal * velocity.x * Time.deltaTime;
        Move(move, false);

        Vector2 yDirection = Vector2.up * velocity.y * Time.deltaTime;
        Move(yDirection, true);
        
    }

    void Move(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        if(distance > minMoveDistance) 
        {
            int count = rbody.Cast(move, contactFilter, raycastHit2Ds, distance + colRadius);
            for(int i = 0; i < count; i++) 
            {
                Vector2 currentNormal = raycastHit2Ds[i].normal;
                if(currentNormal.y > minGroundY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if(projection < 0)
                {
                    velocity -= projection * currentNormal;
                }

                float modifiedDistance = raycastHit2Ds[i].distance - colRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance; //if o valor modificadao da distancia é menor que ela, se for,
                                                                                      //ela vai receber o valor da modifiedDistance, senão permanece
                                                                                      //o mesmo valor
            }
        }
        
        rbody.position += move.normalized * distance;
    }
}
