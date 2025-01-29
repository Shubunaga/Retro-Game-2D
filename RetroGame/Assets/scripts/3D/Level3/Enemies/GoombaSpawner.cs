using Level3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class GoombaSpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;  // Reference to the enemy prefab
        public Transform[] pathPoints;  // Array of path points for enemy to follow
        public float respawnDelay = 10f;  // Time to wait before respawning (in seconds)

        private GameObject spawnedEnemy;  // Reference to the currently spawned enemy


        void Start()
        {
            // Check if enemy prefab and path points are assigned
            if (enemyPrefab == null || pathPoints.Length == 0)
            {
                Debug.LogError("Please assign enemy prefab and path points in inspector!");
                return;
            }
            else
            {
                SpawnEnemy();
            }
        }

        void Update()
        {
            // Check if enemy is dead and there's no currently spawned enemy
            if (spawnedEnemy == null || spawnedEnemy.GetComponent<Goomba>().die)
            {
                // Start a coroutine to handle respawn with delay
                StartCoroutine(RespawnEnemy());
            }
        }

        IEnumerator RespawnEnemy()
        {
            // Wait for the respawn delay with a slight buffer
            yield return new WaitForSeconds(respawnDelay - 0.1f);

            // Ensure the enemy is still dead before spawning
            if (spawnedEnemy == null || spawnedEnemy.GetComponent<Goomba>().die)
            {
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()  // Function to spawn an enemy
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            Goomba enemyScript = enemy.GetComponent<Goomba>();  // Get Goomba script

            if (enemyScript != null)
            {
                enemyScript.pathPoints.Clear();  // Clear existing path points in enemy script
                enemyScript.pathPoints.AddRange(pathPoints);  // Add current path points to enemy script
                spawnedEnemy = enemy;  // Keep track of the spawned enemy
            }
            else
            {
                Debug.LogError("Enemy prefab does not have Goomba script!");
            }
        }
    } 
}
