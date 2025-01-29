using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootForward : MonoBehaviour
{
    public float fireRate = 0.5f;
    public float laserSpeed = 10f;
    public GameObject laserPrefab;
    public Transform firePoint; // ponto de referência para os tiros

    public AudioSource laserSfx;
    private float nextFire = 0.0f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            FireLaser();
        }

    }
    void FireLaser()
    {
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation) as GameObject;
        laser.GetComponent<Rigidbody>().velocity = firePoint.forward * laserSpeed;
        laserSfx.Play();
    }
}
