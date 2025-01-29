using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] Asteroid asteroid;
    [SerializeField] int numberOfAsteroidsOnAnAxis = 10;
    [SerializeField] int grindSpacing = 100;

    void Start()
    {
        PlaceAsteroids();
    }
    void PlaceAsteroids()
    {
        for(int x = 0; x < numberOfAsteroidsOnAnAxis; x++)
        {
            for (int y = 0; y < numberOfAsteroidsOnAnAxis; y++)
            {
                for (int z = 0; z < numberOfAsteroidsOnAnAxis; z++)
                {
                    InstantiateAsteroid(x, y, z);
                }
            }
        }
    }

    void InstantiateAsteroid(int x, int y, int z)
    {
        Instantiate(asteroid,
        new Vector3(transform.position.x + (x * grindSpacing) + AsteroidOffSet(),
                    transform.position.y + (y * grindSpacing) + AsteroidOffSet(),
                    transform.position.z + (z * grindSpacing) + AsteroidOffSet()),
                    Quaternion.identity,
                    transform);
    }

    float AsteroidOffSet()
    {
        return Random.Range(-grindSpacing/2f, grindSpacing/2f);
    }
}
