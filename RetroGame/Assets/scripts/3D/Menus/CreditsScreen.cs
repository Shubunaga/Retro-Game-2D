using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScreen : MonoBehaviour
{
    public AudioSource finalMusic;
    public Animator creditsAnimation;
    
    // Start is called before the first frame update
    void Start()
    {
        finalMusic.Play();   
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = creditsAnimation.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Credits") && stateInfo.normalizedTime >= 1.0f)
        {
            SceneManager.LoadScene("MainMenu3D");
            Debug.Log("CHANGE SCENE");
        }
    }
}
