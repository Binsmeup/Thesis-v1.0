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
        music = root.Q<Slider>("sound");
        sfx = root.Q<Slider>("music");

        music.RegisterValueChangedCallback(OnMusicVolumeChanged);
        sfx.RegisterValueChangedCallback(OnSFXVolumeChanged);

        gameOptions.clicked += OptionsButtonPressed;
        gamePlay.clicked += PlayButtonPressed;
        gameRestart.clicked += RestartButtonPressed;
        gameMainmenu.clicked += HomeButtonPressed;

        // Set initial values for sliders based on AudioManager
        music.value = AudioManager.instance.GetMusicVolume() * 100f;
        sfx.value = AudioManager.instance.GetSFXVolume() * 100f;
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

    // Method to handle music volume slider value change
    void OnMusicVolumeChanged(ChangeEvent<float> evt)
    {
        float volume = evt.newValue / 100f; // Convert from 0-100 range to 0-1 range
        AudioManager.instance.SetMusicVolume(volume);
    }

    // Method to handle sound effects volume slider value change
    void OnSFXVolumeChanged(ChangeEvent<float> evt)
    {
        float volume = evt.newValue / 100f; // Convert from 0-100 range to 0-1 range
        AudioManager.instance.SetSFXVolume(volume);
    }

    void HomeButtonPressed()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
