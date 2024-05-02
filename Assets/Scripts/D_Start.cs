using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class D_Start : MonoBehaviour{
    public MapGenerationShowcase mapGenerationShowcase;

    public Button back;
    public Button Auto;
    public Button Manual;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        back = root.Q<Button>("L-back");
        Auto = root.Q<Button>("Aut");
        Manual = root.Q<Button>("Manua");

        back.clicked += OnbackClicked;
        Auto.clicked += OnAutoClicked;
        Manual.clicked += OnManualClicked;
    }

    public void OnbackClicked()
    {
        SceneManager.LoadScene("Main_menu");
    }
    public void OnAutoClicked()
    {
        SceneManager.LoadScene("Auto");
    }
    public void OnManualClicked()
    {
        SceneManager.LoadScene("Manual");
    }


}
