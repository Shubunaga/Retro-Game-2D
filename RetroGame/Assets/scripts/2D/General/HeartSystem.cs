using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HeartSystem : MonoBehaviour
{
    public int life;
    public int maxLife;

    [SerializeField] private BoxCollider2D bodyCol;
    [SerializeField] private CircleCollider2D lowBodyCol;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Rigidbody2D rbody;

    public GameObject extraLife;
    // public GameObject collected;
    public Image[] heart;
    public Sprite full;
    public Sprite hollow;
    public bool haveKey = false;

    [SerializeField] private float seconds;

    void Start()
    {
        //AudioController.current.PlayMusic(AudioController.current.bgm02);

        extraLife = GameObject.Find("Extra Life");

        playerSprite = GetComponentInParent<SpriteRenderer>();
    }

    void Update()
    {
        Health();
    }

    void Health()
    {
        
        if(life > maxLife)
        {
            life = maxLife;
        }
        
        for(int i = 0; i < heart.Length; i++)
        {
            if(i < life)
            {
                heart[i].sprite = full;
            }
            else
            {
                heart[i].sprite = hollow;
            }

            if(i < maxLife)
            {
                heart[i].enabled = true;
            }
            else
            {
                heart[i].enabled = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "GameOver"){
            // AudioController.current.PlayMusic(AudioController.current.deathSFX);
            EnvironmentController.instance.ShowGameOver();
            Destroy(gameObject);
        }

        if(collision.gameObject.tag == "Enemy"){
            life -= 1;
            Debug.Log(life);
            bodyCol.enabled = false;//DESATIVEI A COLISÃO DO PLAYER AQUI
            lowBodyCol.enabled = false;//DESATIVEI A COLISÃO DO PLAYER AQUI

            StartCoroutine(DamagePlayer());
        }

        if(life <=0 ){
            // animaçao
            // AudioController.current.PlayMusic(AudioController.current.deathSFX);
            gameObject.SetActive(false);
            EnvironmentController.instance.ShowGameOver(); //ainda não temos environmentController nessa cena!
            //EnvironmentController.instance.RestartGame("Level_02_BossFight");
        }

        if(collision.gameObject.tag == "Life" && life < maxLife)
        {
            Destroy(collision.gameObject);
            // collected = Instantiate(collected, gameObject.transform.position, Quaternion.identity);
            // Destroy(collected, 0.1f);
            life += 1;
            
        }

        if(collision.gameObject.tag == "Key" || MainManager.Instance.keyFound == true)// && buffCollected != null)
        {
            MainManager.Instance.keyFound = true;
            //haveKey = true;
            //fazer player regenerar 1 coração a cada 5 segundos
            if(collision.gameObject.tag == "Key") { Destroy(collision.gameObject);
                AudioController.current.PlayMusic(AudioController.current.powerUpPick); }

            InvokeRepeating("LifeRegen", 10f, 7f);
        }
        
    }  
    void LifeRegen(){
        if(life < maxLife && MainManager.Instance.keyFound)//&& haveKey)
        {
            life += 1;
        }
    }

    public IEnumerator DamagePlayer()
    {
        playerSprite.color = new Color(1f, 0, 0, 1f);
        yield return new WaitForSeconds(0.2f);
        playerSprite.color = new Color(1f, 1f, 1f, 1f);

        for (int i = 0; i < 7; i++) 
        {
            playerSprite.enabled = false;
            yield return new WaitForSeconds(0.15f);
            playerSprite.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }

        bodyCol.enabled = true;
        lowBodyCol.enabled = true;
    }
}
