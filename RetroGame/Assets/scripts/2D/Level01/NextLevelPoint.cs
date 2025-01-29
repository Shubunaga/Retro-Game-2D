using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NextLevelPoint : MonoBehaviour
{
    public string sceneName;
    private Button btn;
    [SerializeField] GameObject vidas;
    [SerializeField] GameObject score;
    [SerializeField] GameObject coinIcon;
    [SerializeField] GameObject pause;

    void Start()
    {
        btn = GetComponent<Button>();

        if (btn != null)
        {
            btn.onClick.AddListener(NextLevel);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "Player")
        {
            //AudioController.current.PlayMusic(AudioController.current.bgm);
            SceneManager.LoadScene(sceneName);
            vidas = GameObject.Find("Vidas");
            score = GameObject.Find("Score");
            coinIcon = GameObject.Find("Coin Icon");
            pause = GameObject.Find("Pause");
            
            vidas.SetActive(false);
            score.SetActive(false);
            coinIcon.SetActive(false);
            pause.SetActive(false);
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}
