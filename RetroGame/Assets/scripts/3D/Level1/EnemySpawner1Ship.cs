using System.ComponentModel;
using UnityEngine;
using UnityEngine.Splines;

namespace RailShooter
{
    public class EnemySpawner1Ship : MonoBehaviour
    {
        [SerializeField] Annulus[] annuli;
        [SerializeField] Enemy enemyPrefab;
        [SerializeField] float spawnInterval = 5f;
        [SerializeField] SplineContainer[] paths;

        [SerializeField] Transform enemyParent;
        [SerializeField] Transform flightPathParent;
        private bool first;

        float spawnTimer;
        void Start()
        {
            first = true;
        }

        void Update()
        {
            if (first==true && spawnTimer > 5f)
            {
                SpawnEnemy();
                first= false;
                spawnTimer= 0f;
            }
            if (spawnTimer > spawnInterval)
            {
                spawnTimer = 0f;
                SpawnEnemy();
            }
            spawnTimer += Time.deltaTime;
        }

        void SpawnEnemy()

        {
            //var flightPaths = paths; //FlightPathFactory.GenerateFlightPath(annuli);
                                     //EnemyFactory.GenerateEnemy(enemyPrefab, flightPaths, enemyParent, flightPathParent);
            foreach (var flightPath in paths)
            {
                EnemyFactory.GenerateEnemy(enemyPrefab, flightPath, enemyParent, flightPathParent);
            }
        }
    }
}