using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

namespace Level3
{
    public class PlayerControl : MonoBehaviour
    {
        private CharacterController characterController;
        public Animator anim;
        private Vector3 velocity;
        private float timeSinceJump;

        public Transform playerHand;
        public float playerScore;
        public bool isDead = false;
        public float playerHealth = 100;
        public float speed = 2f;
        public float gravityValue = -9.81f;
        private float _velocity;
        private Vector3 _direction;
        public float rotationSpeed;
        private int jumpCount = 0;
        public bool isJumping;
        public bool isWalking = true;
        public bool withObject;
        public float verticalSpeed;
        public float magnitud;
        public float maxJumpTime = 0.5f; // O tempo máximo que o personagem pode passar no ar
        public float jumpForce = 10f; // A força máxima do salto
        public float maxTimeBetweenJumps = 2;
        public float doubleJumpForce = 15;
        public float knockbackStrength = 3;
        public bool knockBack = false;
        private BlackBomb bomb;
        private float originalSpeed;
        private float originalRotation;
        private Boo boss;

        private Vector3 slideDirection;
        public float slopeLimit = 45f; // Ângulo máximo de inclinação que o personagem pode subir
        public float slideSpeed = 10f; // Velocidade de deslizamento
        private Vector3 normal;

        private float fallDistance = 0f;
        public float fallLimit = 0f;
        public bool isFall = false;
        private float previousYPosition;
        private float currentYPosition;
        public float raycastDistance = 1.0f; // Distância do raio
        public LayerMask groundLayer; // Camada que representa o chão
        public bool inBridge = false;
        [Header("Sound")]
        public AudioSource footStep;
        public AudioSource jumpSound;
        public AudioSource landSound;

        public CinemachineBrain mainCameraBrain;
        public bool cameraInCutScene = false;
        public bool cameraChanged = false;

        private Vector3 forwardDirection;
        private Vector3 rightDirection;
        // Start is called before the first frame 


        void Start()
        {
            boss = GameObject.FindObjectOfType<Boo>();
            characterController= GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
            anim.SetInteger("transition", 0);
            jumpCount= 0;
            originalSpeed = speed;
            originalRotation = rotationSpeed;
            playerScore= 0;
            withObject= false;
            inBridge = false;
        }   

        // Update is called once per frame
        void Update()
        {
            if (!cameraInCutScene && !isDead && !knockBack && !mainCameraBrain.IsBlending)
            {
                ApplyGravity();
                FallDamage();
                Jump();
                Move();
                Take_Object();
            }
            else if (mainCameraBrain.IsBlending)
            {
                Time.timeScale = 1;
            }
            else if(isDead)
            {
                characterController.enabled= false;
                return;
            }
            if (bomb != null)
            {
                if (bomb.isDead)
                {
                    withObject = false;
                }
            }
            else
            {
                withObject= false;
            }
            
        }

        public void AddHealth()
        {
            if(playerHealth < 8)
            {
                playerHealth++;
            }
        }

        public void GetDamage(float damage, Vector3 damageDirection)
        {
            playerHealth -= damage;
            if(playerHealth <= 0)
            {
                PlayerDie();
            }
            knockBack = true;
            float duration = 4f;
            //float duration = IsGrounded() ? 0.3f : 0.1f;
            if (!isDead && !isFall)
                StartCoroutine(Knockback(duration, damageDirection));
            else if(isFall)
                anim.SetTrigger("Fall"); isFall = false; fallDistance = 0f;
        }
        /*public IEnumerator Knockback(float duration, Vector3 hitDirection)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                characterController.Move(hitDirection * knockbackStrength * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            knockBack = false;
        }*/
        public IEnumerator Knockback(float maxDistance, Vector3 hitDirection)
        {
            float distanceTraveled = 0f;
            while (distanceTraveled < maxDistance)
            {
                Vector3 moveVector = hitDirection * knockbackStrength * Time.deltaTime;
                characterController.Move(moveVector);
                distanceTraveled += moveVector.magnitude;
                yield return null;
            }
            knockBack = false;
        }

        void FallDamage()
        {
            if (characterController.isGrounded)
            {
                previousYPosition = currentYPosition;
            }
            if (fallDistance > fallLimit && characterController.isGrounded)
            {
                //float damage = Mathf.Clamp(fallDistance - fallLimit, 0f, Mathf.Infinity);
                isFall = true;
                GetDamage(2, Vector3.up); // Apply damage based on fall distance
                knockBack = false;
            }
            if (!characterController.isGrounded && Vector3.Angle(normal, Vector3.up) < slopeLimit)
            {
                currentYPosition = transform.position.y;
                fallDistance = previousYPosition - currentYPosition;
            }
            else
            {
                fallDistance = 0f;  // Reset fall distance on ground
            }
        }

        void PlayerDie()
        {
            isDead = true;
            anim.SetTrigger("Die");
        }

        void Move()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (horizontalInput != 0 || verticalInput != 0)
            {
                if (cameraChanged)
                {
                    StartCoroutine(DelayCamera());
                }
                else
                {
                    // Calculate the forward and right directions based on the camera's rotation
                    forwardDirection = Camera.main.transform.forward;
                    rightDirection = Camera.main.transform.right;
                }

                // Remove any vertical movement from the directions
                forwardDirection.y = 0;
                rightDirection.y = 0;
                forwardDirection.Normalize();
                rightDirection.Normalize();

                // Calculate the movement direction based on the input
                Vector3 movementDirection = (forwardDirection * verticalInput) + (rightDirection * horizontalInput);
                movementDirection.Normalize();
                
                if (!isJumping && IsGrounded())
                {
                    if (!withObject)
                    {
                        anim.SetInteger("transition", 1);
                    }
                    else
                    {
                        anim.SetInteger("transition", 5);
                    }
                }

                // Move the character in the new direction
                float speedModifier = withObject ? 0.5f : 1f; // Reduce speed by 50% if withObject is true

                if (characterController.enabled)
                {
                    characterController.Move(movementDirection * Time.deltaTime * speed * speedModifier);
                    isWalking = true;
                }
                // If there is any movement, rotate the character to face the movement direction
                if (movementDirection != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                }

            }
            else if (!isJumping && !isWalking)
            {
                if (!withObject)
                    anim.SetInteger("transition", 0);
                else
                    anim.SetInteger("transition", 4);
            }
            else
            {
                isWalking= false;
            }
        }

        IEnumerator DelayCamera()
        {
            yield return new WaitForSeconds(0.5f);
            // Calculate the forward and right directions based on the camera's rotation
            forwardDirection = Camera.main.transform.forward;
            rightDirection = Camera.main.transform.right;
            cameraChanged = false;
        }

        void PlayFootStepSound()
        {
            if(IsGrounded())
                footStep.Play();
        }
        void PlayJumpSound()
        {
            jumpSound.Play();
        }
        void PlayLandingSound()
        {
            landSound.Play();
        }

        void Jump()
        {
            // Check if the character has landed
            if (IsGrounded())
            {
                //anim.SetInteger("transition", 1);
                isJumping = false;
                if (jumpCount == 2)
                {
                    jumpCount = 0;
                }
            }
            if (!isJumping)
            {
                rotationSpeed = originalRotation;
            }
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !withObject)
            {
                if (jumpCount == 0)
                {
                    // Aplica a força do pulo
                    anim.SetInteger("transition", 2);
                    isJumping = true;
                    _direction.y = jumpForce;
                    rotationSpeed = rotationSpeed / 2;
                    characterController.Move(_direction * Time.deltaTime);
                    jumpCount++;
                    timeSinceJump = 0; // Reseta o tempo desde o último pulo
                }
                else if (jumpCount == 1 && timeSinceJump < maxTimeBetweenJumps)
                {
                    anim.SetInteger("transition", 3);
                    isJumping = true;
                    _direction.y = doubleJumpForce;
                    rotationSpeed = rotationSpeed / 2;
                    jumpCount++;
                }
                isWalking = false;
            }

            if (timeSinceJump > maxTimeBetweenJumps && characterController.isGrounded)
            {
                jumpCount = 0;
                timeSinceJump = 0;
            }
            if (jumpCount == 1)
            {
                timeSinceJump += Time.deltaTime;
            }

            // Adiciona a velocidade ao movimento do personagem
            characterController.Move(_direction * Time.deltaTime);
        }

        public bool IsGrounded()
        {
            // Check if CharacterController thinks it's grounded
            if (characterController.isGrounded)
                return true;

            // Create a ray that points downwards from the bottom of the character
            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, -Vector3.up);

            // Perform the raycast
            if (Physics.Raycast(ray, 0.15f))
                return true;

            // If neither check succeeded, we're not grounded
            return false;
        }
        private void ApplyGravity()
        {
            if (!inBridge)
            {
                if (IsGrounded())
                {
                    _direction.y = -1.0f;
                }
                else
                {
                    _direction.y += gravityValue * Time.deltaTime;
                }  
            }
        }

        void Take_Object()
        {
            float heightOffset = 0.5f; // Change this value as needed
            // Define the range within which the character can take an object
            float range = 2.0f;
            // Define the radius of the sphere cast
            float radius = 0.6f;

            if (Input.GetKeyDown(KeyCode.O))
            {
                if (!withObject)
                {
                    Ray ray = new Ray(transform.position + new Vector3(0.5f, heightOffset, 0), transform.forward);
                    
                    // Define the hit information
                    RaycastHit hit;

                    // Cast the ray
                    if (Physics.SphereCast(ray, radius, out hit, range))
                    {
                        // Check if the object is the one you want to take
                        if (hit.collider.gameObject.CompareTag("Catchable"))
                        {
                            withObject = true;
                            // Take the object (you can define what this means for your game)
                            //Debug.Log("Object taken: " + hit.collider.gameObject.name);
                            Rigidbody rb = hit.collider.gameObject.GetComponent<Rigidbody>();
                            if (hit.collider.gameObject.GetComponent<BlackBomb>())
                            {
                                bomb = hit.collider.gameObject.GetComponent<BlackBomb>();
                                bomb.Captured = true;

                            }
                            NavMeshAgent agent = hit.collider.gameObject.GetComponent<NavMeshAgent>();
                            if (agent != null)
                            {
                                agent.enabled= false;
                            }
                            // Make the object a child of the character's hand
                            hit.collider.gameObject.transform.SetParent(playerHand);
                            anim.SetInteger("transition", 4);
                            // Position the object at the location of the character's hand
                            hit.collider.gameObject.transform.position = playerHand.position;

                            // Adjust the position of the object so that its bottom is at the transform location
                            float objectHeight = hit.collider.bounds.size.y;
                            hit.collider.gameObject.transform.position += new Vector3(0, objectHeight / 2, 0);
                            if (!hit.collider.gameObject.GetComponent<BlackBomb>())
                            {
                                float forwardOffset = 0.6f; // You can adjust this value as needed
                                hit.collider.gameObject.transform.position += playerHand.forward * forwardOffset;
                            }
                            // Reset the rotation of the object
                            hit.collider.gameObject.transform.localRotation = Quaternion.identity;

                            if (rb != null)
                            {
                                rb.isKinematic = true;
                            }

                        }
                    }
                }
                else
                {
                    if (playerHand.childCount > 0)
                    {
                        // Get the object that the character is holding
                        Transform objectInHand = playerHand.GetChild(0);

                        if (objectInHand.gameObject.GetComponent<BlackBomb>())
                        {
                            bomb = objectInHand.gameObject.GetComponent<BlackBomb>(); 
                            bomb.Thrown = true;
    }
                        // Unparent the object
                        objectInHand.SetParent(null);

                        // Add a Rigidbody component to the object if it doesn't have one
                        Rigidbody rb = objectInHand.gameObject.GetComponent<Rigidbody>();
                        if (rb == null)
                        {
                            rb = objectInHand.gameObject.AddComponent<Rigidbody>();
                        }

                        rb.isKinematic = false;

                        // Throw the object forward
                        float throwForce = 500.0f; // You can adjust this value as needed
                        rb.AddForce(playerHand.forward * throwForce);
                        withObject = false;

                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            float heightOffset = 0.5f; // Change this value as needed
            // Define the radius of the sphere cast
            float radius = 0.6f;
            Ray ray = new Ray(transform.position + new Vector3(0.5f, heightOffset, 0), transform.forward);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ray.origin + ray.direction, radius);
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 6)
                inBridge = false;
        }

        private void nTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 6)
                inBridge = false;
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                // Ajuste a posição do personagem para a posição X da plataforma
                Vector3 newPosition = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
                transform.position = newPosition;

                // Desative a gravidade (se necessário)
                _direction.y = 0f; // Supondo que _direction é o vetor de movimento do personagem
            }
        }*/
    }
}
