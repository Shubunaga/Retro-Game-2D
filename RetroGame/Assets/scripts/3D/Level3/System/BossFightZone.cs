using Cinemachine;
using RailShooter;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Level3
{
    public class BossFightZone : MonoBehaviour
    {
        [SerializeField] private GameObject boss; // Reference to the boss GameObject
        [SerializeField] private GameObject[] enemySpawners; // Array of enemy spawner GameObjects
        [SerializeField] private float triggerRadius = 5f; // Radius of the trigger collider
        [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
        public Vector3 newFollowOffset = new Vector3(0f, 4f, -5f);
        private Vector4 currentFollowOffset;
        private Collider[] enemyColliders;
        public Transform[] pathPoints;  // Array of path points for enemy to follow
        public bool isPlayerInside = false; // Flag to track player presence
        private bool needRestart = false;
        public AudioSource gameTheme;
        public AudioSource bossTheme;
        public AudioSource bossAwake;
        private Boo booLife;
        private CinemachineOrbitalTransposer transposer;
        private PlayerControl playerScript;
        public Transform playerLookAt;
        public GameObject bossFOV;
        private bool stopedFight;

        private void Start()
        {
            playerScript = GameObject.FindObjectOfType<PlayerControl>();
            transposer = virtualCameras[0].GetCinemachineComponent<CinemachineOrbitalTransposer>();
            currentFollowOffset = transposer.m_FollowOffset;
            booLife = GameObject.FindObjectOfType<Boo>();
            // Ensure initial disabled state
            boss.SetActive(false);
            foreach (GameObject spawner in enemySpawners)
            {
                spawner.SetActive(false);
            }
            stopedFight= false;
        }

        private void Update()
        {
            if (booLife.life <= 0 && !stopedFight)
            {
                StopBossFight();
                stopedFight = true;
            }
                
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && booLife.bossLive)
            {
                //StartCoroutine(BossAwakening());
                //virtualCamera.LookAt = booLife.transform;

                isPlayerInside = true;
                StartBossFight();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && booLife.bossLive)
            {
                bossTheme.Stop();
                transposer.m_FollowOffset = currentFollowOffset;
                isPlayerInside = false;
                needRestart = true;
                ResetBossFight();
                gameTheme.Play();
            }
        }

        private void StartBossFight()
        {
            if (!boss) // Check if boss hasn't been instantiated yet
            {
                boss = Instantiate(boss, transform.position, transform.rotation); // Instantiate the boss
            }
            gameTheme.Stop();
            StartCoroutine(IncreaseBossTransparency());
        }

        /*IEnumerator BossAwakening()
        {
            playerScript.cameraInCutScene = true;
            virtualCameras.LookAt = booLife.transform;
            yield return null;
        }*/

        IEnumerator IncreaseBossTransparency()
        {
            while (!playerScript.IsGrounded() && isPlayerInside)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.25f);
            virtualCameras[0].Priority = 0;
            virtualCameras[1].Priority = 1;
            playerScript.cameraChanged = true;
            // Ensure boss is active
            playerScript.cameraInCutScene = true;
            boss.SetActive(true);
            bossAwake.Play();
            Boo booScript = boss.GetComponent<Boo>();
            booScript.pathPoints.Clear();  // Clear existing path points in enemy script
            booScript.pathPoints.AddRange(pathPoints);  // Add current path points to enemy script
            // Get initial transparency (now guaranteed to be 0)
            float currentAlpha = 0;
            float targetAlpha = 1f;
            float alphaStep = 0.25f; // Alpha increase every 0.25 seconds
            Renderer renderer = boss.GetComponent<Renderer>();
            boss.GetComponent<NavMeshAgent>().enabled = false;
            foreach (Material material in renderer.materials)
            {
                Color color = material.color;
                color.a = currentAlpha;  // Altere este valor para ajustar a transparência
                material.color = color;
            }
            // Disable AI scripts initially
            booScript.enabled = false;
            FieldOfView fovScript = boss.GetComponent<FieldOfView>();
            fovScript.enabled = false;

            // Loop until target alpha is reached
            while (currentAlpha < targetAlpha)
            {
                foreach (Material material in renderer.materials)
                {
                    Color color = material.color;
                    color.a = currentAlpha;  // Altere este valor para ajustar a transparência
                    material.color = color;
                }

                // Update alpha (avoid exceeding target)
                currentAlpha = Mathf.Min(currentAlpha + alphaStep, targetAlpha);

                // Wait for 0.25 seconds before next iteration
                yield return new WaitForSeconds(0.5f);
            }
            booLife.bossSounds[0].Play();
            // Enable AI scripts after fade is complete
            boss.GetComponent<NavMeshAgent>().enabled = true;
            booScript.enabled = true;
            fovScript.enabled = true;
            StartCoroutine(EnableSpawners());
            bossTheme.Play();
            bossFOV.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            playerScript.cameraInCutScene = false;
            //follow the player from further away
            transposer.m_FollowOffset = newFollowOffset;
            virtualCameras[0].Priority = 1;
            virtualCameras[1].Priority = 0;
            playerScript.cameraChanged = true;
            //Debug.Log("Boss fully visible and ready to move!");
        }

        IEnumerator EnableSpawners()
        {
            print("Spawn");
            foreach (GameObject spawner in enemySpawners)
            {
                spawner.SetActive(true);
                // Trigger enemy spawning logic for each spawner
            }
            if (needRestart)
            {
                // Loop through colliders and handle enemies
                foreach (Collider collider in enemyColliders)
                {
                    print("Enemies");
                    // Handle the enemy: deactivate or destroy
                    collider.gameObject.SetActive(true); // Deactivate for re-use
                                                         // Alternatively, Destroy(collider.gameObject); // Destroy if needed
                }
            }
            yield return null;
        }
        private void ResetBossFight()
        {
            if (boss.activeInHierarchy || !isPlayerInside)
            {
                //boss.SetActive(false);
                // Reset boss health, animations, AI state, etc.
                boss.SetActive(false);
                bossFOV.SetActive(false);
                foreach (GameObject spawner in enemySpawners)
                {
                    spawner.SetActive(false);
                    // Reset enemy spawner logic
                }
                // Define the enemy layer
                int enemyLayer = LayerMask.NameToLayer("Enemy"); // Replace "Enemy" with your actual layer name

                // Create a LayerMask combining both tags using bitwise OR
                LayerMask enemyMask = LayerMask.GetMask("Enemy", "Catchable"); // Replace tags if needed

                // Find all active objects on the enemy layer matching the combined mask
                enemyColliders = Physics.OverlapSphere(transform.position, triggerRadius, enemyMask);
                // Loop through colliders and handle enemies
                foreach (Collider collider in enemyColliders)
                {
                    // Handle the enemy: deactivate or destroy
                    collider.gameObject.SetActive(false); // Deactivate for re-use
                    //Destroy(collider.gameObject); // Destroy if needed
                }
            }
        }

        private void StopBossFight()
        {
            foreach (GameObject spawner in enemySpawners)
            {
                spawner.SetActive(false);
                // Reset enemy spawner logic
            }
            bossFOV.SetActive(false);
            // Define the enemy layer
            int enemyLayer = LayerMask.NameToLayer("Enemy"); // Replace "Enemy" with your actual layer name

            // Create a LayerMask combining both tags using bitwise OR
            LayerMask enemyMask = LayerMask.GetMask("Enemy", "Catchable"); // Replace tags if needed

            // Find all active objects on the enemy layer matching the combined mask
            enemyColliders = Physics.OverlapSphere(transform.position, triggerRadius, enemyMask);
            // Loop through colliders and handle enemies
            foreach (Collider collider in enemyColliders)
            {
                // Handle the enemy: deactivate or destroy
                collider.gameObject.SetActive(false); // Deactivate for re-use
                                                      //Destroy(collider.gameObject); // Destroy if needed
            }
            bossTheme.Stop();
        }
    }

}