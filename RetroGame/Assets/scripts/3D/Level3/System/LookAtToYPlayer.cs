using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class LookAtToYPlayer : MonoBehaviour
    {
        public Transform player; // O Transform do seu jogador
        private Vector3 lookAtPosition;
        public float zDistance = 3;
        public bool canMoveZ = false;
        public bool cannotMoveZmore = false;
        private float originalZPosition;

        private void Start()
        {
            canMoveZ= false;
            cannotMoveZmore= false;
            originalZPosition = transform.position.z;
        }
        void LateUpdate()
        {
            // Atualiza a posição Y do objeto "Look At" para corresponder à posição Y do jogador
            if (canMoveZ)
            {
                lookAtPosition = new Vector3(transform.position.x, player.position.y, player.position.z + zDistance);
                transform.position = lookAtPosition;
            }
            else if(cannotMoveZmore)
            {
                lookAtPosition = new Vector3(transform.position.x, player.position.y, originalZPosition);
                transform.position = lookAtPosition;
                cannotMoveZmore= false;
            }
            else
            {
                lookAtPosition = new Vector3(transform.position.x, player.position.y, transform.position.z);
                transform.position = lookAtPosition;
            }
        }
    }

}