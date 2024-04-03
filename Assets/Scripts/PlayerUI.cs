using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour{
    // Game Options
    public Button gameOptions;
    public Button gamePlay;
    public Button gameRestart;
    public Button gameMainmenu;

    public Slider music;
    public Slider sfx;

    public VisualElement OptionsScreen;

    //Stats
    public Label attackSpeedLabel;
    public Label baseDamageLabel;
    public Label damageMultiplierLabel;
    public Label criticalChanceLabel;
    public Label criticalDamageLabel;
    public Label knockbackForceLabel;
    public Label moveSpeedLabel;

    //bars
    public ProgressBar armorBar;
    public ProgressBar healthBar;


    private HealthManager healthManager;
    private playerScript player;

    void Start(){
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

        //stats
        attackSpeedLabel = root.Q<Label>("attackspeed");
        baseDamageLabel = root.Q<Label>("basedamage");
        damageMultiplierLabel = root.Q<Label>("damagemultiplier");
        criticalChanceLabel = root.Q<Label>("criticalchance");
        criticalDamageLabel = root.Q<Label>("criticaldamage");
        knockbackForceLabel = root.Q<Label>("knockbackforce");
        moveSpeedLabel = root.Q<Label>("movespeed");

        player = GameObject.FindObjectOfType<playerScript>();


        //Bar
        armorBar = root.Q<ProgressBar>("armorbar");
        healthBar = root.Q<ProgressBar>("healthbar");
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null){
            healthManager = playerObject.GetComponent<HealthManager>();
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


        }
    }

    // Update is called once per frame
    void Update(){
        UpdateUI();
    }

    // Update is called once per frame
    void OptionsButtonPressed(){
        OptionsScreen.style.display = DisplayStyle.Flex;
    }

    void PlayButtonPressed(){
        OptionsScreen.style.display = DisplayStyle.None;
    }

    void RestartButtonPressed(){
        SceneManager.LoadScene("GameScene");
    }

    void HomeButtonPressed(){
        SceneManager.LoadScene("Main_Menu");
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