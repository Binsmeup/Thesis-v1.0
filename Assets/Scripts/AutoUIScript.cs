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


        back.clicked += OnbackClicked;
        restart.clicked += OnRestartClicked;

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



}
