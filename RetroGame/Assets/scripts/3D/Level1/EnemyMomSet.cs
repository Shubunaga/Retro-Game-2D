using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RailShooter
{
    public class EnemyMomSet : MonoBehaviour
    {
        public GameObject enemyChildPrefab; // prefab do inimigo filho
        public int numberOfChildren = 10; // número de filhos a serem gerados
        public float spawnRadius = 5f; // raio em que os filhos serão gerados em torno da nave mãe

        public EnemyHealthN13D enemyHealth;

        void OnDestroy()
        {
            // verifica se a nave mãe foi destruída pelo jogador
            if (enemyHealth.destroyedByPlayer)
            {
                for (int i = 0; i < numberOfChildren; i++)
                {
                    // gera uma posição aleatória dentro do raio especificado
                    Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;

                    // instancia o inimigo filho na posição gerada e passa o transform do jogador como um parâmetro adicional
                    GameObject enemyChild = Instantiate(enemyChildPrefab, spawnPosition, Quaternion.identity);
                    enemyChild.GetComponent<EnemyChildSet>().playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                }
            }
        }
    }
}
