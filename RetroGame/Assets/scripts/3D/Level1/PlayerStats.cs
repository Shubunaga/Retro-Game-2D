using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance; // Cria uma inst�ncia est�tica para que outros scripts possam acessar

    public int score = 0;

    void Awake()
    {
        // Define esta inst�ncia como a inst�ncia est�tica
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        PlayerPrefs.SetInt("Score", score); // Armazena a pontua��o com a chave "Score"
        PlayerPrefs.Save(); // Salva as altera��es
    }
}
