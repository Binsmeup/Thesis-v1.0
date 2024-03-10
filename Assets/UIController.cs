using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    //Main menu 
    public Button playButton;
    public Button LDButton;
    public Button optionButton;
    public Button creditsButton;
    public Button closeButton;
    public VisualElement MainMenu;

    //Leaderbaord
    public VisualElement Leaderboard;
    public Button Lbackmenu;

    //Options
    public VisualElement Options;
    public Button Obackmenu;

    //Credits
    public VisualElement Credits;
    public Button Cbackmenu;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        //Main menu buttons
        playButton = root.Q<Button>("play-button");
        LDButton = root.Q<Button>("LD-button");
        optionButton = root.Q<Button>("options-button");
        creditsButton = root.Q<Button>("credits-button");
        closeButton = root.Q<Button>("close-button");

        //VisualElements
        MainMenu = root.Q<VisualElement>("MainMenu");
        Leaderboard = root.Q<VisualElement>("Leaderboard");
        Options = root.Q<VisualElement>("Options");
        Credits = root.Q<VisualElement>("Credits");

        //Play Game
        playButton.clicked += PlayButtonPressed;


        //Leaderboard Section
        Lbackmenu = root.Q<Button>("L-back");
        LDButton.clicked += LDButtonPressed;
        Lbackmenu.clicked += ShowMainMenu;

        //Options Section
        Obackmenu = root.Q<Button>("O-back");
        optionButton.clicked += OptionsButtonPressed;
        Obackmenu.clicked += ShowMainMenu;

        //Credits Section
        Cbackmenu = root.Q<Button>("C-back");
        creditsButton.clicked += CreditsButtonPressed;
        Cbackmenu.clicked += ShowMainMenu;

        //Close
        closeButton.clicked += CloseButtonPressed;

        ShowMainMenu();
    }

    void ShowMainMenu()
    {
        MainMenu.style.display = DisplayStyle.Flex;
        Leaderboard.style.display = DisplayStyle.None;
        Options.style.display = DisplayStyle.None;
        Credits.style.display = DisplayStyle.None;
    }

    void PlayButtonPressed() {
        SceneManager.LoadScene("GameScene");
    }


    //load Leaderboard
    void LDButtonPressed(){
        MainMenu.style.display = DisplayStyle.None; // Hide main screen
        Leaderboard.style.display = DisplayStyle.Flex; // Show Leaderboard screen
    }

    void OptionsButtonPressed(){
        MainMenu.style.display = DisplayStyle.None; // Hide main screen
        Options.style.display = DisplayStyle.Flex; // Show options screen
    }

    void CreditsButtonPressed(){
        MainMenu.style.display = DisplayStyle.None; // Hide main screen
        Credits.style.display = DisplayStyle.Flex; // Show Credits screen
    }

    void CloseButtonPressed() {
        Application.Quit();
        Debug.Log("Quit!!!");
    
    }
}

