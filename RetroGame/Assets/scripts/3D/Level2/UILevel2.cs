using Level2;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Level2
{
    public class UILevel2 : MonoBehaviour
    {
        // Referências para os componentes TextMeshPro na UI
        public TMP_Text lifeText;
        public TMP_Text shieldText;
        public TMP_Text ammoCountText;
        public TMP_Text keys;
        public Image redScreen;
        public float targetAlpha;
        private float alphaSpeed = 0.001f; // A velocidade de mudança do alpha
        // Referência para o script do jogador
        public PlayerBase player;
        public Button restartButton;
        public GameObject ingameUI; //UI dentro de jogo
        public GameObject gameOverUI;

        void Start()
        {
            // Inicia o jogo em um estado pausado
            //Time.timeScale = 0;
            // DesativateScreen(ingameUI);
            DesativateScreen(gameOverUI);

            // Adiciona o ouvinte ao evento de clique do botão restart
            restartButton.onClick.AddListener(OnResetButtonPressed);
        }

        void Update()
        {
            // Atualize o texto da UI com as variáveis do jogador
            lifeText.text = player.life.ToString();
            shieldText.text = player.shield.ToString();
            ammoCountText.text = player.ammoCount.ToString();
            if (player.key == true)
            {
                keys.text = "1";
            }
            else
            {
                keys.text = "0";
            }
            if (player.getDamage)
            {
                // Se myVariable for true, defina o alpha para o valor alvo
                Color color = redScreen.color;
                color.a = targetAlpha;
                redScreen.color = color;

                // Comece a corrotina para esperar 2 segundos antes de começar a diminuir o alpha
                StartCoroutine(WaitAndFade());

                // Redefina myVariable para false
                player.getDamage = false;
            }

        }

        IEnumerator WaitAndFade()
        {
            // Espere 2 segundos
            yield return new WaitForSeconds(2);

            // Comece a diminuir o alpha
            while (redScreen.color.a > 0)
            {
                Color color = redScreen.color;
                color.a -= alphaSpeed;
                redScreen.color = color;

                // Espere até o próximo quadro
                yield return null;
            }
        }

        public void PlayerDied()
        {
            // Mostra a tela de "Game Over" e o botão de reinício quando o jogador 
            DesativateScreen(ingameUI);
            ActivateScreen(gameOverUI);
            Time.timeScale = 0;
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

        void OnResetButtonPressed()
        {
            // Reinicia a cena atual quando o botão de reinício é pressionado
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            DesativateScreen(gameOverUI);
            Time.timeScale = 1;
            // Esconde a tela de "Game Over" e o botão de reinício

        }


    }
}