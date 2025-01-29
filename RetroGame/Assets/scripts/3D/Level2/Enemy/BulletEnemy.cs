using Level2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level2
{
    public class BulletEnemy : MonoBehaviour
    {
        public float speed = 20f; // Velocidade do proj�til
        public int damage = 20;
        public Rigidbody rb; // Rigidbody do proj�til

        void Start()
        {
            // Obtenha a dire��o para o jogador
            GameObject player = GameObject.FindWithTag("Player");
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;

            // Aplique uma for�a na dire��o do jogador
            rb.AddForce(dirToPlayer * speed, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerBase player = other.gameObject.GetComponent<PlayerBase>();
                if (player != null)
                {
                    player.GetDamage(damage, true);
                    GameObject.Destroy(gameObject);
                }
            }
            else if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}