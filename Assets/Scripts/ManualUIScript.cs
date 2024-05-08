using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Manual : MonoBehaviour
{
    public MapGenerationShowcase mapGenerationShowcase;

    //Buttons
    public Button show;
    public Button run;
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
    public Button hide;
    public Button goback;

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

    public Label label_1;
    public Label label_2;
    public Label label_3;
    public Label label_4;
    public Label label_5;
    public Label label_6;
    public Label label_7;
    public Label label_8;
    public Label label_9;
    public Label label_10;
    public Label label_11;
    public Label label_12;
    public Label label_13;
    public Label label_14;
    public Label label_15;
    public Label label_16;
    public Label label_17;

    public VisualElement notice;
    public VisualElement layer;

    public int floorCountValue;
    public int defaultWidthValue;
    public int defaultHeightValue;
    public int defaultDensityValue;
    public int defaultIterationValue;
    public int defaultEnemyCValue;
    public float noiseCooldownValue;
    public float cellularCooldownValue;
    public float perlinCooldownValue;
    public float cooldownValue;

    private bool isHidden = false;
    private bool isShowing = false;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        show = root.Q<Button>("showVariables");
        run = root.Q<Button>("generateMap-with-FR");
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
        hide = root.Q<Button>("hideButton");
        goback = root.Q<Button>("goback");

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
        oneLine = root.Q<Toggle>("OneLine");

        label_1 = root.Q<Label>("floorCount");
        label_2 = root.Q<Label>("defaultWidth");
        label_3 = root.Q<Label>("defaultHeight");
        label_4 = root.Q<Label>("defaultDensity");
        label_5 = root.Q<Label>("defaultIteration");
        label_6 = root.Q<Label>("defaultEnemyCount");
        label_7 = root.Q<Label>("noiseCooldown");
        label_8 = root.Q<Label>("cellularCooldown");
        label_9 = root.Q<Label>("perlinCooldown");
        label_10 = root.Q<Label>("cooldownIteration");
        label_11 = root.Q<Label>("cooldown");
        label_12 = root.Q<Label>("1");
        label_13 = root.Q<Label>("2");
        label_14 = root.Q<Label>("3");
        label_15 = root.Q<Label>("4");
        label_16 = root.Q<Label>("cooldownIteration21");
        label_17 = root.Q<Label>("toggle1");

        notice = root.Q<VisualElement>("warning");
        layer = root.Q<VisualElement>("Layer");

        show.clicked += OnShowClicked;
        run.clicked += OnRunClicked;
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
        hide.clicked += OnHideClicked;
        goback.clicked += OnGobackClicked;

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
        CD.RegisterValueChangedCallback(evt => OnCDChanged(evt.newValue));
        oneLine.RegisterValueChangedCallback(evt => OnOneLineChanged(evt.newValue));



        mapGenerationShowcase = FindObjectOfType<MapGenerationShowcase>();
        EnableButtons();
        HideVariable();
    }

    public void OnBackClicked()
    {
        SceneManager.LoadScene("Debug");
    }

    public void OnShowClicked()
    {
        isShowing = !isShowing;

        if (isShowing)
        {
            show.text = "Hide Variables";
            defaultWidth.style.display = DisplayStyle.Flex;
            defaultHeight.style.display = DisplayStyle.Flex;
            defaultDensity.style.display = DisplayStyle.Flex;
            defaultIteration.style.display = DisplayStyle.Flex;
            defaultEnemyC.style.display = DisplayStyle.Flex;

            noiseCooldown.style.display = DisplayStyle.Flex;
            cellularCooldown.style.display = DisplayStyle.Flex;
            perlinCooldown.style.display = DisplayStyle.Flex;
            CD.style.display = DisplayStyle.Flex;

            noise_CD.style.display = DisplayStyle.Flex;
            cellular_CD.style.display = DisplayStyle.Flex;
            perlin_CD.style.display = DisplayStyle.Flex;
            CD_iteration.style.display = DisplayStyle.Flex;
            oneLine.style.display = DisplayStyle.Flex;

            label_2.style.display = DisplayStyle.Flex;
            label_3.style.display = DisplayStyle.Flex;
            label_4.style.display = DisplayStyle.Flex;
            label_5.style.display = DisplayStyle.Flex;
            label_6.style.display = DisplayStyle.Flex;
            label_7.style.display = DisplayStyle.Flex;
            label_8.style.display = DisplayStyle.Flex;
            label_9.style.display = DisplayStyle.Flex;
            label_10.style.display = DisplayStyle.Flex;
            label_11.style.display = DisplayStyle.Flex;
            label_12.style.display = DisplayStyle.Flex;
            label_13.style.display = DisplayStyle.Flex;
            label_14.style.display = DisplayStyle.Flex;
            label_15.style.display = DisplayStyle.Flex;
            label_16.style.display = DisplayStyle.Flex;
            label_17.style.display = DisplayStyle.Flex;
        }
        else
        {
            show.text = "Show Variables";
            floorcount.style.display = DisplayStyle.None;
            defaultWidth.style.display = DisplayStyle.None;
            defaultHeight.style.display = DisplayStyle.None;
            defaultDensity.style.display = DisplayStyle.None;
            defaultIteration.style.display = DisplayStyle.None;
            defaultEnemyC.style.display = DisplayStyle.None;

            noiseCooldown.style.display = DisplayStyle.None;
            cellularCooldown.style.display = DisplayStyle.None;
            perlinCooldown.style.display = DisplayStyle.None;
            CD.style.display = DisplayStyle.None;

            noise_CD.style.display = DisplayStyle.None;
            cellular_CD.style.display = DisplayStyle.None;
            perlin_CD.style.display = DisplayStyle.None;
            CD_iteration.style.display = DisplayStyle.None;
            oneLine.style.display = DisplayStyle.None;

            label_1.style.display = DisplayStyle.None;
            label_2.style.display = DisplayStyle.None;
            label_3.style.display = DisplayStyle.None;
            label_4.style.display = DisplayStyle.None;
            label_5.style.display = DisplayStyle.None;
            label_6.style.display = DisplayStyle.None;
            label_7.style.display = DisplayStyle.None;
            label_8.style.display = DisplayStyle.None;
            label_9.style.display = DisplayStyle.None;
            label_10.style.display = DisplayStyle.None;
            label_11.style.display = DisplayStyle.None;
            label_12.style.display = DisplayStyle.None;
            label_13.style.display = DisplayStyle.None;
            label_14.style.display = DisplayStyle.None;
            label_15.style.display = DisplayStyle.None;
            label_16.style.display = DisplayStyle.None;
            label_17.style.display = DisplayStyle.None;
        }
    }
    public void OnHideClicked()
    {
        isHidden = !isHidden; 

        if (isHidden)
        {
  
            show.style.display = DisplayStyle.None;
            run.style.display = DisplayStyle.None;
            cleartiles.style.display = DisplayStyle.None;
            varset.style.display = DisplayStyle.None;
            creategrid.style.display = DisplayStyle.None;
            genNoise.style.display = DisplayStyle.None;
            applyCA.style.display = DisplayStyle.None;
            floodfill.style.display = DisplayStyle.None;
            applyPN.style.display = DisplayStyle.None;
            movewalls.style.display = DisplayStyle.None;
            positionPlayer.style.display = DisplayStyle.None;


            floorcount.style.display = DisplayStyle.None;
            defaultWidth.style.display = DisplayStyle.None;
            defaultHeight.style.display = DisplayStyle.None;
            defaultDensity.style.display = DisplayStyle.None;
            defaultIteration.style.display = DisplayStyle.None;
            defaultEnemyC.style.display = DisplayStyle.None;

            noiseCooldown.style.display = DisplayStyle.None;
            cellularCooldown.style.display = DisplayStyle.None;
            perlinCooldown.style.display = DisplayStyle.None;
            CD.style.display = DisplayStyle.None;

            noise_CD.style.display = DisplayStyle.None;
            cellular_CD.style.display = DisplayStyle.None;
            perlin_CD.style.display = DisplayStyle.None;
            CD_iteration.style.display = DisplayStyle.None;
            oneLine.style.display = DisplayStyle.None;

            label_1.style.display = DisplayStyle.None;
            label_2.style.display = DisplayStyle.None;
            label_3.style.display = DisplayStyle.None;
            label_4.style.display = DisplayStyle.None;
            label_5.style.display = DisplayStyle.None;
            label_6.style.display = DisplayStyle.None;
            label_7.style.display = DisplayStyle.None;
            label_8.style.display = DisplayStyle.None;
            label_9.style.display = DisplayStyle.None;
            label_10.style.display = DisplayStyle.None;
            label_11.style.display = DisplayStyle.None;
            label_12.style.display = DisplayStyle.None;
            label_13.style.display = DisplayStyle.None;
            label_14.style.display = DisplayStyle.None;
            label_15.style.display = DisplayStyle.None;
            label_16.style.display = DisplayStyle.None;
            label_17.style.display = DisplayStyle.None;
        }
        else
        {
            show.style.display = DisplayStyle.Flex;
            run.style.display = DisplayStyle.Flex;
            cleartiles.style.display = DisplayStyle.Flex;
            varset.style.display = DisplayStyle.Flex;
            creategrid.style.display = DisplayStyle.Flex;
            genNoise.style.display = DisplayStyle.Flex;
            applyCA.style.display = DisplayStyle.Flex;
            floodfill.style.display = DisplayStyle.Flex;
            applyPN.style.display = DisplayStyle.Flex;
            movewalls.style.display = DisplayStyle.Flex;
            positionPlayer.style.display = DisplayStyle.Flex;

        }
    }

    void HideVariable()
    {
        floorcount.style.display = DisplayStyle.None;
        defaultWidth.style.display = DisplayStyle.None;
        defaultHeight.style.display = DisplayStyle.None;
        defaultDensity.style.display = DisplayStyle.None;
        defaultIteration.style.display = DisplayStyle.None;
        defaultEnemyC.style.display = DisplayStyle.None;

        noiseCooldown.style.display = DisplayStyle.None;
        cellularCooldown.style.display = DisplayStyle.None;
        perlinCooldown.style.display = DisplayStyle.None;
        CD.style.display = DisplayStyle.None;

        noise_CD.style.display = DisplayStyle.None;
        cellular_CD.style.display = DisplayStyle.None;
        perlin_CD.style.display = DisplayStyle.None;
        CD_iteration.style.display = DisplayStyle.None;
        oneLine.style.display = DisplayStyle.None;

        label_1.style.display = DisplayStyle.None;
        label_2.style.display = DisplayStyle.None;
        label_3.style.display = DisplayStyle.None;
        label_4.style.display = DisplayStyle.None;
        label_5.style.display = DisplayStyle.None;
        label_6.style.display = DisplayStyle.None;
        label_7.style.display = DisplayStyle.None;
        label_8.style.display = DisplayStyle.None;
        label_9.style.display = DisplayStyle.None;
        label_10.style.display = DisplayStyle.None;
        label_11.style.display = DisplayStyle.None;
        label_12.style.display = DisplayStyle.None;
        label_13.style.display = DisplayStyle.None;
        label_14.style.display = DisplayStyle.None;
        label_15.style.display = DisplayStyle.None;
        label_16.style.display = DisplayStyle.None;
        label_17.style.display = DisplayStyle.None;

    }
    void OnRunClicked()
    {
        if (defaultWidthValue == 0 || defaultHeightValue == 0 || defaultDensityValue == 0 || defaultIterationValue == 0 || defaultEnemyCValue == 0)
        {
            notice.style.display = DisplayStyle.Flex;
            layer.style.display = DisplayStyle.Flex;
        }
        else{ 
        mapGenerationShowcase.StartCoroutine(mapGenerationShowcase.GenerateMap(false, floorCountValue, defaultWidthValue, defaultHeightValue, defaultDensityValue, defaultIterationValue, defaultEnemyCValue));
        DisableButtons();
        varset.SetEnabled(false);
        }
    }

    void OnGobackClicked()
    {
        notice.style.display = DisplayStyle.None;
        layer.style.display = DisplayStyle.None;
    }
    void OnCleartilesClicked()
    {

        mapGenerationShowcase.ClearTiles();
        EnableButtons();
    }

    void OnVarsetClicked()
    {
        if (defaultWidthValue == 0 || defaultHeightValue == 0 || defaultDensityValue == 0 || defaultIterationValue == 0 || defaultEnemyCValue == 0)
        {
            notice.style.display = DisplayStyle.Flex;
            layer.style.display = DisplayStyle.Flex;
        }
        else
        {
            mapGenerationShowcase.VariableSetUp(defaultWidthValue, defaultHeightValue, defaultDensityValue, defaultIterationValue, defaultEnemyCValue);
            EnableButtons();
            run.SetEnabled(false);
            creategrid.SetEnabled(true);
        }

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
        run.SetEnabled(false);
        varset.SetEnabled(false);
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
        run.SetEnabled(true);
        varset.SetEnabled(true);
        creategrid.SetEnabled(false);
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

    void OnNoiseCooldownChanged(string newText)
    {
        if (float.TryParse(newText, out float newValue))
        {
            noiseCooldownValue = newValue;
            mapGenerationShowcase.NoiseCDValueChange(newValue);
            Debug.Log("Noise cooldown value: " + noiseCooldownValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for noise cooldown.");
        }
    }

    void OnCellularCooldownChanged(string newText)
    {
        if (float.TryParse(newText, out float newValue))
        {
            cellularCooldownValue = newValue;
            mapGenerationShowcase.CellularCDValueChange(newValue);
            Debug.Log("Cellular cooldown value: " + cellularCooldownValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for cellular cooldown.");
        }
    }

    void OnPerlinCooldownChanged(string newText)
    {
        if (float.TryParse(newText, out float newValue))
        {
            perlinCooldownValue = newValue;
            mapGenerationShowcase.perlinCDValueChange(newValue);
            Debug.Log("Perlin cooldown value: " + perlinCooldownValue);
        }
        else
        {
            Debug.LogWarning("Invalid input for perlin cooldown.");
        }
    }

    void OnGeneralCooldownChanged(string newText)
    {
        if (float.TryParse(newText, out float newValue))
        {
            cooldownValue = newValue;
            mapGenerationShowcase.CDValueChange(newValue);
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
        mapGenerationShowcase.cellularCDChange(newValue);
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
    void OnCDChanged(string newValue)
    {
        Debug.Log("Cooldowntoggled: " + newValue);
    }
    void OnOneLineChanged(bool newValue)
    {
        mapGenerationShowcase.OneLineChange(newValue);
        Debug.Log("Cooldown Iteration toggled: " + newValue);
    }

}
