using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro.Examples;

public class EnvironmentController : MonoBehaviour
{
    //game controller    
    public int playerScore;
    public int playerLifes = 1;

    public Text scoreText;
    public Text lifesText;

    public GameObject gameOver;
    public GameObject winningScreen;

    public static EnvironmentController instance;

    public Button restartButton;

    public GameObject dialogPrefab;

    private string sceneName;
    //Player envPlayer;
    //private void Awake()
    //{
    //    envPlayer = GameObject.FindWithTag("Player").GetComponent<Player>();
    //}
    private void Awake()
    {        
        if(instance == null)// && (sceneName == "Level_01"))
        {
            instance = this;
            DontDestroyOnLoad(gameObject);            
        }
        else if (instance != this || sceneName != "Level_01")
        {
            Destroy(gameObject);
        }
        UpdateScoreText();
    }

    void Start()
    {
        //instance = this;
        //Time.timeScale = 1f;
        winningScreen = GameObject.Find("Win Boss Fight Panel");

        UpdateScoreText();
        if (playerScore == 0)// && (SceneManager.GetActiveScene().name == "Level_01"))
        {
            dialogPrefab.SetActive(true);
        }
        else dialogPrefab.SetActive(false);

        //restartButton = GameObject.Find("Button").GetComponent<Button>();
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() => RestartGame(SceneManager.GetActiveScene().name));
        }
    }
    
    //PAUSE
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Pause();
        //}

        //if (btn != null)
        //{
        //    btn.onClick.AddListener(Buttons);
        //}
        AutoDelete();
    }

    public void AutoDelete()
    {      
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "Level_01")
        {
            Destroy(gameObject);
        }

    }

    public void SetLives(int life)
    {
        playerLifes += life;
        if(playerLifes >= 0 ) { UpdateScoreText(); }
        
    }

    public void UpdateScoreText()
    {
        scoreText.text = playerScore.ToString();
        lifesText.text = playerLifes.ToString(); ;//envPlayer.lives.ToString();
    }

    public void ShowGameOver()
    {
        Debug.Log("Ativamos o game over");
        gameOver.SetActive(true);
    }

    public void RestartGame(string lvlName)
    {
        playerLifes = 1;
        gameOver.SetActive(false);
        SceneManager.LoadScene(lvlName);
    }

    public void ShowWinningScreen()
    {
        winningScreen.SetActive(true);        
    }

    //private void Pause()
    //{
        
        
    //    if (isPaused == false)
    //    {
    //        Time.timeScale = 0f;
    //        pauseScreen.SetActive(true);
    //        isPaused = true;
    //    } 
    //    else
    //    {
    //        Time.timeScale = 1f;
    //        //isPaused = false;
    //    }

    //}

}
