using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    private Dictionary<string, VisualElement> itemVisualElements = new Dictionary<string, VisualElement>();
    // Game Options
    public Button gameOptions;
    public Button gamePlay;
    public Button gameRestart;
    public Button gameMainmenu;
    public Button dieRestart;
    public Button dieMainmenu;
    public Button Fbutton;

    //Item Slot
    public Button weapon;
    public Button helmet;
    public Button chestplate;
    public Button legs;

    public Slider music;
    public Slider sfx;

    public VisualElement OptionsScreen;
    public VisualElement layer;
    public VisualElement Die;
    public VisualElement LS2;
    public VisualElement LS4;
    public VisualElement LS5;
    public Label submit;
    public Button send;
    public TextField named;

    //Items
    public VisualElement boxhover; 
    public VisualElement pickup;
    public VisualElement BeginnerSword;
    public VisualElement Sword;
    public VisualElement Spear;
    public VisualElement IronHelmet;
    public VisualElement IronChestplate;
    public VisualElement IronLeggings;
    public VisualElement RingOfStrength;
    public VisualElement RingOfSpeed;
    public VisualElement RingOfHealth;
    public VisualElement DeathGem;
    public VisualElement PrecisionGem;
    public VisualElement SonicGem;
    public VisualElement FrenzyGem;
    public VisualElement TitanGem;
    public VisualElement VitalityNecklace;
    public VisualElement MightyNecklace;
    public VisualElement SwiftyNecklace;
    public VisualElement NimbleNecklace;
    public VisualElement DeadeyeNecklace;
    public VisualElement LethalNecklace;
    public VisualElement RingOfPrecision;
    public VisualElement RingOfFatality;
    public VisualElement RingOfHaste;
    public VisualElement LeatherHood;
    public VisualElement LeatherArmor;
    public VisualElement LeatherPants;
    public VisualElement SteelHelmet;
    public VisualElement SteelChestplate;
    public VisualElement SteelLeggings;
    public VisualElement TitaniumHelmet;
    public VisualElement TitaniumChestplate;
    public VisualElement TitaniumLeggings;
    public VisualElement JuggernautHelmet;
    public VisualElement JuggernautChestplate;
    public VisualElement JuggernautLeggings;
    public VisualElement DarksteelHelmet;
    public VisualElement DarksteelChestplate;
    public VisualElement DarksteelLeggings;
    public VisualElement Axe;
    public VisualElement Bat;
    public VisualElement Dagger;
    public VisualElement RareSword;
    public VisualElement RareSpear;
    public VisualElement RareAxe;
    public VisualElement RareBat;
    public VisualElement RareDagger;
    public VisualElement LegendSword;
    public VisualElement LegendSpear;
    public VisualElement LegendAxe;
    public VisualElement LegendBat;
    public VisualElement LegendDagger;

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
    public Label itemNames;

    //buff
    public Label one;
    public Label two;
    public Label three;
    public Label four;
    public Label five;
    public Label six;
    public Label seven;
    public Label eight;
    public Label oneL;
    public Label twoL;
    public Label threeL;
    public Label fourL;
    public Label fiveL;
    public Label sixL;
    public Label sevenL;
    public Label eightL;
    //bars
    public ProgressBar armorBar;
    public ProgressBar healthBar;

    public Texture2D ExampleTexture;
    private HealthManager healthManager;
    private playerScript player;
    private MapGeneration mapGeneration;
    private Leaderboard leaderboard;
    private ItemManagement itemManage; 

    public Sprite weaponS;
    public Sprite helmS;
    public Sprite chestS;
    public Sprite legS;

    public string weaponC;
    public string helmetC;
    public string chestC;
    public string legsC;

    public string playerName;
    private float startTime;
    private bool timerRunning = true;
    public string previousFloorValue;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        gameOptions = root.Q<Button>("options");
        gamePlay = root.Q<Button>("play");
        dieRestart = root.Q<Button>("restartD");
        dieMainmenu = root.Q<Button>("homeD");
        gameRestart = root.Q<Button>("restart");
        gameMainmenu = root.Q<Button>("home");
        Fbutton = root.Q<Button>("fbutton");
        floorcount = root.Q<Label>("Floor");
        timed = root.Q<Label>("timecount");
        startTime = Time.time;
        itemNames = root.Q<Label>("itemNames");       
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

        //buff
        one = root.Q<Label>("1");
        two = root.Q<Label>("2");
        three = root.Q<Label>("3");
        four = root.Q<Label>("4");
        five = root.Q<Label>("5");
        six = root.Q<Label>("6");
        seven = root.Q<Label>("7");
        eight = root.Q<Label>("8");


        oneL = root.Q<Label>("1l");
        twoL = root.Q<Label>("2l");
        threeL = root.Q<Label>("3l");
        fourL = root.Q<Label>("4l");
        fiveL = root.Q<Label>("5l");
        sixL = root.Q<Label>("6l");
        sevenL = root.Q<Label>("7l");
        eightL = root.Q<Label>("8l");

        //inventory
        weapon = root.Q<Button>("WEAPON");
        helmet = root.Q<Button>("HELMET");
        chestplate = root.Q<Button>("CHESTPLATE");
        legs = root.Q<Button>("LEGS");


        
        //items
        boxhover = root.Q<VisualElement>("itemhover");
        itemVisualElements.Add("BeginnerSword", root.Q<VisualElement>("BeginnerSword"));
        itemVisualElements.Add("Sword", root.Q<VisualElement>("Sword"));
        itemVisualElements.Add("Spear", root.Q<VisualElement>("Spear"));
        itemVisualElements.Add("IronHelmet", root.Q<VisualElement>("IronHelmet"));
        itemVisualElements.Add("IronChestplate", root.Q<VisualElement>("IronChestplate"));
        itemVisualElements.Add("IronLeggings", root.Q<VisualElement>("IronLeggings"));
        itemVisualElements.Add("RingOfStrength", root.Q<VisualElement>("RingOfStrength"));
        itemVisualElements.Add("RingOfSpeed", root.Q<VisualElement>("RingOfSpeed"));
        itemVisualElements.Add("RingOfHealth", root.Q<VisualElement>("RingOfHealth"));
        itemVisualElements.Add("DeathGem", root.Q<VisualElement>("DeathGem"));
        itemVisualElements.Add("PrecisionGem", root.Q<VisualElement>("PrecisionGem"));
        itemVisualElements.Add("SonicGem", root.Q<VisualElement>("SonicGem"));
        itemVisualElements.Add("FrenzyGem", root.Q<VisualElement>("FrenzyGem"));
        itemVisualElements.Add("TitanGem", root.Q<VisualElement>("TitanGem"));
        itemVisualElements.Add("VitalityNecklace", root.Q<VisualElement>("VitalityNecklace"));
        itemVisualElements.Add("MightyNecklace", root.Q<VisualElement>("MightyNecklace"));
        itemVisualElements.Add("SwiftyNecklace", root.Q<VisualElement>("SwiftyNecklace"));
        itemVisualElements.Add("NimbleNecklace", root.Q<VisualElement>("NimbleNecklace"));
        itemVisualElements.Add("DeadeyeNecklace", root.Q<VisualElement>("DeadeyeNecklace"));
        itemVisualElements.Add("LethalNecklace", root.Q<VisualElement>("LethalNecklace"));
        itemVisualElements.Add("RingOfPrecision", root.Q<VisualElement>("RingOfPrecision"));
        itemVisualElements.Add("RingOfFatality", root.Q<VisualElement>("RingOfFatality"));
        itemVisualElements.Add("RingOfHaste", root.Q<VisualElement>("RingOfHaste"));
        itemVisualElements.Add("LeatherHood", root.Q<VisualElement>("LeatherHood"));
        itemVisualElements.Add("LeatherArmor", root.Q<VisualElement>("LeatherArmor"));
        itemVisualElements.Add("LeatherPants", root.Q<VisualElement>("LeatherPants"));
        itemVisualElements.Add("SteelHelmet", root.Q<VisualElement>("SteelHelmet"));
        itemVisualElements.Add("SteelChestplate", root.Q<VisualElement>("SteelChestplate"));
        itemVisualElements.Add("SteelLeggings", root.Q<VisualElement>("SteelLeggings"));
        itemVisualElements.Add("TitaniumHelmet", root.Q<VisualElement>("TitaniumHelmet"));
        itemVisualElements.Add("TitaniumChestplate", root.Q<VisualElement>("TitaniumChestplate"));
        itemVisualElements.Add("TitaniumLeggings", root.Q<VisualElement>("TitaniumLeggings"));
        itemVisualElements.Add("JuggernautHelmet", root.Q<VisualElement>("JuggernautHelmet"));
        itemVisualElements.Add("JuggernautChestplate", root.Q<VisualElement>("JuggernautChestplate"));
        itemVisualElements.Add("JuggernautLeggings", root.Q<VisualElement>("JuggernautLeggings"));
        itemVisualElements.Add("DarksteelHelmet", root.Q<VisualElement>("DarksteelHelmet"));
        itemVisualElements.Add("DarksteelChestplate", root.Q<VisualElement>("DarksteelChestplate"));
        itemVisualElements.Add("DarksteelLeggings", root.Q<VisualElement>("DarksteelLeggings"));
        itemVisualElements.Add("Axe", root.Q<VisualElement>("Axe"));
        itemVisualElements.Add("Bat", root.Q<VisualElement>("Bat"));
        itemVisualElements.Add("Dagger", root.Q<VisualElement>("Dagger"));
        itemVisualElements.Add("RareSword", root.Q<VisualElement>("RareSword"));
        itemVisualElements.Add("RareSpear", root.Q<VisualElement>("RareSpear"));
        itemVisualElements.Add("RareAxe", root.Q<VisualElement>("RareAxe"));
        itemVisualElements.Add("RareBat", root.Q<VisualElement>("RareBat"));
        itemVisualElements.Add("RareDagger", root.Q<VisualElement>("RareDagger"));
        itemVisualElements.Add("LegendSword", root.Q<VisualElement>("LegendSword"));
        itemVisualElements.Add("LegendSpear", root.Q<VisualElement>("LegendSpear"));
        itemVisualElements.Add("LegendAxe", root.Q<VisualElement>("LegendAxe"));
        itemVisualElements.Add("LegendBat", root.Q<VisualElement>("LegendBat"));
        itemVisualElements.Add("LegendDagger", root.Q<VisualElement>("LegendDagger"));

        LS2 = root.Q<VisualElement>("LoadingScreen2");
        LS4 = root.Q<VisualElement>("LoadingScreen4");
        LS5 = root.Q<VisualElement>("LoadingScreen5");
        OptionsScreen = root.Q<VisualElement>("optionsscreen");
        Die = root.Q<VisualElement>("whenudie");
        layer = root.Q<VisualElement>("Layer");

        music = root.Q<Slider>("music");
        sfx = root.Q<Slider>("sound");
        previousFloorValue = floorFinal.text;

        send.clicked += SendButtonPressed;
        gameOptions.clicked += OptionsButtonPressed;
        gamePlay.clicked += PlayButtonPressed;
        gameRestart.clicked += RestartButtonPressed;
        gameMainmenu.clicked += HomeButtonPressed;
        dieRestart.clicked += DieRestartButtonPressed;
        dieMainmenu.clicked += DieMainmenuButtonPressed;
        music.RegisterValueChangedCallback(OnMusicVolumeChanged);
        sfx.RegisterValueChangedCallback(OnSFXVolumeChanged);

        weapon.RegisterCallback<MouseEnterEvent>(OnWeaponHoverEnter);
        weapon.RegisterCallback<MouseLeaveEvent>(OnWeaponHoverLeave);



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

        StartCoroutine(ShowAndHideLoadingScreen2());

        if (playerObject != null)
        {
            healthManager = playerObject.GetComponent<HealthManager>();
            player = playerObject.GetComponent<playerScript>();
            itemManage = FindObjectOfType<ItemManagement>();
        }


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

    public IEnumerator ShowAndHideLoadingScreen2()
    {
        LS2.style.display = DisplayStyle.Flex;


        yield return new WaitForSeconds(2f);

        LS2.style.display = DisplayStyle.None;
    }

    public IEnumerator ShowAndHideLoadingScreen4()
    {
        LS4.style.display = DisplayStyle.Flex;

        yield return new WaitForSeconds(2f);

        LS4.style.display = DisplayStyle.None;
    }
    private IEnumerator LoadMainMenuAfterDelay()
    {
        LS5.style.display = DisplayStyle.Flex;

        yield return new WaitForSeconds(2f);

        LS5.style.display = DisplayStyle.None;

    }

    void UpdateUI()
    {
        if (healthManager != null)
        {
            healthBar.title = $"Health: {healthManager.health}/{healthManager.maxHealth}";
            healthBar.lowValue = 0;
            healthBar.highValue = healthManager.maxHealth;
            healthBar.value = healthManager.health;


            float armorValue = healthManager.HelmHP + healthManager.ChestHP + healthManager.LegHP;
            float maxArmorValue = healthManager.HelmMaxHP + healthManager.ChestMaxHP + healthManager.LegMaxHP;
            armorBar.title = $"Armor: {armorValue}/{maxArmorValue}";
            armorBar.lowValue = 0;
            armorBar.highValue = maxArmorValue;
            armorBar.value = armorValue;
        }

        if (player != null)
        {
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
            weaponC = player.currentWeapon.name;



            weaponS = Resources.Load<Sprite>(weaponC);
            weapon.style.backgroundImage = weaponS.texture;


            if (player.currentChest != null)
            {
                chestC = player.currentChest.name;
                chestplate.style.display = DisplayStyle.Flex;

                chestS = Resources.Load<Sprite>(chestC);
                chestplate.style.backgroundImage = chestS.texture;

                chestplate.RegisterCallback<MouseEnterEvent>(OnChestplateHoverEnter);
                chestplate.RegisterCallback<MouseLeaveEvent>(OnChestplateHoverLeave);
            }
            else
            {
            }

            if (player.currentHelm != null)
            {
                helmetC = player.currentHelm.name;
                helmet.style.display = DisplayStyle.Flex;

                helmS = Resources.Load<Sprite>(helmetC);
                helmet.style.backgroundImage = helmS.texture;

                helmet.RegisterCallback<MouseEnterEvent>(OnHelmetHoverEnter);
                helmet.RegisterCallback<MouseLeaveEvent>(OnHelmetHoverLeave);
            }
            else
            {
            }
            if (player.currentLeg != null)
            {
                legsC = player.currentLeg.name;
                legs.style.display = DisplayStyle.Flex;

                legS = Resources.Load<Sprite>(legsC);
                legs.style.backgroundImage = legS.texture;

                legs.RegisterCallback<MouseEnterEvent>(OnLegsHoverEnter);
                legs.RegisterCallback<MouseLeaveEvent>(OnLegsHoverLeave);
            }
            else
            {
            }


            if (timerRunning)
            {
                TimeSpan timeElapsed = TimeSpan.FromSeconds(Time.time - startTime);

                string timerText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeElapsed.Hours, timeElapsed.Minutes, timeElapsed.Seconds);

                timed.text = timerText;
            }
        }
        
    }
    void Update()
    {
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

        if (floorFinal.text != previousFloorValue)
        {
            StartCoroutine(ShowAndHideLoadingScreen4());
            previousFloorValue = floorFinal.text;
        }

    }



    void OptionsButtonPressed()
    {
        Time.timeScale = 0f;
        layer.style.display = DisplayStyle.Flex;
        OptionsScreen.style.display = DisplayStyle.Flex;
    }

    void PlayButtonPressed()
    {
        Time.timeScale = 1f;
        layer.style.display = DisplayStyle.None;
        OptionsScreen.style.display = DisplayStyle.None;
    }

    void RestartButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    void HomeButtonPressed()
    {

        StartCoroutine(LoadMainMenuAfterDelay());
            Time.timeScale = 1f;
            SceneManager.LoadScene("Main_Menu");
    }
    void DieRestartButtonPressed()
    {
        SceneManager.LoadScene("GameScene");
    }

    void DieMainmenuButtonPressed()
    {
                StartCoroutine(LoadMainMenuAfterDelay());
        SceneManager.LoadScene("Main_Menu");
    }

    void SendButtonPressed()
    {
        string playerName = named.value;
        float maxDamage = player.baseDamage * player.damageMulti;
        float maxArmorValue = healthManager.HelmMaxHP + healthManager.ChestMaxHP + healthManager.LegMaxHP;
        string playerHelm = "None";
        string playerChest = "None";
        string playerLeg = "None";
        string playerWeapon = "None";
        if (player.currentHelm != null)
        {
            playerHelm = player.currentHelm.name;
        }
        if (player.currentChest != null)
        {
            playerChest = player.currentChest.name;
        }
        if (player.currentLeg != null)
        {
            playerLeg = player.currentLeg.name;
        }
        if (player.currentWeapon != null)
        {
            playerWeapon = player.currentWeapon.name;
        }
        string timeText = timed.text;
        string[] timeComponents = timeText.Split(':');
        int hours = int.Parse(timeComponents[0]);
        int minutes = int.Parse(timeComponents[1]);
        int seconds = int.Parse(timeComponents[2]);
        int timeElapsedSeconds = hours * 3600 + minutes * 60 + seconds;
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



    public void ShowItemDetail(string itemName)
    {

        if (itemVisualElements.ContainsKey(itemName))
        {
            itemVisualElements[itemName].style.display = DisplayStyle.Flex;

        }
        else
        {
            Debug.LogError($"VisualElement for item {itemName} not found.");
        }
    }

    public void HideItemDetail(string itemName)
    {
            itemVisualElements[itemName].style.display = DisplayStyle.None;
    }

    public void ShowFButton()
    {
        Fbutton.style.display = DisplayStyle.Flex;
        boxhover.style.display = DisplayStyle.Flex;
    }

    public void HideFButton()
    {
        Fbutton.style.display = DisplayStyle.None;
        boxhover.style.display = DisplayStyle.None;
    }
    public void SetItemName(string name)
    {
        itemNames.text = name;
    }

    // Method to clear the item name from the UI
    public void ClearItemName()
    {
        itemNames.text = "";
    }

    public void OnWeaponHoverEnter(MouseEnterEvent evt)
    {

        ShowItemDetail(weaponC);

    }

    void OnWeaponHoverLeave(MouseLeaveEvent evt)
    {
        HideItemDetail(weaponC);

    }

    void OnHelmetHoverEnter(MouseEnterEvent evt)
    {
        ShowItemDetail(helmetC);
    }

    void OnHelmetHoverLeave(MouseLeaveEvent evt)
    {
        HideItemDetail(helmetC);
    }

    void OnChestplateHoverEnter(MouseEnterEvent evt)
    {
        ShowItemDetail(chestC);
    }

    void OnChestplateHoverLeave(MouseLeaveEvent evt)
    {
        HideItemDetail(chestC);
    }

    void OnLegsHoverEnter(MouseEnterEvent evt)
    {
        ShowItemDetail(legsC);
    }

    void OnLegsHoverLeave(MouseLeaveEvent evt)
    {
        HideItemDetail(legsC);
    }

    public void OnLabelHoverEnter(MouseEnterEvent evt, string labelName)
    {

    switch (labelName)
        {
        case "1":
                ShowItemDetail(itemManage.item_one);
                break;
        case "2":
                ShowItemDetail(itemManage.item_two);
                break;
        case "3":
                ShowItemDetail(itemManage.item_three);
                break;
        case "4":
                ShowItemDetail(itemManage.item_four);
                break;
        case "5":
                ShowItemDetail(itemManage.item_five);
                break;
        case "6":
                ShowItemDetail(itemManage.item_six);
                break;
        case "7":
                ShowItemDetail(itemManage.item_seven);
                break;
        case "8":
                ShowItemDetail(itemManage.item_eight);
                break;
        default:
                break;
        }
    }

    public void OnLabelHoverLeave(MouseLeaveEvent evt, string labelName)
    {
    switch (labelName)
    {
        case "1":
                HideItemDetail(itemManage.item_one);
                break;
        case "2":
                HideItemDetail(itemManage.item_two);
                break;
        case "3":
                HideItemDetail(itemManage.item_three);
                break;
        case "4":
                HideItemDetail(itemManage.item_four);
                break;
        case "5":
                HideItemDetail(itemManage.item_five);
                break;
        case "6":
                HideItemDetail(itemManage.item_six);
                break;
        case "7":
                HideItemDetail(itemManage.item_seven);
                break;
        case "8":
                HideItemDetail(itemManage.item_eight);
                break;
        default:
                break;
    }
    }



    // Method to handle music volume slider value change
    void OnMusicVolumeChanged(ChangeEvent<float> evt)
    {
        float volume = evt.newValue;
        AudioManager.BGM.SetMusicVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    // Method to handle sound effects volume slider value change
    void OnSFXVolumeChanged(ChangeEvent<float> evt)
    {
        float volume = evt.newValue;
        AudioManager.BGM.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}