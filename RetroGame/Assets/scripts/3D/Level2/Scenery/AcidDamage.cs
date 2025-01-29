using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level2
{
    public class AcidDamage : MonoBehaviour
    {
        public float damagePerSecond = 10f; // dano por segundo

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player") // verifica se o objeto que está no trigger é o jogador
            {
                // aplica dano ao jogador
                other.gameObject.GetComponent<Level2.PlayerBase>().GetDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
