using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misery : MonoBehaviour
{
    private Rigidbody2D rig;
    private Animator anim;
    public int Score;

    public float speed;

    public Transform leftCol, rightCol, headPoint;

    private bool colliding;

    public LayerMask layer;

    [SerializeField] Player playerGO;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        playerGO = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        rig.velocity = new Vector2(speed, rig.velocity.y);

        colliding = Physics2D.Linecast(rightCol.position, leftCol.position, layer);

        if(colliding)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
            speed *= -1f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player")
        {
            float height = collision.contacts[0].point.y - headPoint.position.y;

            if (height > 0)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 7, ForceMode2D.Impulse);
                anim.SetTrigger("death");
                Destroy(gameObject, 0.3f);
                EnvironmentController.instance.playerScore +=  Score;
                EnvironmentController.instance.UpdateScoreText();
            }
            else
            {
                collision.gameObject.GetComponent<Lifes>().loseLife();
                
                AudioController.current.PlayMusic(AudioController.current.deathSFX);
                playerGO.Die();
            }
        }
    }
}
