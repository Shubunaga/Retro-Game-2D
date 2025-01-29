using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level2
{
    public class CameraController : MonoBehaviour
    {
        public float speed = 5.0f;
        public float rotationSpeed = 200.0f;

        private CharacterController characterController;
        private void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            float translation = Input.GetAxis("Vertical") * speed;
            float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

            // Movimentação para frente e para trás
            translation *= Time.deltaTime;

            // Rotação esquerda e direita
            rotation *= Time.deltaTime;

            Vector3 forwardMovement = transform.forward * translation;
            characterController.SimpleMove(forwardMovement);

            transform.Rotate(0, rotation, 0);
        }
    }
}