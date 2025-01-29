using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Level2
{
    public class Enemy1 : EnemyStats
    {
        public GameObject bulletPrefab;
        public Transform firePoint1;
        public Transform firePoint2;
        private bool isAttacking = false;


        void Start()
        {
            life = 100f; // Defina a vida inicial aqui
        }


        public override void MoveTowardsPlayer(Transform target)
        {
            base.MoveTowardsPlayer(target);

            if (!playerIsInRange)
            {
                if (!isWalking)
                {
                    anim.SetBool("walk", true);
                    isWalking = true;
                }
            }
            else
            {
                if (isWalking)
                {
                    anim.SetBool("walk", false);
                    isWalking = false;
                }
            }
        }

        public override void Attack()
        {

            if (!isAttacking && playerIsInRange)
            {
                anim.SetBool("attack", true);
                Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
                // Instancie a bala na posição do segundo ponto de disparo
                Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
                isAttacking = true;
                fireTimer = 0; // Reinicie o temporizador de ataque
            }
            else if (isAttacking && fireTimer >= fireRate)
            {
                anim.SetBool("attack", false);
                isAttacking = false;
            }
        }
    }
}
