using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DebugUI : MonoBehaviour
{
    public MapGenerationShowcase mapGenerationShowcase;

    //Buttons
    public Button GMnoFR;
    public Button GMwithFR;
    public Button cleartiles;
    public Button varset;
    public Button creategrid;
    public Button genNoise;
    public Button applyCA;
    public Button floodfill;
    public Button applyPN;
    public Button movewalls;
    public Button positionPlayer;
    

    //TextField
    public TextField floorcount;
    public TextField defaultWidth;
    public TextField defaultHeight;
    public TextField defaultDensity;
    public TextField defaultIteration;
    public TextField defaultEnemyC;


    //Toggle
    public Toggle noise_CD;
    public Toggle cellular_CD;
    public Toggle perlin_CD;
    public Toggle CD_iteration;
    public Toggle CD;

    public int floorCountValue;
    public int defaultWidthValue;
    public int defaultHeightValue;
    public int defaultDensityValue;
    public int defaultIterationValue;
    public int defaultEnemyCValue;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        GMnoFR = root.Q<Button>("generateMap-without-FR");
        GMwithFR = root.Q<Button>("generateMap-with-FR");
        cleartiles = root.Q<Button>("ClearTiles");
        varset = root.Q<Button>("VariableSetUp");
        creategrid = root.Q<Button>("CreateGrid");
        genNoise = root.Q<Button>("GenerateNoise");
        applyCA = root.Q<Button>("ApplyCellularAutomata");
        floodfill = root.Q<Button>("FloodFill");
        applyPN = root.Q<Button>("ApplyPerlinNoise");
        movewalls = root.Q<Button>("MoveWalls");
        positionPlayer = root.Q<Button>("PositionPlayer");

        floorcount = root.Q<TextField>("floorCountInput");
        defaultWidth = root.Q<TextField>("defaultWidthInput");
        defaultHeight = root.Q<TextField>("defaultHeightInput");
        defaultDensity = root.Q<TextField>("defaultDensityInput");
        defaultIteration = root.Q<TextField>("defaultIterationInput");
        defaultEnemyC = root.Q<TextField>("defaultEnemyCountInput");

        noise_CD = root.Q<Toggle>("cooldownOnNoise");
        cellular_CD = root.Q<Toggle>("cooldownOnCellular");
        perlin_CD = root.Q<Toggle>("cooldownOnPerlin");
        CD_iteration = root.Q<Toggle>("cooldownOnIteration");
        CD = root.Q<Toggle>("cooldown");

        GMnoFR.clicked += OnGMnoFRClicked;
        GMwithFR.clicked += OnGMwithFRClicked;
        cleartiles.clicked += OnCleartilesClicked;
        varset.clicked += OnVarsetClicked;
        creategrid.clicked += OnCreategridClicked;
        genNoise.clicked += OnGenNoiseClicked;
        applyCA.clicked += OnApplyCAClicked;
        floodfill.clicked += OnFloodfillClicked;
        applyPN.clicked += OnApplyPNClicked;
        movewalls.clicked += OnMovewallsClicked;
        positionPlayer.clicked += OnPositionPlayerClicked;

        floorcount.RegisterCallback<ChangeEvent<string>>(evt => OnFloorcountChanged(evt.newValue));
        defaultWidth.RegisterCallback<ChangeEvent<string>>(evt => OnDefaultWidthChanged(evt.newValue));
        defaultHeight.RegisterCallback<ChangeEvent<string>>(evt => OnDefaultHeightChanged(evt.newValue));
        defaultDensity.RegisterCallback<ChangeEvent<string>>(evt => OnDefaultDensityChanged(evt.newValue));
        defaultIteration.RegisterCallback<ChangeEvent<string>>(evt => OnDefaultIterationChanged(evt.newValue));
        defaultEnemyC.RegisterCallback<ChangeEvent<string>>(evt => OnDefaultEnemyCChanged(evt.newValue));

        noise_CD.RegisterValueChangedCallback(evt => OnNoiseCDChanged(evt.newValue));
        cellular_CD.RegisterValueChangedCallback(evt => OnCellularCDChanged(evt.newValue));
        perlin_CD.RegisterValueChangedCallback(evt => OnPerlinCDChanged(evt.newValue));
        CD_iteration.RegisterValueChangedCallback(evt => OnCDIterationChanged(evt.newValue));
        CD.RegisterValueChangedCallback(evt => OnCDChanged(evt.newValue));

        mapGenerationShowcase = FindObjectOfType<MapGenerationShowcase>();
        EnableButtons();
    }


    void OnGMnoFRClicked()
    {
        if (mapGenerationShowcase != null)
        {
            mapGenerationShowcase.StartCoroutine(mapGenerationShowcase.GenerateMap());//not sure what to do here
            DisableButtons();
        }
        else
        {
            Debug.LogWarning("MapGenerationShowcase not found!");
        }
    }


    void OnGMwithFRClicked()
    {
        mapGenerationShowcase.StartCoroutine(mapGenerationShowcase.GenerateMap());//not sure what to do here
        DisableButtons();

    }

    void OnCleartilesClicked()
    {

        mapGenerationShowcase.ClearTiles();
        EnableButtons();
    }

    void OnVarsetClicked()
    {
        mapGenerationShowcase.VariableSetUp();
    }

    void OnCreategridClicked()
    {
        mapGenerationShowcase.CreateGrid();
        creategrid.SetEnabled(false);
        genNoise.SetEnabled(true);
        applyCA.SetEnabled(false);
        floodfill.SetEnabled(false);
        applyPN.SetEnabled(false);
        movewalls.SetEnabled(false);
        positionPlayer.SetEnabled(false);
    }

    void OnGenNoiseClicked()
    {
        mapGenerationShowcase.GenerateNoise();
        creategrid.SetEnabled(false);
        genNoise.SetEnabled(false);
        applyCA.SetEnabled(true);
        floodfill.SetEnabled(false);
        applyPN.SetEnabled(false);
        movewalls.SetEnabled(false);
        positionPlayer.SetEnabled(false);
    }

    void OnApplyCAClicked()
    {
        mapGenerationShowcase.ApplyCellularAutomata();
        creategrid.SetEnabled(false);
        genNoise.SetEnabled(false);
        applyCA.SetEnabled(false);
        floodfill.SetEnabled(true);
        applyPN.SetEnabled(false);
        movewalls.SetEnabled(false);
        positionPlayer.SetEnabled(false);
    }

    void OnFloodfillClicked()
    {
        mapGenerationShowcase.FloodFill();
        creategrid.SetEnabled(false);
        genNoise.SetEnabled(false);
        applyCA.SetEnabled(false);
        floodfill.SetEnabled(false);
        applyPN.SetEnabled(true);
        movewalls.SetEnabled(false);
        positionPlayer.SetEnabled(false);
    }

    void OnApplyPNClicked()
    {
        mapGenerationShowcase.ApplyPerlinNoise();
        creategrid.SetEnabled(false);
        genNoise.SetEnabled(false);
        applyCA.SetEnabled(false);
        floodfill.SetEnabled(false);
        applyPN.SetEnabled(false);
        movewalls.SetEnabled(true);
        positionPlayer.SetEnabled(false);
    }

    void OnMovewallsClicked()
    {
        mapGenerationShowcase.MoveWalls();
        creategrid.SetEnabled(false);
        genNoise.SetEnabled(false);
        applyCA.SetEnabled(false);
        floodfill.SetEnabled(false);
        applyPN.SetEnabled(false);
        movewalls.SetEnabled(false);
        positionPlayer.SetEnabled(true);
    }

    void OnPositionPlayerClicked()
    {
        mapGenerationShowcase.PositionPlayer();
        creategrid.SetEnabled(false);
        genNoise.SetEnabled(false);
        applyCA.SetEnabled(false);
        floodfill.SetEnabled(false);
        applyPN.SetEnabled(false);
        movewalls.SetEnabled(false);
        positionPlayer.SetEnabled(false);
    }
    public void DisableButtons()
    {
        GMnoFR.SetEnabled(false);
        GMwithFR.SetEnabled(false);
        creategrid.SetEnabled(false);
        genNoise.SetEnabled(false);
        applyCA.SetEnabled(false);
        floodfill.SetEnabled(false);
        applyPN.SetEnabled(false);
        movewalls.SetEnabled(false);
        positionPlayer.SetEnabled(false);
    }

    public void EnableButtons()
    {
        GMnoFR.SetEnabled(true);
        GMwithFR.SetEnabled(true);
        creategrid.SetEnabled(true);
        genNoise.SetEnabled(false);
        applyCA.SetEnabled(false);
        floodfill.SetEnabled(false);
        applyPN.SetEnabled(false);
        movewalls.SetEnabled(false);
        positionPlayer.SetEnabled(false);
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

    public void OnDefaultWidthChanged(string newText)
    {
        if (int.TryParse(newText, out int newValue))
        {
            defaultWidthValue = newValue;
            Debug.Log("width value: " + defaultWidthValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for width.");
        }
    }

    public void OnDefaultHeightChanged(string newText)
    {
        if (int.TryParse(newText, out int newValue))
        {
            defaultHeightValue = newValue;
            Debug.Log("height value: " + defaultHeightValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for height.");
        }
    }

    public void OnDefaultDensityChanged(string newText)
    {
        if (int.TryParse(newText, out int newValue))
        {
            defaultDensityValue = newValue;
            Debug.Log("density value: " + defaultDensityValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for density.");
        }
    }

    public void OnDefaultIterationChanged(string newText)
    {
        if (int.TryParse(newText, out int newValue))
        {
            defaultIterationValue = newValue;
            Debug.Log("iteration value: " + defaultIterationValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for iteration.");
        }
    }

    public void OnDefaultEnemyCChanged(string newText)
    {
        if (int.TryParse(newText, out int newValue))
        {
            defaultEnemyCValue = newValue;
            Debug.Log("enemy count value: " + defaultEnemyCValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for enemy count.");
        }
    }
    void OnNoiseCDChanged(bool newValue)
    {
        Debug.Log("Noise Cooldown toggled: " + newValue);
    }

    void OnCellularCDChanged(bool newValue)
    {
        Debug.Log("Cellular Cooldown toggled: " + newValue);
    }

    void OnPerlinCDChanged(bool newValue)
    {
        Debug.Log("Perlin Cooldown toggled: " + newValue);
    }

    void OnCDIterationChanged(bool newValue)
    {
        Debug.Log("Cooldown Iteration toggled: " + newValue);
    }
    void OnCDChanged(bool newValue)
    {
        Debug.Log("Cooldowntoggled: " + newValue);
    }
}
