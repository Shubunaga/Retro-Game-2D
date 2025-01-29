using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class MechanismActivation : MonoBehaviour
{
    public GameObject stoneBlock;
    public int mechanismCount;
    public PlayerMovement playerMovement;

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Pushable")){
            
            collision.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
            collision.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
            
            playerMovement.MechanismActivation();
            collision.gameObject.tag = "Activated";
            collision.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
        }
        //Debug.Log(mechanismCount);
    }
}
