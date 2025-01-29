using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float JumpForce;

    Vector2 checkPointPos;
    SpriteRenderer playerSprite;
    public GameObject respawnEffect;

    private Rigidbody2D rig;
    private Animator anim;

    public bool isJumping;
    Lifes playerGo;
    //EnvironmentController envControll;

    private void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        playerGo = GetComponent<Lifes>();
        //envControll = GameObject.Find("Environment Controller").GetComponent<EnvironmentController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        checkPointPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * Speed;
        
        if(Input.GetAxis("Horizontal") > 0f)
        {
          anim.SetBool("walk", true);
          transform.eulerAngles = new Vector3(0f,0f,0f);
        }
        
        if(Input.GetAxis("Horizontal") < 0f)
        {
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f,180f,0f);
        }

        if(Input.GetAxis("Horizontal") == 0f)
        {
            anim.SetBool("walk", false);
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump")  && !isJumping)
        {
            rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            AudioController.current.PlayMusic(AudioController.current.jump);
            anim.SetBool("jump", true);
        }
    }

    public void Die()
    {
        StartCoroutine(Respawn());
        EnvironmentController.instance.UpdateScoreText();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            isJumping = false;
            anim.SetBool("jump", false);
        }

        if(collision.gameObject.tag == "GameOver")
        {
            //collision.gameObject.GetComponent<Lifes>().loseLife();
            playerGo.loseLife();
            AudioController.current.PlayMusic(AudioController.current.deathSFX);
            Die();
        }

        //if(collision.gameObject.tag == "GameOver" || collision.gameObject.tag == "Enemy")
        //{
        //    AudioController.current.PlayMusic(AudioController.current.deathSFX);

        //    if(EnvironmentController.instance.playerLifes > 0)//envControll.playerLifes <= 1)
        //    {
        //        Die();
                
        //        //enabled = false;
        //    } else
        //    {
        //        playerSprite.enabled = false;
        //        EnvironmentController.instance.ShowGameOver();
        //    }
        //}

        if(collision.gameObject.tag == "Trampoline")
        {
            AudioController.current.PlayMusic(AudioController.current.Trampoline);
        }
    }

    
    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            isJumping = true;
        }

        if((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "GameOver") && EnvironmentController.instance.playerLifes >= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        rig.velocity = Vector3.zero;
        playerSprite.enabled = false;
        rig.simulated = false;
        respawnEffect.SetActive(true);
        //transform.localScale = new Vector3(0,0,0);

        yield return new WaitForSeconds(0.5f);

        
        respawnEffect.SetActive(false);
        transform.position = checkPointPos;
        playerSprite.enabled = true;
        //transform.localScale = new Vector3(1, 1, 1);
        rig.simulated = true;
           
    }
    
    public void UpdateCheckpoint(Vector2 pos)
    {
        checkPointPos = pos;
    }
}
