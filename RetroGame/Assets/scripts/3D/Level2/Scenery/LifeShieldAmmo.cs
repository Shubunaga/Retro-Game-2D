using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level2
{
    public class LifeShieldAmmo : MonoBehaviour
    {
        public float lifeBonus = 50f;
        public float ShieldBonus = 25f;
        public float AmmoBonus = 10f;

        void OnTriggerEnter(Collider other)
        {
            // Verifica se o objeto que entrou no trigger � o jogador
            if (other.gameObject.CompareTag("Player"))
            {
                // Obt�m o script de estat�sticas do jogador
                PlayerBase player = other.gameObject.GetComponent<Level2.PlayerBase>();

                // Verifica a tag do item e adiciona o b�nus correspondente ao jogador
                if (gameObject.CompareTag("Life"))
                {
                    player.AddLife(lifeBonus);
                }
                else if (gameObject.CompareTag("Shield"))
                {
                    player.AddShield(ShieldBonus);
                }
                else if (gameObject.CompareTag("Ammo"))
                {
                    player.AddAmmo(AmmoBonus);
                }

                // Destroi o item depois que o jogador pegou
                Destroy(gameObject);
            }
        }
    }
}