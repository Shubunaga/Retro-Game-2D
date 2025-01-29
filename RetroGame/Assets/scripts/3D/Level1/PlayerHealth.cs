using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject explosionPrefab; // Refer�ncia para o prefab da explos�o
    private CameraShake cameraShake;
    private PlayButtonScript menus;
    public AudioSource explosionSfx;

    void Start()
    {
        currentHealth = maxHealth;
        cameraShake = GameObject.FindObjectOfType<CameraShake>();
        menus = GameObject.FindObjectOfType<PlayButtonScript>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        cameraShake.TriggerShake();

        if (currentHealth <= 0)
        {
            Die();
            menus.PlayerDied();
        }
    }

    private void Die()
    {
        // Instancie a explos�o na posi��o do jogador
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionSfx.clip, transform.position);
        // Destrua o objeto do jogador
        Destroy(gameObject);

        // Destrua a explos�o ap�s 2 segundos
        Destroy(explosion, 2f);
    }
}
