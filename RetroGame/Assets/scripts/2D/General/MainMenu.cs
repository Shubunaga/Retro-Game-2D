using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Button btn;
    [SerializeField] GameObject telaInicial;
    [SerializeField] GameObject telaDeControles;
    [SerializeField] GameObject telaDeCreditos;

    private void Start()
    {
        btn  = GetComponent<Button>();
        if(btn != null)
        {
            btn.onClick.AddListener(LoadSceneBasedOnButtonName);
        }
    }

    public void LoadSceneBasedOnButtonName()
    {
        string buttonName = GetComponent<Button>().gameObject.name;

        Debug.Log("Botão clicado: " + buttonName);

        switch (buttonName)
        {
            case "Start Button":
                SceneManager.LoadScene("Level_01");
                break;
            case "Controls Button":
                telaInicial.SetActive(false);
                telaDeControles.SetActive(true);
                //SceneManager.LoadScene("Controls");
                break;
            case "About Button":
                telaInicial.SetActive(false);
                telaDeCreditos.SetActive(true);
                //SceneManager.LoadScene("AboutGame");
                break;
            case "Back Button":
                telaDeControles.SetActive(false);
                telaDeCreditos.SetActive(false);
                telaInicial.SetActive(true);
                //Ativar a tela principal novamente e desativar essa
                //SceneManager.LoadScene("TelaDeMenuInicial");
                break;
            case "Exit":
                Debug.Log("Saímos do jogo");
                break;
            default:
                Debug.Log("Nenhuma cena correspondente para o botão: " + buttonName);
                break;
        }
    }
}
