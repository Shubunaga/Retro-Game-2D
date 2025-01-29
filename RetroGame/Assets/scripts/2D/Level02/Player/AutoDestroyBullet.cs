using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyBullet : MonoBehaviour
{
    public GameObject BoltHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if(collision.gameObject.layer == 15 || collision.gameObject.CompareTag("Enemy")){
            BoltHit = Instantiate(BoltHit, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject, 0.05f);
            Destroy(BoltHit, 0.3f);
        }
    }
}
