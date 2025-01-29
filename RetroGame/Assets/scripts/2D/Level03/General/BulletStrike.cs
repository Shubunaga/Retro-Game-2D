using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStrike : MonoBehaviour
{
    [SerializeField] private int spDamage;
    [SerializeField] private float spLifeTime;
    [SerializeField] private float spDistance;

    private SpecialBarAttack spBar;

    public GameObject BoltHit;

    public LayerMask enemyLayer;

    private Animator bossAnim;
    void Start()
    {
        Invoke("SpDestroy", spLifeTime);
    }

    void SpDestroy(GameObject btHit)
    {
        if(btHit.CompareTag("Enemy") || btHit.gameObject.layer == 6)
        {

            BoltHit = Instantiate(BoltHit, gameObject.transform.position, Quaternion.identity);

            bossAnim = GameObject.Find("Enemy Test").GetComponentInChildren<Animator>();
            bossAnim.SetTrigger("SufferedDamage");

            btHit.GetComponent<Life>().TakeDamage(spDamage);

            Destroy(gameObject, 0.08f);
            Destroy(BoltHit, 0.3f);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        SpDestroy(collision.gameObject);

    }
}
