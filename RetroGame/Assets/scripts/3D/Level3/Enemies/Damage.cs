using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class Damage : MonoBehaviour
    {
        public float damage = 30f;
        void OnTriggerEnter(Collider other)
        {
            PlayerControl player = other.gameObject.GetComponent<PlayerControl>();
            if (player != null)
            {
                Vector3 damageDirection = player.transform.position - transform.position;
                player.GetDamage(damage, damageDirection);
            }
        }
    }
}
