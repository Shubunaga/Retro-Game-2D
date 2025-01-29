using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    public GameObject gameOverScreen;

    void Start()
    {
        Button btn = GetComponent<Button>();
        if(btn != null)
        {
            btn.onClick.AddListener(ChangeScene);
            btn.onClick.AddListener(OnButtonClick);
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnButtonClick()
    {
        if(gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }
}
