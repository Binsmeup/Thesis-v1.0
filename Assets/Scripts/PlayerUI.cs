using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour{

    // Game Options
    public Button gameOptions;
    public Button gamePlay;
    public Button gameRestart;
    public Button gameMainmenu;
    public Button dieRestart;
    public Button dieMainmenu;


    public Slider music;
    public Slider sfx;

    public VisualElement OptionsScreen;
    public VisualElement layer;
    public VisualElement Die;
    public Label submit;
    public Button send;
    public TextField named;

    //Stats
    public Label attackSpeedLabel;
    public Label baseDamageLabel;
    public Label damageMultiplierLabel;
    public Label criticalChanceLabel;
    public Label criticalDamageLabel;
    public Label knockbackForceLabel;
    public Label moveSpeedLabel;
    public Label coins;
    public Label kills;
    public Label floorcount;
    public Label timed;
    public Label floorFinal;
    public Label killFinal;
    public Label finalTime;
    public Label before;
    public Label homed;
    public Label rest;
    //bars
    public ProgressBar armorBar;
    public ProgressBar healthBar;

    private HealthManager healthManager;
    private playerScript player;
    private MapGeneration mapGeneration;
    private Leaderboard leaderboard;

    public string playerName;
    private float startTime;
    private bool timerRunning = true;
    void Start(){
        var root = GetComponent<UIDocument>().rootVisualElement;

        gameOptions = root.Q<Button>("options");
        gamePlay = root.Q<Button>("play");
        dieRestart = root.Q<Button>("restartD");
        dieMainmenu = root.Q<Button>("homeD");
        gameRestart = root.Q<Button>("restart");
        gameMainmenu = root.Q<Button>("home");
        floorcount = root.Q<Label>("Floor");
        timed = root.Q<Label>("timecount");
        startTime = Time.time;
        coins = root.Q<Label>("coin");
        kills = root.Q<Label>("kill");
        killFinal = root.Q<Label>("KillsFin");
        floorFinal = root.Q<Label>("FloorFin");
        finalTime = root.Q<Label>("TimeFin");
        submit = root.Q<Label>("submitted");
        named = root.Q<TextField>("naming");
        send = root.Q<Button>("check");
        before = root.Q<Label>("sub");
        homed = root.Q<Label>("hom");
        rest = root.Q<Label>("restar");
        send.clicked += SendButtonPressed;


        OptionsScreen = root.Q<VisualElement>("optionsscreen");
        Die = root.Q<VisualElement>("whenudie");
        layer = root.Q<VisualElement>("Layer");

        music = root.Q<Slider>("music");
        sfx = root.Q<Slider>("sound");

        gameOptions.clicked += OptionsButtonPressed;
        gamePlay.clicked += PlayButtonPressed;
        gameRestart.clicked += RestartButtonPressed;
        gameMainmenu.clicked += HomeButtonPressed;
        dieRestart.clicked += DieRestartButtonPressed;
        dieMainmenu.clicked += DieMainmenuButtonPressed;
        music.RegisterValueChangedCallback(OnMusicVolumeChanged);
        sfx.RegisterValueChangedCallback(OnSFXVolumeChanged);

        //stats
        attackSpeedLabel = root.Q<Label>("attackspeed");
        baseDamageLabel = root.Q<Label>("basedamage");
        damageMultiplierLabel = root.Q<Label>("damagemultiplier");
        criticalChanceLabel = root.Q<Label>("criticalchance");
        criticalDamageLabel = root.Q<Label>("criticaldamage");
        knockbackForceLabel = root.Q<Label>("knockbackforce");
        moveSpeedLabel = root.Q<Label>("movespeed");

        //Bar
        armorBar = root.Q<ProgressBar>("armorbar");
        healthBar = root.Q<ProgressBar>("healthbar");
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        GameObject mapGenerationObject = GameObject.FindWithTag("MapGeneration");
        mapGeneration = mapGenerationObject.GetComponent<MapGeneration>();

        GameObject leaderboardManager = GameObject.FindWithTag("Leaderboard");
        leaderboard = leaderboardManager.GetComponent<Leaderboard>();

        if (playerObject != null){
            healthManager = playerObject.GetComponent<HealthManager>();
            player = playerObject.GetComponent<playerScript>();
        }


        // Load the music and SFX volumes from PlayerPrefs
        if (PlayerPrefs.HasKey("MusicVolume")){
            music.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        if (PlayerPrefs.HasKey("SFXVolume")){
            sfx.value = PlayerPrefs.GetFloat("SFXSlider");
        }
    }

    void UpdateUI(){
        if (healthManager != null){
            // Update health bar
            healthBar.title = $"Health: {healthManager.health}/{healthManager.maxHealth}";
            healthBar.lowValue = 0;
            healthBar.highValue = healthManager.maxHealth;
            healthBar.value = healthManager.health;

            // Update armor bar
            float armorValue = healthManager.HelmHP + healthManager.ChestHP + healthManager.LegHP;
            float maxArmorValue = healthManager.HelmMaxHP + healthManager.ChestMaxHP + healthManager.LegMaxHP;
            armorBar.title = $"Armor: {armorValue}/{maxArmorValue}";
            armorBar.lowValue = 0;
            armorBar.highValue = maxArmorValue;
            armorBar.value = armorValue;
        }

        if (player != null){

            attackSpeedLabel.text = "Attack Speed: " + player.attackSpeed.ToString();
            baseDamageLabel.text = "Base Damage: " + player.baseDamage.ToString();
            damageMultiplierLabel.text = "Damage Multiplier: " + player.damageMulti.ToString();
            criticalChanceLabel.text = "Critical Chance: " + player.critChance.ToString();
            criticalDamageLabel.text = "Critical Damage: " + player.critDamage.ToString();
            knockbackForceLabel.text = "Knockback Force: " + player.knockbackForce.ToString();
            moveSpeedLabel.text = "Movement Speed: " + player.moveSpeed.ToString();
            floorcount.text = "Floor: " + mapGeneration.floorCount;
            killFinal.text = "Kills: " + mapGeneration.killCount;
            floorFinal.text = "Floor: " + mapGeneration.floorCount;
            kills.text = mapGeneration.killCount.ToString();
            coins.text = player.coins.ToString();

            if (timerRunning)
            {
                TimeSpan timeElapsed = TimeSpan.FromSeconds(Time.time - startTime);

                string timerText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeElapsed.Hours, timeElapsed.Minutes, timeElapsed.Seconds);

                timed.text = timerText;
            }
        }
    }

    void Update(){
        UpdateUI();

        if (healthManager.health <= 0)
        {
            Time.timeScale = 1f;
            timerRunning = false;
            finalTime.text = "Time: " + timed.text;
            Die.style.display = DisplayStyle.Flex;
        }
        else
        {
            Die.style.display = DisplayStyle.None;
        }
    }


    void OptionsButtonPressed(){
        Time.timeScale = 0f;
        layer.style.display = DisplayStyle.Flex;
        OptionsScreen.style.display = DisplayStyle.Flex;
    }

    void PlayButtonPressed(){
        Time.timeScale = 1f;
        layer.style.display = DisplayStyle.None;
        OptionsScreen.style.display = DisplayStyle.None;
    }

    void RestartButtonPressed(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    void HomeButtonPressed(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main_Menu");
    }
    void DieRestartButtonPressed()
    {
        SceneManager.LoadScene("GameScene");
    }

    void DieMainmenuButtonPressed()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    void SendButtonPressed(){
        string playerName = named.value;
        float maxDamage = player.baseDamage * player.damageMulti;
        float maxArmorValue = healthManager.HelmMaxHP + healthManager.ChestMaxHP + healthManager.LegMaxHP;
        string playerHelm = "None";
        string playerChest = "None";
        string playerLeg = "None";
        string playerWeapon = "None";
        if (player.currentHelm != null){
            playerHelm = player.currentHelm.name;
        }
        if (player.currentChest != null){
            playerChest = player.currentChest.name;
        }
        if (player.currentLeg != null){
            playerLeg = player.currentLeg.name;
        }
        if (player.currentWeapon != null){
            playerWeapon = player.currentWeapon.name;
        }
        string timeText = timed.text;
        string[] timeComponents = timeText.Split(':');
        int hours = int.Parse(timeComponents[0]);
        int minutes = int.Parse(timeComponents[1]);
        int seconds = int.Parse(timeComponents[2]);
        int timeElapsedSeconds = hours * 3600 + minutes * 60 + seconds;
        Debug.Log("Player Name: " + playerName);
        Debug.Log("Player floor: " + mapGeneration.floorCount);
        Debug.Log("Player kill: " + mapGeneration.killCount);
        Debug.Log("Player time: " + timed.text);
        named.SetEnabled(false);
        send.SetEnabled(false);
        leaderboard.addScore(playerName, mapGeneration.killCount, mapGeneration.floorCount, timeElapsedSeconds, healthManager.maxHealth, maxArmorValue, maxDamage, playerHelm, playerChest, playerLeg, playerWeapon);
        leaderboard.printScoresOrderedByName();
        submit.style.display = DisplayStyle.Flex;
        dieMainmenu.style.display = DisplayStyle.Flex;
        dieRestart.style.display = DisplayStyle.Flex;
        homed.style.display = DisplayStyle.Flex;
        rest.style.display = DisplayStyle.Flex;
        before.style.display = DisplayStyle.None;
    }

    // Method to handle music volume slider value change
    void OnMusicVolumeChanged(ChangeEvent<float> evt){
        float volume = evt.newValue;
        AudioManager.BGM.SetMusicVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        Debug.LogWarning(volume);
    }

    // Method to handle sound effects volume slider value change
    void OnSFXVolumeChanged(ChangeEvent<float> evt){
        float volume = evt.newValue;
        AudioManager.BGM.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        Debug.LogWarning(volume);
    }
}