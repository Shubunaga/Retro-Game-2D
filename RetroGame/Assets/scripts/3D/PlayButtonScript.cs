using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class PlayButtonScript : MonoBehaviour
{
    public Button playButton; // O bot�o PLAY
    public Button restartButton;
    public GameObject menuUI; // A UI do menu
    public GameObject ingameUI; //UI dentro de jogo
    public GameObject gameOverUI;

    void Start()
    {
        // Inicia o jogo em um estado pausado
        //Time.timeScale = 0;
        //DesativateScreen(ingameUI);
        DesativateScreen(gameOverUI);

        // Adiciona o ouvinte ao evento de clique do bot�o PLAY
        playButton.onClick.AddListener(OnPlayButtonPressed);
        restartButton.onClick.AddListener(OnResetButtonPressed);
    }
    public void PlayerDied()
    {
        // Mostra a tela de "Game Over" e o bot�o de rein�cio quando o jogador morre
        ActivateScreen(gameOverUI);
    }
    public void ActivateScreen(GameObject screen)
    {
        screen.GetComponent<CanvasGroup>().alpha = 1;
        screen.GetComponent<CanvasGroup>().interactable = true;
    }
    public void DesativateScreen(GameObject screen)
    {
        screen.GetComponent<CanvasGroup>().alpha = 0;
        screen.GetComponent<CanvasGroup>().interactable = false;
    }

    void OnPlayButtonPressed()
    {
        PlayerPrefs.SetInt("Score", 0); // Armazena a pontua��o com a chave "Score"
        PlayerPrefs.Save(); // Salva as altera��es
        DesativateScreen(menuUI);
        // Retoma o jogo quando o bot�o PLAY � pressionado
        ActivateScreen(ingameUI);
        Time.timeScale = 1;    
    }

    void OnResetButtonPressed()
    {
        // Reinicia a cena atual quando o bot�o de rein�cio � pressionado
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        DesativateScreen(gameOverUI);
        // Esconde a tela de "Game Over" e o bot�o de rein�cio
    }
}
