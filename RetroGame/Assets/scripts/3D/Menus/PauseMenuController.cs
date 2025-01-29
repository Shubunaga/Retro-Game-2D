using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{

    [Header("Pause Screen")]
    public GameObject pauseMenuOptionsScreen;

    [Header("Screens")]
    public GameObject[] controlScreens;

    [Header("Volume")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else if (PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSfxVolume();
        }
    }

    private void Update()
    {
        //Debug.Log(Time.timeScale);
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenuOptionsScreen.activeSelf)
        {
            pauseMenuOptionsScreen.SetActive(true);
            controlScreens[0].SetActive(true);
            Time.timeScale = 0;
        }
        /*else if (!controlScreens[1].activeSelf && !controlScreens[2].activeSelf)
        {
            pauseMenuOptionsScreen.SetActive(false);
            Time.timeScale = 1;
        }*/
    }

    public void Resume()
    {
        controlScreens[0].SetActive(false);
        pauseMenuOptionsScreen.SetActive(false);
        Time.timeScale = 1;
    }

    //Controls
    public void ControlsReturnPreviousPage()
    {
        if (controlScreens[1].activeSelf)
        {
            controlScreens[1].SetActive(false);
            controlScreens[0].SetActive(true);
        }
    }

    public void ControlsScreen()
    {
        if (!controlScreens[1].activeSelf)
        {
            controlScreens[0].SetActive(false);
            controlScreens[1].SetActive(true);
        }
    }

    //MainMenu
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu3D");
    }

    //Volumes
    public void VolumeScreen()
    {
        controlScreens[2].SetActive(true);
        controlScreens[0].SetActive(false);
    }

    public void VolumeReturnMenu()
    {
        controlScreens[2].SetActive(false);
        controlScreens[0].SetActive(true);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSfxVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetMusicVolume();
        SetSfxVolume();
    }
}
