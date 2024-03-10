using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip backgroundMusic;
    public AudioClip swingSound;
    public AudioClip walkSound;

    // Singleton instance
    public static AudioManager instance;

    private void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ensure AudioManager persists between scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Play background music when the game starts
        PlayBackgroundMusic();
    }

    // Play background music
    public void PlayBackgroundMusic()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music source or background music clip is not set!");
        }
    }

    // Play swing sound effect
    public void PlaySwingSound()
    {
        if (sfxSource != null && swingSound != null)
        {
            sfxSource.PlayOneShot(swingSound);
        }
        else
        {
            Debug.LogWarning("SFX source or swing sound clip is not set!");
        }
    }

    // Play walk sound effect
    public void PlayWalkSound()
    {
        if (sfxSource != null && walkSound != null)
        {
            sfxSource.PlayOneShot(walkSound);
        }
        else
        {
            Debug.LogWarning("SFX source or walk sound clip is not set!");
        }
    }

    // Method to set music volume
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            // Convert the slider value (0 to 1) to the appropriate volume level (0% to 100%)
            musicSource.volume = Mathf.Clamp01(volume / 100f);
        }
    }

    // Set sound effects volume
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            // Convert the slider value (0 to 1) to the appropriate volume level (0% to 100%)
            sfxSource.volume = Mathf.Clamp01(volume / 100f);
        }
    }

    // Method to get the current SFX volume
    public float GetSFXVolume()
    {
        if (sfxSource != null)
        {
            // Return the current volume level (0% to 100%)
            return sfxSource.volume * 100f;
        }
        else
        {
            // If the sfxSource is null, return a default value
            return 100f; // Assuming the default volume is 100%
        }
    }

    public float GetMusicVolume()
    {
        if (musicSource != null)
        {
            // Return the current volume level (0% to 100%)
            return sfxSource.volume * 100f;
        }
        else
        {
            // If the sfxSource is null, return a default value
            return 100f; // Assuming the default volume is 100%
        }
    }
}
