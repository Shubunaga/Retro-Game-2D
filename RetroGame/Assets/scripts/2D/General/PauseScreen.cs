using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PauseScreen : MonoBehaviour
{
    private bool isPaused;
    private Button btn;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject telaDeControles;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        btn = GetComponent<Button>();

        if (btn != null)
        {
            btn.onClick.AddListener(Buttons);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseScreen.activeSelf)
        {
            Debug.Log("Antes: " + isPaused);
            Pause();
            Debug.Log("Depois: " + isPaused);
            Debug.Log("Pausamos o jogo");

        }

    }

    public void Pause()
    {
        
        if (!isPaused)
        {   
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            isPaused = true;
        }
        else
        {
            Debug.Log("is paused do else: " + isPaused);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false); // Esconde a tela ao despausar
            isPaused = false; // Restaura o estado corretamente
        }

    }

    public void Buttons()
    {
        string buttonName = GetComponent<Button>().gameObject.name;
        Debug.Log("Botão clicado: " + buttonName);

        switch (buttonName)
        {
            case "Resume":
                Debug.Log("antes no resume: " + isPaused);
                Pause();
                Debug.Log("Botão depois: " + isPaused);
                break;
            case "Controls":
                pauseScreen.SetActive(false);
                telaDeControles.SetActive(true);
                break;
            case "Exit":
                SceneManager.LoadScene("TelaDeMenuInicial");
                break;
            case "Back Button":
                telaDeControles.SetActive(false);
                pauseScreen.SetActive(true);
                break;
            case "Quit":
            #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
            #else
                    Application.Quit();
            #endif
                break;
            default:
                Debug.Log("Nenhuma cena correspondente para o botão: " + buttonName);
                break;
        }
    }
}
