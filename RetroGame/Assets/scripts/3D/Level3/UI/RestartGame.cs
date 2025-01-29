using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Level3
{
    public class RestartGame : MonoBehaviour
    {
        public GameObject telaMorte; // Tela de reiniciar o jogo
        public PlayerControl playerControl; // Script do jogador
        public Button restartButton;
        private GameControllerLevel3 uiplayer;

        private void Start()
        {
            uiplayer = GameObject.FindObjectOfType<GameControllerLevel3>();
            telaMorte.SetActive(false);
            // Adiciona o ouvinte ao evento de clique do botão restart
            restartButton.onClick.AddListener(OnResetButtonPressed);
        }
        void Update()
        {
            // Se a vida do jogador for menor ou igual a 0, ele está morto
            if (playerControl.playerHealth <= 0)
            {
                uiplayer.healthUI.sprite = uiplayer.healthSprites[0];
                StartCoroutine(playerDeath());

            }
        }

        IEnumerator playerDeath()
        {
            yield return new WaitForSeconds(2f);
            // Mostra a tela de morte
            telaMorte.SetActive(true);
            // Desativa o jogador
            playerControl.gameObject.SetActive(false);
        }
        void OnResetButtonPressed()
        {
            // Reinicia a cena atual quando o botão de reinício é pressionado
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            telaMorte.SetActive(false);
        }

        public void Restart()
        {
            // Reinicia a cena atual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    } 
}
