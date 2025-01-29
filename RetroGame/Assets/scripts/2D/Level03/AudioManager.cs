using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("----------- Audio Source -----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------- Audio Clip -----------")]
    public AudioClip background;
    public AudioClip specialATK;
    public AudioClip bossSpecialATK;
    public AudioClip bossPunches;
    public AudioClip level03Ending;

    private string currentScene;
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  
        Debug.Log("Current scene on Start: " + SceneManager.GetActiveScene().name);
        PlayMusicForScene(SceneManager.GetActiveScene().name);       
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " +  scene.name);
        
        PlayMusicForScene(scene.name); //Toca a música correta dependendo da cena
        Debug.Log("A CENA CARREGOU");
    }

    private void PlayMusicForScene(string sceneName)
    {
        currentScene = sceneName;
        Debug.Log("Playing music for scene: " + sceneName);

        // Para a música atual
        //musicSource.Stop();

        switch (sceneName)
        {
            case "Level_03":
                musicSource.clip = background;
                
                break;
            case "Ending_Level03":
                musicSource.clip = level03Ending;
                break;
        }
        
        //reproduz a música condizente com a cena
        musicSource.Play();
        Debug.Log("Music playing: " + musicSource.clip.name);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
