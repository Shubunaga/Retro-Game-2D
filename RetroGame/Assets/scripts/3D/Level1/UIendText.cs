using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIendText : MonoBehaviour
{
    public float delay = 0.1f; // Atraso entre cada letra
    private string fullText;
    public TMP_Text textComponent;
    public TMP_Text textToDisplay; // O texto que voc� quer exibir
    public TMP_Text scoreText; // O texto que voc� quer exibir
    public float duration = 1.0f; // A dura��o da anima��o da pontua��o   

    public RectTransform imageToMove; // A imagem que voc� quer mover
    public Vector3 startPosition; // A posi��o inicial da imagem
    public Vector3 endPosition; // A posi��o final da imagem
    public float speed = 1.0f; // A velocidade da movimenta��o
    private bool shouldMove = false;
    private float progress = 0;

    public Button restartButton;
    public Button continueButton;
    public AudioSource scoreSound;

    void Start()
    {

        startPosition = imageToMove.localPosition;
        textToDisplay.color = new Color(textToDisplay.color.r, textToDisplay.color.g, textToDisplay.color.b, 0);
        fullText = textComponent.text; // Armazena o texto completo
        textComponent.text = ""; // Come�a com o texto vazio
        restartButton.onClick.AddListener(OnResetButtonPressed);
        continueButton.onClick.AddListener(OnContinueButtonPressed);
        restartButton.gameObject.SetActive(false);
        restartButton.interactable = false;
        continueButton.gameObject.SetActive(false);
        continueButton.interactable = false;
        StartCoroutine(TypeText());
    }
    void Update()
    {
        if (shouldMove)
        {
            // Mova a imagem de startPosition para endPosition
            progress += speed * Time.deltaTime;
            imageToMove.localPosition = Vector3.Lerp(startPosition, endPosition, progress);
            if (imageToMove.localPosition == endPosition)
            {
                textToDisplay.color = new Color(textToDisplay.color.r, textToDisplay.color.g, textToDisplay.color.b, 1);
                int score = PlayerPrefs.GetInt("Score"); // Recupera a pontua��o
                StartCoroutine(AnimateScore(score));
                shouldMove= false;
            }
        }
    }
    public void StartMoving()
    {
        shouldMove = true;
        progress = 0; // Reinicia o progresso quando come�a a mover
    }
    IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(delay);
        }
        StartMoving();
    }
    IEnumerator AnimateScore(int targetScore)
    {
        if (targetScore == 0)
        {
            scoreText.text = "0";
            restartButton.gameObject.SetActive(true); // Mostra o bot�o
            restartButton.interactable = true; // Ativa a intera��o
            continueButton.gameObject.SetActive(true); // Mostra o bot�o
            continueButton.interactable = true; // Ativa a intera��o
            yield break;
        }
        float elapsed = 0;
        int startScore = 0;
        bool isCounting = true;

        while (elapsed < duration && isCounting==true)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            int score = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, t));
            scoreText.text = score.ToString();

            // Toca o som de pontua��o se n�o estiver tocando
            if (!scoreSound.isPlaying)
            {
                scoreSound.Play();
            }

            if (scoreText.text.Equals(targetScore.ToString()))
            {
                isCounting= false;
            }

            yield return null;
        }
        // Para o som de pontua��o quando a contagem terminar
        if (scoreSound.isPlaying)
        {
            scoreSound.Stop();
        }

        // Garante que a pontua��o final seja exatamente a pontua��o alvo
        scoreText.text = targetScore.ToString();
        restartButton.gameObject.SetActive(true); // Mostra o bot�o
        restartButton.interactable = true; // Ativa a intera��o
        continueButton.gameObject.SetActive(true); // Mostra o bot�o
        continueButton.interactable = true; // Ativa a intera��o
    }
    void OnResetButtonPressed()
    {
        SceneManager.LoadScene("Nivel 1 - 3D");
    }
    void OnContinueButtonPressed()
    {
        SceneManager.LoadScene("Nivel 2 - 3D");
    }

}
