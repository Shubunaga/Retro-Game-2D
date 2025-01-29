using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Level3
{
    public class GoombaHead : MonoBehaviour
    {
        public Goomba goombaEnemy;
        public float cooldown = 0.2f;
        //private bool hitTimerVar = false;
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();

                if (player != null)
                {
                    //if(!hitTimerVar)
                    Debug.Log("Smash");
                    goombaEnemy.Smash(player);
                }
            }
        }

        public IEnumerator hitTimer()
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
            yield return new WaitForSeconds(cooldown);
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}