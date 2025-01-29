using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.VFX;

namespace Level3
{
    public class BlackBomb : MonoBehaviour
    {
        [Header("Status")]
        public float damage = 2;
        public float speed = 4.0f;
        public float chaseSpeed = 8.0f;
        public float rotationSpeed = 10.0f;
        public bool isDead = false;
        public float score = 20;

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
        //Specifics
        private bool captured = false;
        private bool thrown = false;
        private bool readyToThrow;
        [Header("Sounds")]
        public AudioSource fuseBurning;
        public AudioSource explosion;

 
        public GameObject vfxExplosion;
        private bool explode = false;
        public GameObject body1;
        public GameObject body2;
        public PlayerControl character;
        public GameObject player;

        void Start()
        {
            // Find the player object with the "Player" tag
            if(player==null)
                player = GameObject.FindGameObjectWithTag("Player");
            character= player.GetComponent<PlayerControl>();
            if (player != null)
            {
                target = player;
            }
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            if(target != null)
                playerIsDead = target.GetComponent<PlayerControl>().isDead;
        }

        public bool Captured
        {
            get { return captured; }
            set { captured = value; }
        }
        
        public bool Thrown
        {
            get { return thrown; }
            set { thrown = value; }
        }

        void moveToNextPoint()
        {
            if(pathPoints.Count > 0)
            {
                float distance = Vector3.Distance(pathPoints[currentPathIndex].position, transform.position);
                agent.destination = pathPoints[currentPathIndex].position;

                if(distance <= 4f)
                {
                    currentPathIndex++;
                    currentPathIndex %= pathPoints.Count;
                    // Check if it's the last path point AND the enemy is within a tolerance (optional)
                    if (currentPathIndex == 0 && distance <= 4f && pathPoints.Count > 8) // Adjust tolerance as needed
                    {
                        Explode();
                    }
                }

            }
        }

        void Update()
        {
            Walk();
            CapturedByPlayer();
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
            if (target == null)
                playerIsDead = true;
            else
                playerIsDead = target.GetComponent<PlayerControl>().isDead;

            // Verifica se o jogador está dentro do alcance usando um raycast
            RaycastHit hit;
            Vector3 rayDirection = transform.forward;
            if (agent.enabled)
            {

                moveDirection.y -= 9.81f * Time.deltaTime; // Aplica a gravidade
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
                    if (!fuseBurning.isPlaying)
                    {
                        fuseBurning.Play();
                    }
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
            }

            if (startCountdown)
            {
                chaseTime += Time.deltaTime;
                if (chaseTime >= 5f)
                {
                    // Causa dano em uma área esférica ao redor do inimigo
                    Explode();
                }
                else if(playerIsDead)
                {
                    startCountdown= false;
                }
            }


            Debug.DrawRay(transform.position, rayDirection * 2, Color.red);
        }

        void CapturedByPlayer()
        {
            if (captured && !readyToThrow)
            {
                chaseTime = 0;
                readyToThrow= true;
                //agent.isStopped= true;
                startCountdown = true;
                anim.SetInteger("transition", 0);
            }else if (thrown && captured)
            {
                captured= false;
                chaseTime = 4f;
            }
        }

        void Explode()
        {
            if (!explode)
            {
                explode = true;
                StartCoroutine(ExplodeAndDestroy());
                // Define o raio da explosão
                float radius = 4f;
                // Obtém todos os objetos dentro do raio da explosão
                Collider[] objectsInRange = Physics.OverlapSphere(transform.position, radius);
                foreach (Collider col in objectsInRange)
                {
                    // Verifica se o objeto é o jogador
                    PlayerControl player = col.gameObject.GetComponent<PlayerControl>();
                    // Check for enemies more efficiently
                    if (col.CompareTag("Enemy"))
                    {
                        character.AddHealth();
                        Destroy(col.gameObject);
                    }

                    Boo bossBoo = col.gameObject.gameObject.GetComponent<Boo>();
                    if (player != null)
                    {
                        // Causa dano ao jogador
                        Vector3 damageDirection = player.transform.position - transform.position;
                        player.GetDamage(damage, damageDirection);
                    }
                    else if (bossBoo != null)
                    {
                        Vector3 damageDirection = col.transform.position - transform.position;
                        bossBoo.GetHit(damageDirection);
                    }
                }
                isDead = true;
                if (thrown)
                {
                    target.GetComponent<PlayerControl>().playerScore += score;
                }

                //Destroy(gameObject);

            }
        }

        IEnumerator ExplodeAndDestroy()
        {
            body1.SetActive(false);
            body2.SetActive(false);
            gameObject.GetComponent<SphereCollider>().enabled = false;
            // Toca o som da explosão
            if (!explosion.isPlaying)
            {
                explosion.Play();
                Instantiate(vfxExplosion, transform.position, Quaternion.identity);
            }

            // Aguarda um pouco para o som ser reproduzido
            yield return new WaitForSeconds(explosion.clip.length);

            // Agora destrói o objeto
            Destroy(gameObject);
        }
    }
}