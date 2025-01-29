using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour
{
    public Text scoreText;
    public TextMeshProUGUI bombText;
    public Image lifeBar;
    public Image redBar;
    public Image boostBar;
    public Image bomb;
    public int currentHealth;
    public int currentBoost;
    private PlayerHealth player;
    private PlayerMovement2 moveset;
    private BombSpawer bomb2;

    void Start()
    {
        bomb2 = GameObject.FindObjectOfType<BombSpawer>();
        player = GameObject.FindObjectOfType<PlayerHealth>();
        moveset = GameObject.FindObjectOfType<PlayerMovement2>();
    }
    // Update is called once per frame
    void Update()
    {
        this.scoreText.text = PlayerStats.instance.score.ToString("D9");
        this.bombText.text = "x " + bomb2.bombLimit.ToString();
        SetHealth(player.currentHealth);
        SetBoost(moveset.boostAmount);
    }

    private void SetBoost(float boostAmount)
    {
        //currentBoost = Mathf.Clamp(boostAmount, 0, 100);
        Vector3 boostbarScale = boostBar.rectTransform.localScale;
        boostbarScale.x = (float)boostAmount / 100;
        boostBar.rectTransform.localScale = boostbarScale;
    }

    public void SetHealth(int amount)
    {
        currentHealth = Mathf.Clamp(amount, 0, player.maxHealth);

        Vector3 lifebarScale = lifeBar.rectTransform.localScale;
        lifebarScale.x = (float)currentHealth / player.maxHealth;
        lifeBar.rectTransform.localScale = lifebarScale;
        StartCoroutine(DecreasingRedBar(lifebarScale));
    }

    IEnumerator DecreasingRedBar(Vector3 newScale)
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 redBarScale = redBar.transform.localScale;
        while (redBar.transform.localScale.x > newScale.x)
        {
            redBarScale.x -= Time.deltaTime * 0.25f;
            redBar.transform.localScale = redBarScale;
        }
    }

}
