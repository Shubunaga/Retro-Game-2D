using Level2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level2
{
    public class BulletDamage : MonoBehaviour
    {
        public int damage = 20;
        private float lifetime = 3f;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                CharacterController enemyCharacterController = other.gameObject.GetComponent<CharacterController>();
                if (enemyCharacterController != null && other == enemyCharacterController)
                {
                    EnemyStats enemyStats = other.gameObject.GetComponent<EnemyStats>();
                    if (enemyStats != null)
                    {
                        enemyStats.GetDamage(damage);
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}