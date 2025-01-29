using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController2DLevel02 : MonoBehaviour
{
    [SerializeField] AudioSource Sounds;
    public AudioClip[] BackgroundSounds;

    void Start()
    {
        AudioClip currentStageMusic = BackgroundSounds[0];
        Sounds.clip = currentStageMusic;
        Sounds.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
