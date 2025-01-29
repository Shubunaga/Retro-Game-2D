using Level2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject explosionAnimation; // arraste o prefab da anima��o de explos�o para este campo no inspetor
    public float explosionRadius = 6f; // Raio da explos�o
    public float explosionDamage = 100f; // Dano da explos�o
    public GameObject audioSourcePrefab;

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBullets")
        {
            GameObject audioObject = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();
            audioSource.Play();
            GameObject explosion = Instantiate(explosionAnimation, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
            // Destrua o objeto de �udio depois que o som terminar
            Destroy(audioObject, audioSource.clip.length);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Explode();
    }


    void Explode()
    {
        // Cria uma esfera ao redor do barril
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            // Verifica se o objeto atingido tem um componente CharacterController
            CharacterController characterController = hit.GetComponent<CharacterController>();

            if (characterController != null)
            {
                // Verifica se o objeto atingido tem um componente EnemyStats
                Level2.EnemyStats enemy = hit.GetComponent<Level2.EnemyStats>();

                if (enemy != null)
                {
                    // Se tiver, causa dano
                    enemy.GetDamage(explosionDamage);
                }
                Level2.PlayerBase player = hit.GetComponent<Level2.PlayerBase>();

                if (player != null)
                {
                    player.GetDamage(explosionDamage);
                }
            }
        }

        // Aqui voc� pode adicionar o c�digo para criar o efeito visual da explos�o
    }




    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
