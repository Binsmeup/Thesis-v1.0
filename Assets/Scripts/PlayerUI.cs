using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    // Game Options
    public Button gameOptions;
    public Button gamePlay;
    public Button gameRestart;
    public Button gameMainmenu;

    public Slider music;
    public Slider sfx;

    public VisualElement OptionsScreen;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Game Menu
        gameOptions = root.Q<Button>("options");
        gamePlay = root.Q<Button>("play");
        gameRestart = root.Q<Button>("restart");
        gameMainmenu = root.Q<Button>("home");
        OptionsScreen = root.Q<VisualElement>("optionsscreen");
        music = root.Q<Slider>("music");
        sfx = root.Q<Slider>("sound");


        gameOptions.clicked += OptionsButtonPressed;
        gamePlay.clicked += PlayButtonPressed;
        gameRestart.clicked += RestartButtonPressed;
        gameMainmenu.clicked += HomeButtonPressed;
        music.RegisterValueChangedCallback(OnMusicVolumeChanged);
        sfx.RegisterValueChangedCallback(OnSFXVolumeChanged);


        // Load the music and SFX volumes from PlayerPrefs
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            music.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfx.value = PlayerPrefs.GetFloat("SFXVolume");
        }
    }

    // Update is called once per frame
    void OptionsButtonPressed()
    {
        OptionsScreen.style.display = DisplayStyle.Flex;
    }

    void PlayButtonPressed()
    {
        OptionsScreen.style.display = DisplayStyle.None;
    }

    void RestartButtonPressed()
    {
        SceneManager.LoadScene("GameScene");
    }


    void HomeButtonPressed()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    // Method to handle music volume slider value change
    void OnMusicVolumeChanged(ChangeEvent<float> evt)
    {
        float volume = evt.newValue;
        AudioManager.BGM.SetMusicVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        Debug.LogWarning(volume);
    }

    // Method to handle sound effects volume slider value change
    void OnSFXVolumeChanged(ChangeEvent<float> evt)
    {
        float volume = evt.newValue;
        AudioManager.BGM.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        Debug.LogWarning(volume);
    }
}
