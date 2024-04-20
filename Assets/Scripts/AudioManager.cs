using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip BGMMain;
    public AudioClip BGMGame;
    public AudioClip swingSound;
    public AudioClip walkSound;

    public static AudioManager BGM;

    private void Awake()
    {
        if (BGM == null)
        {
            BGM = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    // Play background music
    public void PlayBackgroundMusic()
    {
        if (musicSource != null)
        {
            // Load appropriate background music based on the scene name
            string sceneName = SceneManager.GetActiveScene().name;


            if (sceneName == "Main_menu")
            {
                musicSource.clip = BGMMain;
            }
            else if (sceneName == "GameScene")
            {
                musicSource.clip = BGMGame;
            }

            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music source is not set!");
        }
    }

    // Method called when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play background music when a new scene is loaded
        PlayBackgroundMusic();
    }

    // Play swing sound effect
    public void PlaySwingSound()
    {
        if (sfxSource != null && swingSound != null)
        {
            sfxSource.clip = swingSound;
            sfxSource.Play();
        }
        else
        {
            Debug.LogWarning("SFX source or swing sound clip is not set!");
        }
    }



    public void PlayWalkSoundLoop()
    {
        if (sfxSource != null && walkSound != null)
        {
            sfxSource.clip = walkSound;
            sfxSource.Play();
        }
        else
        {
            Debug.LogWarning("SFX source or walk sound clip is not set!");
        }
    }

    public void StopWalkSoundLoop()
    {
        sfxSource.Stop();
    }

    // Method to set music volume
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {

            musicSource.volume = Mathf.Clamp01(volume / 100f);
        }
    }

    // Set sound effects volume
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = Mathf.Clamp01(volume / 100f);
        }
    }

    // Method to get the current SFX volume
    public float GetSFXVolume()
    {
        if (sfxSource != null)
        {
            return sfxSource.volume * 100f;
        }
        else
        {

            return 100f;
        }
    }

    public float GetMusicVolume()
    {
        if (musicSource != null)
        {
            return musicSource.volume * 100f;
        }
        else
        {
  
            return 100f; 
        }
    }
}
