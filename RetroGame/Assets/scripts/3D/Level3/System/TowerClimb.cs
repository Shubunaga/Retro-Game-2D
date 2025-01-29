using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class TowerClimb : MonoBehaviour
    {
        public CinemachineVirtualCamera baseCamera;
        public CinemachineVirtualCamera towerCamera;
        public Transform lookAtTower;
        public PlayerControl playerScript;

        private void Start()
        {
            playerScript = GameObject.FindObjectOfType<PlayerControl>();    
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                Debug.Log("Entrou");
                towerCamera.LookAt = lookAtTower;
                baseCamera.Priority = 0;
                towerCamera.Priority = 1;
                playerScript.cameraChanged = true;
            }

        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                baseCamera.Priority = 1;
                towerCamera.Priority = 0;
                playerScript.cameraChanged = true;
            }
        }
    } 
}
