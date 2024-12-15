using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System;
using System.Data;

public class UIController : MonoBehaviour
{
    // Main menu buttons
    public Button playButton;
    public Button LDButton;
    public Button optionButton;
    public Button creditsButton;
    public Button closeButton;
    public Button debug;
    public VisualElement MainMenu;

    // Leaderboard
    public VisualElement Leaderboard;
    public Button Lbackmenu;


    public List<Label> nameLabels;
    public List<Label> floorLabels;
    public List<Label> timeLabels;
    public List<Label> killCountLabels;
    //Sort
    public Button NAMES;
    public Button FLOORS;
    public Button TIME;
    public Button KILLCOUNT;
    //INFO
    public VisualElement details;
    public Label nem;
    public Label kill;
    public Label floor;
    public Label run;
    public Label time;
    public Label health;
    public Label armor;
    public Label damage;
    public Label helm;
    public Label chest;
    public Label legs;
    public Label weapon;

    private Leaderboard leaderboard;

    //Options
    public VisualElement Options;
    public Button Obackmenu;
    public Slider music;
    public Slider sfx;

    //Credits
    public VisualElement Credits;
    public Button Cbackmenu;


    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        InitializeUI(root);

        leaderboard = FindObjectOfType<Leaderboard>();

        //Options Section
        Obackmenu = root.Q<Button>("O-back");
        music = root.Q<Slider>("MusicSlider");
        sfx = root.Q<Slider>("SFXSlider");
        music.RegisterValueChangedCallback(OnMusicVolumeChanged);
        sfx.RegisterValueChangedCallback(OnSFXVolumeChanged);

        // Attach button click event listeners
        playButton.clicked += PlayButtonPressed;
        LDButton.clicked += LDButtonPressed;
        Lbackmenu.clicked += ShowMainMenu;
        optionButton.clicked += OptionsButtonPressed;
        closeButton.clicked += CloseButtonPressed;
        debug.clicked += DebugPressed;
        Obackmenu.clicked += ShowMainMenu;
        LDButton.clicked += LDButtonPressed;
        Lbackmenu.clicked += ShowMainMenu;
        creditsButton.clicked += CreditsButtonPressed;
        Cbackmenu.clicked += ShowMainMenu;

        NAMES.clicked += SortByName;
        FLOORS.clicked += SortByFloor;
        TIME.clicked += SortByTime;
        KILLCOUNT.clicked += SortByKillCount;


        ShowMainMenu();
    }

    void InitializeUI(VisualElement root)
    {
        //Main menu buttons
        playButton = root.Q<Button>("play-button");
        LDButton = root.Q<Button>("LD-button");
        optionButton = root.Q<Button>("options-button");
        creditsButton = root.Q<Button>("credits-button");
        closeButton = root.Q<Button>("close-button");
        debug = root.Q<Button>("debug-button");
        Cbackmenu = root.Q<Button>("C-back");

        //Sort buttons
        NAMES = root.Q<Button>("Name");
        FLOORS = root.Q<Button>("Floor");
        TIME = root.Q<Button>("Time");
        KILLCOUNT = root.Q<Button>("KillCount");

        //VisualElements
        MainMenu = root.Q<VisualElement>("MainMenu");
        Leaderboard = root.Q<VisualElement>("Leaderboard");
        Options = root.Q<VisualElement>("Options");
        Credits = root.Q<VisualElement>("Credits");
        MainMenu = root.Q<VisualElement>("MainMenu");
        Leaderboard = root.Q<VisualElement>("Leaderboard");

        nameLabels = new List<Label>();
        floorLabels = new List<Label>();
        timeLabels = new List<Label>();
        killCountLabels = new List<Label>();

        //dets
        details = root.Q<VisualElement>("Details");
        nem = root.Q<Label>("t1");
        kill = root.Q<Label>("t2");
        floor = root.Q<Label>("t3");
        time = root.Q<Label>("t4");
        health = root.Q<Label>("t5");
        armor = root.Q<Label>("t6");
        damage = root.Q<Label>("t7");
        helm = root.Q<Label>("t8");
        chest = root.Q<Label>("t9");
        legs = root.Q<Label>("t10");
        weapon = root.Q<Label>("t11");
        //Leaderboard Section
        Lbackmenu = root.Q<Button>("L-back");

        for (int i = 1; i <= 9; i++)
        {
            nameLabels.Add(root.Q<Label>($"Name{i}"));
            floorLabels.Add(root.Q<Label>($"Floor{i}"));
            timeLabels.Add(root.Q<Label>($"Time{i}"));
            killCountLabels.Add(root.Q<Label>($"KillCount{i}"));
        }

        foreach (var nameLabel in nameLabels)
        {
            nameLabel.RegisterCallback<MouseEnterEvent>(evt => OnNameLabelHoverEnter(nameLabel.text));
            nameLabel.RegisterCallback<MouseLeaveEvent>(evt => OnNameLabelHoverLeave());
        }

    }

    void PlayButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    void LDButtonPressed()
    {
        MainMenu.style.display = DisplayStyle.None;
        Leaderboard.style.display = DisplayStyle.Flex;
        UpdateLeaderboardUI();
    }

    void OptionsButtonPressed()
    {
        MainMenu.style.display = DisplayStyle.None;
        Options.style.display = DisplayStyle.Flex;
    }

    void CreditsButtonPressed()
    {
        MainMenu.style.display = DisplayStyle.None;
        Credits.style.display = DisplayStyle.Flex;
    }

    void CloseButtonPressed()
    {
        Application.Quit();
        Debug.Log("Quit!!!");
    }

    void DebugPressed()
    {
        SceneManager.LoadScene("Debug");
    }

    void ShowMainMenu()
    {
        MainMenu.style.display = DisplayStyle.Flex;
        Leaderboard.style.display = DisplayStyle.None;
        Options.style.display = DisplayStyle.None;
        Credits.style.display = DisplayStyle.None;
    }


    void OnMusicVolumeChanged(ChangeEvent<float> evt)
    {
        float volume = evt.newValue;
        AudioManager.BGM.SetMusicVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    void OnSFXVolumeChanged(ChangeEvent<float> evt)
    {
        float volume = evt.newValue;
        AudioManager.BGM.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }


    void SortByName()
    {
        List<Leaderboard.LeaderboardEntry> leaderboardEntries = leaderboard.OrderedByName();

        for (int i = 0; i < leaderboardEntries.Count && i < nameLabels.Count; i++)
        {
            nameLabels[i].text = leaderboardEntries[i].name;
            floorLabels[i].text = leaderboardEntries[i].floorCount.ToString();
            timeLabels[i].text = leaderboardEntries[i].timeCount.ToString();
            killCountLabels[i].text = leaderboardEntries[i].killCount.ToString();
        }
    }

    void SortByFloor()
    {
        List<Leaderboard.LeaderboardEntry> leaderboardEntries = leaderboard.OrderedByFloor();

        for (int i = 0; i < leaderboardEntries.Count && i < nameLabels.Count; i++)
        {
            nameLabels[i].text = leaderboardEntries[i].name;
            floorLabels[i].text = leaderboardEntries[i].floorCount.ToString();
            timeLabels[i].text = leaderboardEntries[i].timeCount.ToString();
            killCountLabels[i].text = leaderboardEntries[i].killCount.ToString();
        }
    }

    void SortByTime()
    {
        List<Leaderboard.LeaderboardEntry> leaderboardEntries = leaderboard.OrderedByTime();

        for (int i = 0; i < leaderboardEntries.Count && i < nameLabels.Count; i++)
        {
            nameLabels[i].text = leaderboardEntries[i].name;
            floorLabels[i].text = leaderboardEntries[i].floorCount.ToString();
            timeLabels[i].text = leaderboardEntries[i].timeCount.ToString();
            killCountLabels[i].text = leaderboardEntries[i].killCount.ToString();
        }
    }
    void OnNameLabelHoverEnter(string playerName)
    {
        details.style.display = DisplayStyle.Flex;
        leaderboard.printScoresByName(playerName, this);
    }
    void OnNameLabelHoverLeave()
    {
        details.style.display = DisplayStyle.None;
    }

    void SortByKillCount()
    {
        List<Leaderboard.LeaderboardEntry> leaderboardEntries = leaderboard.OrderedByKillCount();

        for (int i = 0; i < leaderboardEntries.Count && i < nameLabels.Count; i++)
        {
            nameLabels[i].text = leaderboardEntries[i].name;
            floorLabels[i].text = leaderboardEntries[i].floorCount.ToString();
            timeLabels[i].text = leaderboardEntries[i].timeCount.ToString();
            killCountLabels[i].text = leaderboardEntries[i].killCount.ToString();
        }

    }

    void UpdateLeaderboardUI()
    {
        leaderboard.printScores();
        List<Leaderboard.LeaderboardEntry> leaderboardEntries = leaderboard.OrderedByName();

        for (int i = 0; i < leaderboardEntries.Count && i < nameLabels.Count; i++)
        {
            nameLabels[i].text = leaderboardEntries[i].name;
            floorLabels[i].text = leaderboardEntries[i].floorCount.ToString();
            timeLabels[i].text = leaderboardEntries[i].timeCount.ToString();
            killCountLabels[i].text = leaderboardEntries[i].killCount.ToString();
        }
    }
}
