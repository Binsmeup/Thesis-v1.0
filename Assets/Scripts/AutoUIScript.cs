using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class AutoUIScript : MonoBehaviour
{
    public MapGenerationShowcase mapGenerationShowcase;

    public Button back;
    public Button restart;

    public TextField floorCount;
    public Label floorLabel;

    public int floorCountValue;
    public int defaultWidthValue;
    public int defaultHeightValue;
    public int defaultDensityValue;
    public int defaultIterationValue;
    public int defaultEnemyCValue;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        mapGenerationShowcase = FindObjectOfType<MapGenerationShowcase>();
        back = root.Q<Button>("L-back");
        restart = root.Q<Button>("restart");
        floorCount = root.Q<TextField>("floorCountInput");


        back.clicked += OnbackClicked;
        restart.clicked += OnRestartClicked;

        floorCount.RegisterCallback<ChangeEvent<string>>(evt => OnFloorcountChanged(evt.newValue));

        if (mapGenerationShowcase != null)
        {
            mapGenerationShowcase.StartCoroutine(mapGenerationShowcase.GenerateMap(true, floorCountValue, defaultWidthValue, defaultHeightValue, defaultDensityValue, defaultIterationValue, defaultEnemyCValue));
        }
        else
        {
            Debug.LogWarning("MapGenerationShowcase not found!");

        }
    }

    public void OnbackClicked()
    {
        SceneManager.LoadScene("Debug");
    }
    public void OnRestartClicked()
    {
        mapGenerationShowcase.StartCoroutine(mapGenerationShowcase.GenerateMap(true, floorCountValue, defaultWidthValue, defaultHeightValue, defaultDensityValue, defaultIterationValue, defaultEnemyCValue));
    }
    public void OnFloorcountChanged(string newText)
    {

        if (int.TryParse(newText, out int newValue))
        {
            floorCountValue = newValue;
            Debug.Log("Floor count value: " + floorCountValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for floor count.");
        }
    }

}
