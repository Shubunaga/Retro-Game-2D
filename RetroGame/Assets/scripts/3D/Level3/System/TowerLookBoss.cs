using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class TowerLookBoss : MonoBehaviour
    {
        public CinemachineVirtualCamera baseCamera;
        public CinemachineVirtualCamera towerCamera;
        public LookAtToYPlayer lookY;
        private PlayerControl playerScript;

        private void Start()
        {
            playerScript = GameObject.FindObjectOfType<PlayerControl>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                lookY.canMoveZ= true;
            }

        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                baseCamera.Priority = 0;
                towerCamera.Priority = 1;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                lookY.canMoveZ= false;
                lookY.cannotMoveZmore= true;
                playerScript.cameraChanged = true;
            }
        }
    } 
}
