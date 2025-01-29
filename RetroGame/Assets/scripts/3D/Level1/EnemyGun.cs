using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [SerializeField] GameObject laserPrefab;
    [SerializeField]
    float laserSpeed = 300;
    float laserInterval = 3f;
    private float laserTimer = 0f;
    private bool first;
    public AudioSource laserSfx;
    // Start is called before the first frame update
    void Start()
    {
        first = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Atualizar o contador de tempo
        laserTimer += Time.deltaTime;
        if (first==true && laserTimer>=0.5f)
         {
             BaseGun();
             first= false;
         }
        // Verificar se é hora de atirar
        if (laserTimer >= laserInterval)
        {
            // Resetar o contador de tempo
            laserTimer = 0f;

            // Atirar a bala
            BaseGun();
        }
    }
    public int damage = 20;

    /*void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Passou enemy");
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }*/
    void BaseGun()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, transform.rotation) as GameObject;
        laser.GetComponent<Rigidbody>().velocity = transform.forward * laserSpeed;
        laserSfx.Play();
    }
}
