using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance; // Cria uma instância estática para que outros scripts possam acessar

    public int score = 0;

    void Awake()
    {
        // Define esta instância como a instância estática
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
        PlayerPrefs.SetInt("Score", score); // Armazena a pontuação com a chave "Score"
        PlayerPrefs.Save(); // Salva as alterações
    }
}
