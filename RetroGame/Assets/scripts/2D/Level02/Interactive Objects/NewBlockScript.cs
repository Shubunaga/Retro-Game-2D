using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBlockScript : MonoBehaviour
{
  public float moveSpeed = 5f;
  public Transform movePoint;

  public LayerMask whatStopsMovement;

  void Start() 
  {
    movePoint.parent = null;  
  }

  void Update() 
  {
    transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    // Check if the collision is with the player
    if(collision.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Pushable"))
    {
      // Get the direction of the push
      Vector3 direction = transform.position - collision.transform.position;

      // Move the object along the push direction
      if(!Physics2D.OverlapCircle(movePoint.position + direction, .2f, whatStopsMovement))
      {
        movePoint.position += direction;
      }
    }
  }
}
