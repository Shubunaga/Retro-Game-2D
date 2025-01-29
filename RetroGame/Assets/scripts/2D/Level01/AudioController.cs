using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip jump,jumpInEnemySFX, Trampoline, Coin, ExtraPoints, FireTraps, SawTraps, deathSFX, bgm, bgm02,
                      shooting, phantomDeath, bossAttack, bossHit, bossDeath, powerUpPick;
    public static AudioController current;
    
    private AudioSource audioSource;


    void Start()
    {
        current = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
