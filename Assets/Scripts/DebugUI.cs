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
    public Button back;


    //TextField
    public TextField floorcount;
    public TextField defaultWidth;
    public TextField defaultHeight;
    public TextField defaultDensity;
    public TextField defaultIteration;
    public TextField defaultEnemyC;
    public TextField noiseCooldown;
    public TextField cellularCooldown;
    public TextField perlinCooldown;
    public TextField CD;

    //Toggle
    public Toggle noise_CD;
    public Toggle cellular_CD;
    public Toggle perlin_CD;
    public Toggle CD_iteration;
    public Toggle oneLine;

    public int floorCountValue;
    public int defaultWidthValue;
    public int defaultHeightValue;
    public int defaultDensityValue;
    public int defaultIterationValue;
    public int defaultEnemyCValue;
    public int noiseCooldownValue;
    public int cellularCooldownValue;
    public int perlinCooldownValue;
    public int cooldownValue;

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
        back = root.Q<Button>("L-back");

        floorcount = root.Q<TextField>("floorCountInput");
        defaultWidth = root.Q<TextField>("defaultWidthInput");
        defaultHeight = root.Q<TextField>("defaultHeightInput");
        defaultDensity = root.Q<TextField>("defaultDensityInput");
        defaultIteration = root.Q<TextField>("defaultIterationInput");
        defaultEnemyC = root.Q<TextField>("defaultEnemyCountInput");
        noiseCooldown = root.Q<TextField>("noiseCooldownInput");
        cellularCooldown = root.Q<TextField>("CACooldownInput");
        perlinCooldown = root.Q<TextField>("perlinCooldownInput");
        CD = root.Q<TextField>("cooldownInput");

        noise_CD = root.Q<Toggle>("cooldownOnNoise");
        cellular_CD = root.Q<Toggle>("cooldownOnCellular");
        perlin_CD = root.Q<Toggle>("cooldownOnPerlin");
        CD_iteration = root.Q<Toggle>("cooldownOnIteration");
        oneLine= root.Q<Toggle>("OneLine");

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
        back.clicked += OnBackClicked;

        floorcount.RegisterCallback<ChangeEvent<string>>(evt => OnFloorcountChanged(evt.newValue));
        defaultWidth.RegisterCallback<ChangeEvent<string>>(evt => OnDefaultWidthChanged(evt.newValue));
        defaultHeight.RegisterCallback<ChangeEvent<string>>(evt => OnDefaultHeightChanged(evt.newValue));
        defaultDensity.RegisterCallback<ChangeEvent<string>>(evt => OnDefaultDensityChanged(evt.newValue));
        defaultIteration.RegisterCallback<ChangeEvent<string>>(evt => OnDefaultIterationChanged(evt.newValue));
        defaultEnemyC.RegisterCallback<ChangeEvent<string>>(evt => OnDefaultEnemyCChanged(evt.newValue));
        noiseCooldown.RegisterCallback<ChangeEvent<string>>(evt => OnNoiseCooldownChanged(evt.newValue));
        cellularCooldown.RegisterCallback<ChangeEvent<string>>(evt => OnCellularCooldownChanged(evt.newValue));
        perlinCooldown.RegisterCallback<ChangeEvent<string>>(evt => OnPerlinCooldownChanged(evt.newValue));
        CD.RegisterCallback<ChangeEvent<string>>(evt => OnGeneralCooldownChanged(evt.newValue));

        noise_CD.RegisterValueChangedCallback(evt => OnNoiseCDChanged(evt.newValue));
        cellular_CD.RegisterValueChangedCallback(evt => OnCellularCDChanged(evt.newValue));
        perlin_CD.RegisterValueChangedCallback(evt => OnPerlinCDChanged(evt.newValue));
        CD_iteration.RegisterValueChangedCallback(evt => OnCDIterationChanged(evt.newValue));
        oneLine.RegisterValueChangedCallback(evt => OnOneLineChanged(evt.newValue));

        

        mapGenerationShowcase = FindObjectOfType<MapGenerationShowcase>();
        EnableButtons();
    }


    void OnGMnoFRClicked()
    {
        if (mapGenerationShowcase != null)
        {
            mapGenerationShowcase.StartCoroutine(mapGenerationShowcase.GenerateMap(false, floorCountValue, defaultWidthValue, defaultHeightValue, defaultDensityValue, defaultIterationValue, defaultEnemyCValue));//not sure what to do here
            DisableButtons();
        }
        else
        {
            Debug.LogWarning("MapGenerationShowcase not found!");
        }
    }


    void OnGMwithFRClicked()
    {
        mapGenerationShowcase.StartCoroutine(mapGenerationShowcase.GenerateMap(true, floorCountValue, defaultWidthValue, defaultHeightValue, defaultDensityValue, defaultIterationValue, defaultEnemyCValue));//not sure what to do here
        DisableButtons();

    }

    void OnCleartilesClicked()
    {

        mapGenerationShowcase.ClearTiles();
        EnableButtons();
    }

    void OnVarsetClicked()
    {
        mapGenerationShowcase.VariableSetUp(defaultWidthValue, defaultHeightValue, defaultDensityValue, defaultIterationValue, defaultEnemyCValue);
        EnableButtons();
        creategrid.SetEnabled(true);

    }

    void OnCreategridClicked()
    {
        mapGenerationShowcase.CreateGrid();
        DisableButtons();
        genNoise.SetEnabled(true);

    }

    void OnGenNoiseClicked()
    {
        mapGenerationShowcase.GenerateNoise();
        DisableButtons();
        applyCA.SetEnabled(true);
    }

    void OnApplyCAClicked()
    {
        mapGenerationShowcase.ApplyCellularAutomata();
        DisableButtons();
        floodfill.SetEnabled(true);
    }

    void OnFloodfillClicked()
    {
        mapGenerationShowcase.FloodFill();
        DisableButtons();
        applyPN.SetEnabled(true);
    }

    void OnApplyPNClicked()
    {
        mapGenerationShowcase.ApplyPerlinNoise();
        DisableButtons();
        movewalls.SetEnabled(true);
    }

    void OnMovewallsClicked()
    {
        mapGenerationShowcase.MoveWalls();
        DisableButtons();
        positionPlayer.SetEnabled(true);
    }

    void OnPositionPlayerClicked()
    {
        mapGenerationShowcase.PositionPlayer();
        DisableButtons();
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
        creategrid.SetEnabled(false);
        genNoise.SetEnabled(false);
        applyCA.SetEnabled(false);
        floodfill.SetEnabled(false);
        applyPN.SetEnabled(false);
        movewalls.SetEnabled(false);
        positionPlayer.SetEnabled(false);
    }

    public void OnBackClicked()
    {
        SceneManager.LoadScene("Main_menu");
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

    void OnNoiseCooldownChanged(string newText)
    {
        if (int.TryParse(newText, out int newValue))
        {
            noiseCooldownValue = newValue;
            Debug.Log("Noise cooldown value: " + noiseCooldownValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for noise cooldown.");
        }
    }

    void OnCellularCooldownChanged(string newText)
    {
        if (int.TryParse(newText, out int newValue))
        {
            cellularCooldownValue = newValue;
            Debug.Log("Cellular cooldown value: " + cellularCooldownValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for cellular cooldown.");
        }
    }

    void OnPerlinCooldownChanged(string newText)
    {
        if (int.TryParse(newText, out int newValue))
        {
            perlinCooldownValue = newValue;
            Debug.Log("Perlin cooldown value: " + perlinCooldownValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for perlin cooldown.");
        }
    }

    void OnGeneralCooldownChanged(string newText)
    {
        if (int.TryParse(newText, out int newValue))
        {
            cooldownValue = newValue;
            Debug.Log("cooldown value: " + cooldownValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for general cooldown.");
        }
    }



    void OnNoiseCDChanged(bool newValue)
    {
        mapGenerationShowcase.noiseCDChange(newValue);
        Debug.Log("Noise Cooldown toggled: " + newValue);
    }

    void OnCellularCDChanged(bool newValue)
    {
        mapGenerationShowcase.celluarCDChange(newValue);
        Debug.Log("Cellular Cooldown toggled: " + newValue);
    }

    void OnPerlinCDChanged(bool newValue)
    {
        mapGenerationShowcase.perlinCDChange(newValue);
        Debug.Log("Perlin Cooldown toggled: " + newValue);
    }

    void OnCDIterationChanged(bool newValue)
    {
        mapGenerationShowcase.iterationCDChange(newValue);
        Debug.Log("Cooldown Iteration toggled: " + newValue);
    }

    void OnOneLineChanged(bool newValue)
    {
        mapGenerationShowcase.OneLineChange(newValue);
        Debug.Log("Cooldown Iteration toggled: " + newValue);
    }

}
