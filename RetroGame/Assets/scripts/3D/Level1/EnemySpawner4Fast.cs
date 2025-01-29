using RailShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner4Fast : MonoBehaviour
{
    [SerializeField] Square[] spawn;
    [SerializeField] EnemyBase enemyPrefab;
    [SerializeField] float spawnInterval = 5f;

    [SerializeField] Transform enemyParent;
    [SerializeField] Transform flightPathParent;

    float spawnTimer;

    void Update()
    {
        if (spawnTimer > spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemy();
        }
        spawnTimer += Time.deltaTime;
    }

    void SpawnEnemy()

    {
        // Gera um número aleatório de caminhos e inimigos entre 3 e 7
        int numPaths = Random.Range(3, 8);

        for (int i = 0; i < numPaths; i++)
        {
            var flightPaths = FlightPathFactory.GenerateFlightPath(spawn);
            EnemyFactory.GenerateEnemyBase(enemyPrefab, flightPaths, enemyParent, flightPathParent);
        }

    }
}
