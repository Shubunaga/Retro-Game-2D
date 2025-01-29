using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter(Collider other)
    {
        EnemyHealthN13D enemyHealth = other.GetComponent<EnemyHealthN13D>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}