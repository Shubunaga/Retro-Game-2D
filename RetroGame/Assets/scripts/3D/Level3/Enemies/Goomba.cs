using Level3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Level3
{
    public class Goomba : MonoBehaviour
    {
        [Header("Status")]
        public float damage = 1;
        public float speed = 4.0f;
        public float chaseSpeed = 8.0f;
        public float rotationSpeed = 10.0f;
        public float score = 10;

        [Header("Agro")]
        public GameObject target;
        public float lookDistance = 2f;

        [Header("Path")]
        public List<Transform> pathPoints = new List<Transform>();
        public int currentPathIndex = 0;


        [Header("Sound")]
        public AudioSource smashSound;

        private bool chasingPlayer = false;
        private Animator anim;
        private Vector3 moveDirection;
        private float chaseTime = 0f;
        private bool startCountdown = false;
        private NavMeshAgent agent;
        private bool playerIsDead;
        //Specifics
        public bool die = false;
        private bool cooldown = false;
        public Collider deadCollider;
        private bool smashed = false;
        public GoombaHead hitDamage;

        void Start()
        {
            // Find the player object with the "Player" tag
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player;
            }
            else
            {
                Debug.LogError("Player object with tag 'Player' not found!");
            }
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
            if (!die)
            {
                Walk();
            }
            else
            {
                anim.SetInteger("transition", 0);
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
            // Verifica se o jogador está dentro do alcance usando um raycast
            RaycastHit hit;
            Vector3 rayDirection = transform.forward;
            //moveDirection.y -= 9.81f * Time.deltaTime; // Aplica a gravidade
            agent.isStopped = false;

            if (!playerIsDead)
            {
                if (Physics.Raycast(transform.position, rayDirection, out hit))
                {
                    if (hit.transform == target.transform && Vector3.Distance(target.transform.position, transform.position) < 2)
                    {
                        if (!chasingPlayer)
                        {
                            chasingPlayer = true;
                            startCountdown = true;
                        }

                    }
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
                anim.SetInteger("transition", 2);
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
                if (chaseTime >= 3f || playerIsDead)
                {
                    chasingPlayer = false;
                    startCountdown = false;
                    moveToNextPoint();
                }
            }

            Debug.DrawRay(transform.position, rayDirection * 2, Color.red);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Player")
            { 
                PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();

                if (player != null)
                {
                    if(!smashed && !cooldown)
                    {
                        StartCoroutine(hitDamage.hitTimer());
                        StartCoroutine(CooldownTimer());
                        Vector3 damageDirection = player.transform.position - transform.position;
                        player.GetDamage(damage, damageDirection);
                    }
                }
            }
        }

        IEnumerator CooldownTimer()
        {
            cooldown = true;
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
            yield return new WaitForSeconds(1.5f);
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            cooldown = false;
        }

        public void Smash(PlayerControl player)
        {
            smashed = true;
            Debug.Log("Matar");
            // Player jumped on the enemy's head
            anim.SetInteger("transition", 0);

            Vector3 currentScale = gameObject.transform.localScale;
            currentScale.y = 0.01f;
            gameObject.transform.localScale = currentScale;
            player.GetComponent<PlayerControl>().AddHealth();
            player.playerScore += score;
            Die();
        }

        void Die()
        {
            die = true;
            agent.isStopped = true;
            agent.enabled = false; // Desativa o NavMeshAgent
            BoxCollider box = GetComponent<BoxCollider>();
            box.enabled= false;

            // Move o inimigo um pouco para baixo
            Vector3 newPosition = transform.position;
            newPosition.y -= 0.3f; // Ajuste este valor conforme necessário
            transform.position = newPosition;
            StartCoroutine(smashAndDestroy());
        }

        IEnumerator smashAndDestroy()
        {
            if(!smashSound.isPlaying)
                smashSound.Play();
            yield return new WaitForSeconds(smashSound.clip.length);
            // Agora destrói o objeto
            Destroy(gameObject);
        }
    }
}