using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : MonoBehaviour
{
    public GameObject explosionPrefab;
    public ParticleSystem explosionEffect; // O efeito da explosão

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") 
        { 
            //GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation) as GameObject;
            ParticleSystem explosion = Instantiate(explosionEffect, transform.position, transform.rotation);

            // Inicia o efeito da explosão
            explosion.Play();
            Destroy(gameObject);
            Destroy(explosion, 6f); 
        }
    }
}