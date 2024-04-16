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

    //bars
    public ProgressBar armorBar;
    public ProgressBar healthBar;

    private HealthManager healthManager;
    private playerScript player;

    public string playerName;

    void Start(){
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Game Menu
        gameOptions = root.Q<Button>("options");
        gamePlay = root.Q<Button>("play");
        dieRestart = root.Q<Button>("restartD");
        dieMainmenu = root.Q<Button>("homeD");
        gameRestart = root.Q<Button>("restart");
        gameMainmenu = root.Q<Button>("home");
        submit = root.Q<Label>("submitted");
        named = root.Q<TextField>("naming");
        send = root.Q<Button>("check");
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

    void Update(){
        UpdateUI();

        if (healthManager.health <= 0)
        {
            Debug.Log("Health is 0 or less");
            Time.timeScale = 1f;
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

    void SendButtonPressed()
    {
        string playerName = named.value;
        Debug.Log("Player Name: " + playerName);
        named.SetEnabled(false);
        send.SetEnabled(false);
        submit.style.display = DisplayStyle.Flex;
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