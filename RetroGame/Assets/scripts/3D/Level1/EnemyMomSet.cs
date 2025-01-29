using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RailShooter
{
    public class EnemyMomSet : MonoBehaviour
    {
        public GameObject enemyChildPrefab; // prefab do inimigo filho
        public int numberOfChildren = 10; // n�mero de filhos a serem gerados
        public float spawnRadius = 5f; // raio em que os filhos ser�o gerados em torno da nave m�e

        public EnemyHealthN13D enemyHealth;

        void OnDestroy()
        {
            // verifica se a nave m�e foi destru�da pelo jogador
            if (enemyHealth.destroyedByPlayer)
            {
                for (int i = 0; i < numberOfChildren; i++)
                {
                    // gera uma posi��o aleat�ria dentro do raio especificado
                    Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;

                    // instancia o inimigo filho na posi��o gerada e passa o transform do jogador como um par�metro adicional
                    GameObject enemyChild = Instantiate(enemyChildPrefab, spawnPosition, Quaternion.identity);
                    enemyChild.GetComponent<EnemyChildSet>().playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                }
            }
        }
    }
}
