using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    //public Sprite profile;
    public string[] speechText;
    public string actorName;
    
    public LayerMask playerLayer;
    public float radious;

    private DialogControl dc;
    bool onRadious;

    private void Start() 
    {
        dc = FindObjectOfType<DialogControl>();
    }

    private void FixedUpdate() {
        Interact();
    }

    private void Update() 
    {
        if(Input.GetButtonDown("interact") && onRadious) //cada vez que aperto F ele reseta
        {
            dc.Speech(speechText, actorName);//, profile);

            // if(dc.gameObject.activeInHierarchy && Input.GetButtonDown("interact")){
            //     dc.gameObject.SetActive(false);
            // }            
        }
    }

    public void Interact()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radious, playerLayer);

        if(hit != null)// && Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("na área");
            onRadious = true;
        }
        else
        {
            onRadious = false;
        }
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(transform.position, radious);
    }
}
