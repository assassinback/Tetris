using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerZia : MonoBehaviour
{
    public static SoundManagerZia _instance;
    public AudioSource audioSource;
    public AudioSource backgroundMusic;
    public AudioClip cardCompleteSound;
    public AudioClip buttonClickSound;
    public bool toggle = true;
    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        //audioSource = this.GetComponent<AudioSource>();
    }
    public void PlayCardCompletedSound()
    {
        audioSource.clip = cardCompleteSound;
        audioSource.Play();
    }
    public void PlayButtonClickSound()
    {
        audioSource.clip = buttonClickSound;
        audioSource.Play();
    }
    public void StopAllAudio()
    {
        toggle = !toggle;

        if (toggle)
            AudioListener.volume = 1f;

        else
            AudioListener.volume = 0f;
    }
    public void StopBackgroundMusic()
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }
        else
        {
            backgroundMusic.Play();
        }
    }
}
