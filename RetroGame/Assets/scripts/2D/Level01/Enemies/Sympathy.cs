using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sympathy : MonoBehaviour
{

    private Rigidbody2D rig;
    private BoxCollider2D boxCol;
    private CircleCollider2D cirCol;
    private Animator anim;
    public int Score;
    public float speed, JumpForce, timeLeft;
    public bool timerOn = false;

    public Transform rightCol, leftCol, headPoint;

    private bool colliding; // Jump

    public LayerMask layer;

    [SerializeField]Player playerGO;

    void Start()
    {
        timerOn = true;
        rig = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        cirCol = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();

        playerGO = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        rig.velocity = new Vector2(speed, rig.velocity.y);

        colliding = Physics2D.Linecast(rightCol.position, leftCol.position, layer);

        if (colliding)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
            speed *= -1f;
        }

        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                updateTimer(timeLeft);
            }
            else
            {   //pulo
                rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                anim.SetBool("Jump", true);
                timeLeft = 3.5f;
            }
        }

    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;
    }

    bool playerDestroyed;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            anim.SetBool("Jump", false);
        }

        if (collision.gameObject.tag == "Player")
        {
            float height = collision.contacts[0].point.y - headPoint.position.y;

            if (height > 0 && !playerDestroyed)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 7, ForceMode2D.Impulse);
                anim.SetTrigger("death");
                AudioController.current.PlayMusic(AudioController.current.jumpInEnemySFX);
                speed = 0;
                boxCol.enabled = false;
                cirCol.enabled = false;
                rig.Sleep();
                Destroy(gameObject, 1.4f);
                EnvironmentController.instance.playerScore +=  Score;
                EnvironmentController.instance.UpdateScoreText();
            }
            else
            {
                collision.gameObject.GetComponent<Lifes>().loseLife();
                
                playerDestroyed = true;
                AudioController.current.PlayMusic(AudioController.current.deathSFX);
                playerGO.Die();
                //EnvironmentController.instance.ShowGameOver();
                //Destroy(collision.gameObject);
            }
        }
    }
}