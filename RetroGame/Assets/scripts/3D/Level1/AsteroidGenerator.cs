using System.Collections;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    public GameObject asteroidPrefab; // Prefab do asteróide
    public float spawnAreaSize = 50f; // Tamanho da área de spawn
    public float minSize = 0.5f; // Tamanho mínimo do asteróide
    public float maxSize = 50f; // Tamanho máximo do asteróide
    public float pushForce = 10f; // Força de empurrão
    public float rotationSpeed = 1f; // Velocidade de rotação

    void Start()
    {
        StartCoroutine(SpawnAsteroids());
    }

    IEnumerator SpawnAsteroids()
    {
        while (true) // Loop infinito
        {
            // Gera uma posição aleatória dentro da área de spawn
            Vector3 spawnPosition = transform.position + new Vector3(
                Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2),
                Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2),
                Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2)
            );

            // Cria um novo asteróide na posição gerada
            GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

            // Gera um tamanho aleatório para o asteróide dentro dos limites especificados
            float size = Random.Range(minSize, maxSize);
            asteroid.transform.localScale = new Vector3(size, size, size);

            // Gera uma rotação aleatória para o asteróide
            Vector3 rotation = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ) * rotationSpeed; // Aplica a velocidade de rotação

            // Aplica a rotação ao asteróide
            Rigidbody asteroidRigidbody = asteroid.GetComponent<Rigidbody>();
            asteroidRigidbody.AddTorque(rotation);

            // Empurra o asteróide na direção -Z com uma força fixa
            Vector3 pushDirection = new Vector3(0, 0, -1);
            asteroidRigidbody.AddForce(pushDirection * pushForce);


            yield return new WaitForSeconds(1f); // Espera antes de gerar o próximo asteróide
        }
    }
}
