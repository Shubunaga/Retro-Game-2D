using System.Collections;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    public GameObject asteroidPrefab; // Prefab do aster�ide
    public float spawnAreaSize = 50f; // Tamanho da �rea de spawn
    public float minSize = 0.5f; // Tamanho m�nimo do aster�ide
    public float maxSize = 50f; // Tamanho m�ximo do aster�ide
    public float pushForce = 10f; // For�a de empurr�o
    public float rotationSpeed = 1f; // Velocidade de rota��o

    void Start()
    {
        StartCoroutine(SpawnAsteroids());
    }

    IEnumerator SpawnAsteroids()
    {
        while (true) // Loop infinito
        {
            // Gera uma posi��o aleat�ria dentro da �rea de spawn
            Vector3 spawnPosition = transform.position + new Vector3(
                Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2),
                Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2),
                Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2)
            );

            // Cria um novo aster�ide na posi��o gerada
            GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

            // Gera um tamanho aleat�rio para o aster�ide dentro dos limites especificados
            float size = Random.Range(minSize, maxSize);
            asteroid.transform.localScale = new Vector3(size, size, size);

            // Gera uma rota��o aleat�ria para o aster�ide
            Vector3 rotation = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ) * rotationSpeed; // Aplica a velocidade de rota��o

            // Aplica a rota��o ao aster�ide
            Rigidbody asteroidRigidbody = asteroid.GetComponent<Rigidbody>();
            asteroidRigidbody.AddTorque(rotation);

            // Empurra o aster�ide na dire��o -Z com uma for�a fixa
            Vector3 pushDirection = new Vector3(0, 0, -1);
            asteroidRigidbody.AddForce(pushDirection * pushForce);


            yield return new WaitForSeconds(1f); // Espera antes de gerar o pr�ximo aster�ide
        }
    }
}
