using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteRenderer sr;
    private CircleCollider2D circle;
    
    public GameObject collected;
    public int Score;

    //Player player;

    //private void Awake()
    //{
    //    player = GameObject.FindWithTag("Player").GetComponent<Player>();
    //}
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        circle = GetComponent<CircleCollider2D>();
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            sr.enabled = false;
            circle.enabled = false;
            collected.SetActive(true);
            AudioController.current.PlayMusic(AudioController.current.Coin);

            EnvironmentController.instance.playerScore +=  Score;
            EnvironmentController.instance.UpdateScoreText();
            //
            Destroy(gameObject, 0.3f);

            if(EnvironmentController.instance.playerScore >= 99)
            {
                EnvironmentController.instance.SetLives(+1);
                //EnvironmentController.instance.playerLifes += 1;
                EnvironmentController.instance.playerScore -= 100;
                
                //
            }

        }
    }
}
