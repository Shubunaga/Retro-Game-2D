using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Level3
{
    public class Chomp : MonoBehaviour
    {
        [Header("Status")]
        public float damage = 40;
        public float speed = 4.0f;
        public float chaseSpeed = 8.0f;
        public float rotationSpeed = 10.0f;
        public bool isDead = false;

        [Header("Agro")]
        public GameObject target;
        public float lookDistance = 2f;

        [Header("Path")]
        public List<Transform> pathPoints = new List<Transform>();
        public int currentPathIndex = 0;

        private bool chasingPlayer = false;
        private Animator anim;
        private Vector3 moveDirection;
        private float chaseTime = 0f;
        private bool startCountdown = false;
        private NavMeshAgent agent;
        private bool playerIsDead;
        //Specific
        public float lookRadius = 10f;
        public float cooldown = 0;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            playerIsDead = target.GetComponent<PlayerControl>().isDead;
        }

        void moveToNextPoint()
        {
            if (pathPoints.Count > 0)
            {
                float distance = Vector3.Distance(pathPoints[currentPathIndex].position, transform.position);
                agent.destination = pathPoints[currentPathIndex].position;

                if (distance <= 4f)
                {
                    currentPathIndex++;
                    currentPathIndex %= pathPoints.Count;
                }
            }
        }

        void Update()
        {
            Walk();
            if(cooldown > 0)
            {
                cooldown -= Time.deltaTime;
            }
        }

        void LookTarget()
        {
            Vector3 direction = target.transform.position - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }

        void Walk()
        {
            playerIsDead = target.GetComponent<PlayerControl>().isDead;
            // Verifica se o jogador está dentro do alcance
            float distance = Vector3.Distance(target.transform.position, transform.position);
            if (agent.enabled)
            {
                if (!playerIsDead)
                {
                    if (distance <= lookRadius)
                    {
                        agent.SetDestination(target.transform.position);
                        chasingPlayer = true;
                        startCountdown = true;
                    }
                    else
                    {
                        chasingPlayer = false;
                        startCountdown = false;
                        moveToNextPoint();
                    }
                }
                else
                {
                    chasingPlayer = false;
                }

                if (chasingPlayer)
                {
                    agent.acceleration = chaseSpeed;
                    agent.SetDestination(target.transform.position);
                    anim.SetInteger("transition", 1);
                    LookTarget();
                }
                else if (!chasingPlayer)
                {
                    agent.acceleration = speed;
                    moveToNextPoint();
                    anim.SetInteger("transition", 1);
                }
                else
                {
                    anim.SetInteger("transition", 0);
                }
                if (startCountdown)
                {
                    chaseTime += Time.deltaTime;
                    if (chaseTime >= 3f)
                    {
                        float dashSpeed = 20f; // Set your dash speed
                        agent.speed = dashSpeed; // Set agent speed to dash speed
                        StartCoroutine(EndDash(speed)); // Start a coroutine to end the dash after a certain time
                    }
                }
            }
        }

        IEnumerator EndDash(float originalSpeed)
        {
            yield return new WaitForSeconds(1f); // Dash duration
            agent.isStopped= true;
            yield return new WaitForSeconds(3f); // Stop duration
            agent.isStopped= false;
            chaseTime = 0;
            agent.speed = originalSpeed; // Reset speed after dash
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere (transform.position, lookRadius);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                PlayerControl player = other.gameObject.GetComponent<PlayerControl>();

                if (player != null && cooldown <= 0)
                {
                    print("Dano enter");
                    cooldown = 2f;
                    Vector3 damageDirection = player.transform.position - transform.position;
                    player.GetDamage(damage, damageDirection);
                    print("Dano no player");
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                PlayerControl player = other.gameObject.GetComponent<PlayerControl>();

                if (player != null && cooldown <= 0)
                {
                    print("Dano stay");
                    cooldown = 2f;
                    Vector3 damageDirection = player.transform.position - transform.position;
                    player.GetDamage(damage, damageDirection);
                    print("Dano no player");
                }
            }
        }
    } 
}
