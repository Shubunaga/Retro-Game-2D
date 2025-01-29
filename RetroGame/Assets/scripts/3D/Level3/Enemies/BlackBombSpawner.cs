using Level3;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Level3
{
    public class BlackBombSpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;  // Reference to the enemy prefab
        public Transform[] pathPoints;  // Array of path points for enemy to follow
        private GameObject spawnedEnemy;  // Reference to the currently spawned enemy
        public float respawnDelay = 10f;

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
                if(pathPoints.Length > 8)
                    StartCoroutine(SpawnEnemyCoroutine());
            }
        }
        IEnumerator SpawnEnemyCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(respawnDelay);  // Wait for cooldown
                SpawnEnemy();
            }
        }
        private void Update()
        {
            if (spawnedEnemy == null || spawnedEnemy.GetComponent<BlackBomb>().isDead)
            {
                // Start respawn based on path points length
                if (pathPoints.Length < 8)
                {
                    StartCoroutine(RespawnEnemy());
                }
            }
        }
        IEnumerator RespawnEnemy()
        {
            // Wait for the respawn delay with a slight buffer
            yield return new WaitForSeconds(respawnDelay - 0.1f);
            // Ensure the enemy is still dead before spawning
            if (spawnedEnemy == null || spawnedEnemy.GetComponent<BlackBomb>().isDead)
            {
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()  // Function to spawn an enemy
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            BlackBomb enemyScript = enemy.GetComponent<BlackBomb>();  // Get Goomba script

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
