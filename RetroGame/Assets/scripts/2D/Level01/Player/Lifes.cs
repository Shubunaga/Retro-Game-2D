using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifes : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerSprite;
    public bool gameOver = false;

    EnvironmentController envControll;

    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        EnvironmentController.instance.UpdateScoreText();

        envControll = GameObject.Find("Canvas").GetComponent<EnvironmentController>();
    }

    public void loseLife()
    {
        if(envControll.playerLifes <= 0)
        {
            playerSprite.enabled = false;
            envControll.playerLifes = 0;
            EnvironmentController.instance.ShowGameOver();
        }
        else
        {
            EnvironmentController.instance.SetLives(-1);  
        }
    }
}
