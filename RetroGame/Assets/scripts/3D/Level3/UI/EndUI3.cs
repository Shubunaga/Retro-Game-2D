using Level3;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class EndUI3 : MonoBehaviour
{
    public GameObject[] gameObjectsOnScreen;
    public TMP_Text KillsText;
    public AudioSource scoreSfx;
    public AudioSource music;
    public float duration = 1.0f;            // A duração da animação da pontuação
    public float delayTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        //music.Play;
        int score = 200; 
        PlayerPrefs.SetInt("Score3", score); // Armazena a pontuação com a chave "Score"
        PlayerPrefs.Save(); // Salva as alterações
        StartCoroutine(TextSequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TextSequence()
    {
        int score = PlayerPrefs.GetInt("Score3"); // Recupera a pontuação
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 3; i++)
        {
            gameObjectsOnScreen[i].SetActive(true);
            yield return new WaitForSeconds(delayTime);
        }
        yield return null;
        StartCoroutine(AnimateScore(score));
    }

    IEnumerator AnimateScore(int targetScore)
    {
        if (targetScore == 0)
        {
            KillsText.text = "0";
            gameObjectsOnScreen[4].SetActive(true);
            yield break;
        }
        float elapsed = 0;
        int startScore = 0;
        bool isCounting = true;

        while (elapsed < duration && isCounting == true)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            int score = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, t));
            KillsText.text = score.ToString();

            // Toca o som de pontuação se não estiver tocando
            if (!scoreSfx.isPlaying)
            {
                scoreSfx.Play();
            }

            if (KillsText.text.Equals(targetScore.ToString()))
            {
                isCounting = false;
            }

            yield return null;
        }
        // Para o som de pontuação quando a contagem terminar
        if (scoreSfx.isPlaying)
        {
            scoreSfx.Stop();
        }

        // Garante que a pontuação final seja exatamente a pontuação alvo
        KillsText.text = targetScore.ToString();
        gameObjectsOnScreen[3].SetActive(true);
    }

    public void ContinueToLevel3()
    {
        SceneManager.LoadScene("Credits - 3D");
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene("Nivel 3 - 3D");
    }
}
