using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace RailShooter
{
    public class EnemySpawner2Ufo : MonoBehaviour
    {
        [SerializeField] Square[] spawn;
        [SerializeField] EnemyBase enemyPrefab;
        [SerializeField] float minSpawnInterval = 3f;
        [SerializeField] float maxSpawnInterval = 8f;

        [SerializeField] Transform enemyParent;
        [SerializeField] Transform flightPathParent;

        float spawnTimer;
        float spawnInterval;

        void Start()
        {
            spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        }

        void Update()
        {
            if (spawnTimer > spawnInterval)
            {
                spawnTimer = 0f;
                spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
                SpawnEnemy();
            }
            spawnTimer += Time.deltaTime;
        }

        void SpawnEnemy()
        {
            var flightPaths = FlightPathFactory.GenerateFlightPath(spawn);
            EnemyFactory.GenerateEnemyBase(enemyPrefab, flightPaths, enemyParent, flightPathParent);
        }
    }
}
