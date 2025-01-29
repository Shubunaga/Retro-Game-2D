using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    //BackGround Items
    [Header("BackGround Itens")]
    public Button[] changeBackGroundVideoButtons;
    public VideoPlayer backGroundVideoPlayer; // Drag your VideoPlayer here in inspector
    public VideoClip[] backGroundVideoClips; // Assign your VideoClip here in inspector
    public RawImage backgroundImage;
    public GameObject[] gameImages;
    public AudioSource selectExample;
    public AudioSource selectOption;

    [Header("Menu Options Screens")]
    public GameObject[] mainMenuOptionsScreens;

    [Header("Controls")]
    public GameObject[] controlScreens;

    [Header("Volume")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        //backGroundVideoPlayer.clip = backGroundVideoClips[0];
        //backGroundVideoPlayer.Play();
        //Listers
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }else if (PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSfxVolume();
        }
        backgroundImage.enabled = false;
        InstantiateBackGroundListeners();
        gameImages[0].SetActive(true);
        //InstantiatChangeSizingOfButtons();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectSound()
    {
        selectOption.Play();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Nivel 1 - 3D");
    }

    //Menu Options
    public void Controls()
    {
        mainMenuOptionsScreens[1].SetActive(true);
        mainMenuOptionsScreens[0].SetActive(false);
        controlScreens[0].SetActive(true);
    }

    public void Volume()
    {
        mainMenuOptionsScreens[2].SetActive(true);
        mainMenuOptionsScreens[0].SetActive(false);
    }

    public void About()
    {
        mainMenuOptionsScreens[3].SetActive(true);
        mainMenuOptionsScreens[0].SetActive(false);
    }
    
    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    //Controls
    public void ControlsReturnPreviousPage()
    {
        if (controlScreens[0].activeSelf)
        {
            controlScreens[0].SetActive(false);
            mainMenuOptionsScreens[0].SetActive(true);
            mainMenuOptionsScreens[1].SetActive(false);
        }else if(controlScreens[1].activeSelf)
        {
            controlScreens[1].SetActive(false);
            controlScreens[0].SetActive(true);
        }else
        {
            controlScreens[2].SetActive(false);
            controlScreens[1].SetActive(true);
        }
    }

    public void ControlsGoNextPage()
    {
        if (controlScreens[0].activeSelf)
        {
            controlScreens[0].SetActive(false);
            controlScreens[1].SetActive(true);
        }
        else if (controlScreens[1].activeSelf)
        {
            controlScreens[1].SetActive(false);
            controlScreens[2].SetActive(true);
        }
    }

    //Volumes
    public void VolumeReturnMenu()
    {
        mainMenuOptionsScreens[0].SetActive(true);
        mainMenuOptionsScreens[2].SetActive(false);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSfxVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetMusicVolume();
        SetSfxVolume();
    }

    //About
    public void AboutReturnMenu()
    {
        mainMenuOptionsScreens[0].SetActive(true);
        mainMenuOptionsScreens[3].SetActive(false);
    }


    void InstantiateBackGroundListeners()
    {
        for (int i = 0; i < changeBackGroundVideoButtons.Length; i++)
        {
            int index = i; // To avoid the closure problem in C#
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => { DisableImage(); ChangeBackGroundVideo(backGroundVideoClips[index], gameImages[index]); });

            EventTrigger trigger = changeBackGroundVideoButtons[i].gameObject.AddComponent<EventTrigger>();
            trigger.triggers.Add(entry);
        }
    }

    void DisableImage()
    {
        foreach(GameObject objetos in gameImages)
        {
            objetos.SetActive(false);
        }
    }
    void ChangeBackGroundVideo(VideoClip clip, GameObject image)
    {
        image.SetActive(true);
        backGroundVideoPlayer.Stop();
        backGroundVideoPlayer.clip = clip;
        backGroundVideoPlayer.isLooping = true;
        backGroundVideoPlayer.Play();
        selectExample.Play();
        backgroundImage.enabled = false;
        //backGroundVideoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        backgroundImage.enabled = true;
    }

}
