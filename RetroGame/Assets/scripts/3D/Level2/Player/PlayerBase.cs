using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level2
{
    public class PlayerBase : MonoBehaviour
    {
        public Vector3 respawnPoint;
        public Animator animator;
        private float fireTimer;
        private float fireRate = 0.5f;
        public GameObject muzzleFlashPrefab;
        public GameObject bulletPrefab;
        public Transform firePoint;
        public Transform gunPoint;
        public int bulletSpeed = 50;
        public float life = 100f;
        public float shield = 0;
        public float ammoCount = 10f;
        public bool pistol;
        public bool key = false;
        public bool getDamage = false;
        public LayerMask groundLayer; // Adicione uma variável para a LayerMask
        public AudioSource tiro;
        private UILevel2 menu;


        private void Start()
        {
            pistol=false;
            menu = GameObject.FindObjectOfType<UILevel2>();
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightControl) && pistol==true)
            {
                Shot();
            }
            if(fireTimer < fireRate)
            {
                fireTimer += Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                OpenDoor();
            }
            Debug.DrawRay(transform.position, transform.forward * 100, Color.red);
        }

        public void EnablePistol()
        {
            pistol = true;
            animator.SetBool("Pistol", pistol);
        }

        public void GetDamage(float damage, bool enemy = false)
        {
            if (!enemy)
            {
                getDamage= true;
                life -= damage;
                life = Mathf.Round(life * 100f) / 100f;  // Arredonda a vida para ter no máximo 2 dígitos após o ponto decimal
                if (life <= 0)
                {
                    IsDead();
                }
            }
            else
            {
                getDamage= true;
                if (shield > 0)
                {
                    shield -= damage;
                    shield = Mathf.Round(shield * 100f) / 100f;  // Arredonda o escudo para ter no máximo 2 dígitos após o ponto decimal
                    if (shield < 0)
                    {
                        life += shield; // Adiciona o valor negativo do escudo à vida
                        life = Mathf.Round(life * 100f) / 100f;  // Arredonda a vida para ter no máximo 2 dígitos após o ponto decimal
                        shield = 0; // Redefine o escudo para 0
                    }
                }
                else
                {
                    life -= damage;
                    life = Mathf.Round(life * 100f) / 100f;  // Arredonda a vida para ter no máximo 2 dígitos após o ponto decimal
                    if (life <= 0)
                    {
                        IsDead();
                    }
                }
            }
        }

        public void AddLife(float bonusLife)
        {
            life += bonusLife;

            if (life > 100)
            {
                life = 100;
            }
        }
        public void AddShield(float bonusShield)
        {
            shield += bonusShield;

            if (shield > 50)
            {
                shield = 50;
            }
        }
        public void AddAmmo(float bonusAmmo)
        {
            ammoCount += bonusAmmo;
        }

        void IsDead()
        {
            menu.PlayerDied();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Gun")
            {
                EnablePistol();
            }else if(other.gameObject.tag == "Key")
            {
                key = true;
            }
        }

        void Shot()
        {
            if(fireTimer < fireRate)
            {
                return;
            }
            if (ammoCount > 0)
            {
                animator.CrossFadeInFixedTime("PlayerShot", 0.01f);
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

                GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, gunPoint.position, gunPoint.rotation);
                muzzleFlash.transform.SetParent(gunPoint.transform);
                Destroy(muzzleFlash, 0.05f);

                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    tiro.Play();
                    rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);
                }
                ammoCount--;
            }
            fireTimer= 0f;
        }

        public void OpenDoor()
        {
            RaycastHit hit;
            // Use Physics.Raycast com a LayerMask
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, groundLayer) && key)
            {
                DoorController door = hit.transform.GetComponent<Level2.DoorController>();
                if (door != null)
                {
                    door.Open(); // Chame o método Open na porta
                    key = false; // Defina a chave como false
                }
            }
        }
    }
}
