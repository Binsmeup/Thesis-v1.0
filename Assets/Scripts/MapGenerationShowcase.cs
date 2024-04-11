using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class MapGenerationShowcase : MonoBehaviour{
    public Tilemap Tilemap;
    public Tilemap WallCollision;
    public Tilemap Item;
    public Tile Floor;
    public Tile Wall;
    public Tile Perlin1;
    public Tile Perlin2;
    public Tile Perlin3;
    public Tile EnemyLocation;
    public Tile ChestLocation;
    public Tile PortalLocation;

    public GameObject Player;
    public GameObject Enemy;
    public GameObject Miniboss;
    public GameObject Boss;
    public GameObject Portal;
    public GameObject Chest;
    public GameObject Coin;
    public GameObject Debris;
    public GameObject Potion;

    public int floorCount;
    public int defaultWidth;
    public int defaultHeight;
    public int defaultDensity;
    public int defaultIteration;
    public int defaultEnemyCount;
    public int defaultChestCount;
    public float perlinScale = 0.1f;

    public bool floorRules;

    public bool OneLine;

    public float cooldown;

    public float noiseCooldown;
    public bool cooldownOnNoise;

    public float cellularCooldown;
    public bool cooldownOnCellular;

    public float perlinCooldown;
    public bool cooldownOnPerlin;

    public bool cooldownOnIteration;

    private int width;
    private int height;
    private int density;
    private int iteration;
    private int enemyCount;
    private int chestCount;
    private int portalCount = 1;
    private float statScale;
    private bool miniBossFloor;
    private bool bossFloor;

    private TileBase[,] grid;

    private List<Vector3Int> enemySpawns = new List<Vector3Int>();
    private List<Vector3Int> chestSpawns = new List<Vector3Int>();
    private List<Vector3Int> portalSpawns = new List<Vector3Int>();
    private List<Vector3Int> coinSpawns = new List<Vector3Int>();
    private List<Vector3Int> debrisSpawns = new List<Vector3Int>();
    private List<Vector3Int> potionSpawns = new List<Vector3Int>();
    

    void Start(){
        //StartCoroutine(GenerateMap());
    }

    public IEnumerator GenerateMap(bool floorRulesCheck, int setFloorValue, int defaultWidthValue,int defaultHeightValue, int defaultDensityValue,int defaultIterationValue,int defaultEnemyCValue){
        switch(floorRulesCheck){
            case true:
            floorRules = true;
            floorCount = setFloorValue;
            break;

            case false:
            floorRules = false;
            break;
        }
        ClearTiles();
        yield return new WaitForSeconds(cooldown);
        VariableSetUp(defaultWidthValue, defaultHeightValue, defaultDensityValue, defaultIterationValue, defaultEnemyCValue);
        yield return new WaitForSeconds(cooldown);
        CreateGrid();
        yield return new WaitForSeconds(cooldown);
        GenerateNoise();
        while (!noiseFinished){
        yield return null;
        }
        ApplyCellularAutomata();
        while (!cellularAutomataFinished){
        yield return null;
        }
        FloodFill();
        yield return new WaitForSeconds(cooldown);
        ApplyPerlinNoise();
        while (!perlinNoiseFinished){
        yield return null;
        }
        MoveWalls();
        yield return new WaitForSeconds(cooldown);
        PositionPlayer();
    }

    public void VariableSetUp(int setWidth, int setHeight, int setDensity, int setIteration, int setEnemyCount){
        switch (floorRules){
            case true:
            defaultWidth = 52;
            defaultHeight = 52;
            defaultDensity = 57;
            defaultIteration = 6;
            defaultEnemyCount = 12;
            miniBossFloor = false;
            bossFloor = false;

            width = defaultWidth + (4 * floorCount);
            height = defaultHeight + (4 * floorCount);
            density = defaultDensity;
            iteration = defaultIteration;
            enemyCount = defaultEnemyCount + (3 * floorCount);

            switch (floorCount){
            case int n when n >= 1 && n <= 5:
                statScale = 0.1f;
                chestCount = 1;
                switch (floorCount){
                    case 5:
                    BossRoom();
                    break;
                default:

                break;               
            }
            break;
            case int n when n >= 6 && n <= 10:
                statScale = 0.15f;
                chestCount = 2;
                switch (floorCount){
                    case 10:
                    BossRoom();
                    break;
                default:
                
                break;               
            }
            break;
            case int n when n >= 11 && n <= 15:
                statScale = 0.2f;
                chestCount = 3;
                switch (floorCount){
                    case 15:
                    BossRoom();
                    break;
                default:
                
                break;               
            }
            break;
            case int n when n >= 16 && n <= 20:
                statScale = 0.25f;
                chestCount = 4;
                switch (floorCount){
                    case 20:
                    BossRoom();
                    break;
                default:
                
                break;               
            }
            break;

            default:
                statScale = 0.3f + ((floorCount-20)*0.01f);
                chestCount = 0;
            break;
        }   
            break;

            case false:
            width = setWidth;
            height = setHeight;
            density = setDensity;
            iteration = setIteration;
            enemyCount = setEnemyCount;
            chestCount = defaultChestCount;

            break;
        }
        if (width >= 360){
             width = 360;
        }
        if (height >= 360){
            height = 360;
        }
        if (enemyCount >= 210){
            enemyCount = 210;
        }   
    }

    bool noiseFinished = false;
    bool cellularAutomataFinished = false;
    bool perlinNoiseFinished = false;

    void BossRoom(){
        bossFloor = true;
        chestCount = 0;
        width = 40;
        height = 40;
        density = 50;
        iteration = 10;
        enemyCount = 0;
        chestCount = 0;
        portalCount = 0;
    }

    public void GenerateNoise(){
        StartCoroutine(Noise());
    }

    public void ApplyCellularAutomata(){
        StartCoroutine(CellularAutomata());
    }
    public void ApplyPerlinNoise(){
        StartCoroutine(PerlinNoiseActivate());
    }

    public void CreateGrid(){
        grid = new TileBase[width, height];

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                grid[x, y] = Floor;
                Tilemap.SetTile(tilePosition, Floor);
            }
        }
    }

    IEnumerator Noise(){
        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (Random.Range(0, 100) < density){
                    grid[x, y] = Wall;
                    Tilemap.SetTile(tilePosition, Wall);
                }
                if (!OneLine){
                    if (cooldownOnNoise){
                        yield return new WaitForSeconds(noiseCooldown);
                    }
                }
            }
            if (cooldownOnNoise){
                if (OneLine){
                    yield return new WaitForSeconds(noiseCooldown);
                }
            }
        }
        noiseFinished = true;
    }

    IEnumerator CellularAutomata(){
        for (int currentIteration = 0; currentIteration < iteration; currentIteration++){
            TileBase[,] newGrid = CopyGrid();

            for (int x = 0; x < width; x++){
                for (int y = 0; y < height; y++){
                    int neighborWallCount = CountNeighboringWalls(newGrid, x, y);
                    bool hasOutOfBoundsNeighbor = HasOutOfBoundsNeighbor(x, y);

                    if (neighborWallCount > 4 || hasOutOfBoundsNeighbor){
                        grid[x, y] = Wall;
                    }
                    else{
                        grid[x, y] = Floor;
                    }
                    if (!OneLine){
                        if (cooldownOnCellular){
                            UpdateGrid();
                            yield return new WaitForSeconds(cellularCooldown);
                        }
                    }
                }
                if (cooldownOnCellular){
                    if (OneLine){
                        UpdateGrid();
                        yield return new WaitForSeconds(cellularCooldown);
                    }
                }
            }
            UpdateGrid();
            if (cooldownOnIteration){
                Debug.Log("Iteration Count = " + currentIteration);
                yield return new WaitForSeconds(cooldown);
            }
        }
        cellularAutomataFinished = true;
    }

    private bool HasOutOfBoundsNeighbor(int x, int y){
    for (int i = x - 1; i <= x + 1; i++){
        for (int j = y - 1; j <= y + 1; j++){
            if ((i != x || j != y) && (i < 0 || i >= width || j < 0 || j >= height)){
                return true;
            }
        }
    }

    return false;
}

    private int CountNeighboringWalls(TileBase[,] currentGrid, int x, int y){
        int wallCount = 0;

        for (int i = x - 1; i <= x + 1; i++){
            for (int j = y - 1; j <= y + 1; j++){
                if ((i != x || j != y)){
                    if (i >= 0 && i < width && j >= 0 && j < height){
                        if (currentGrid[i, j] == Wall){
                            wallCount++;
                        }
                    }
                    else{
                        wallCount++;
                    }
                }
            }
        }

        return wallCount;
    }

    private TileBase[,] CopyGrid(){
        TileBase[,] newGrid = new TileBase[width, height];

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                newGrid[x, y] = grid[x, y];
            }
        }

        return newGrid;
    }

    void UpdateGrid(){
        Tilemap.ClearAllTiles();

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                Tilemap.SetTile(tilePosition, grid[x, y]);
            }
        }
    }
    public void FloodFill(){
        List<List<Vector2Int>> groups = new List<List<Vector2Int>>();

        bool[,] visited = new bool[width, height];
        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                visited[x, y] = grid[x, y] == Wall;
            }
        }

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                if (!visited[x, y]){
                    List<Vector2Int> group = new List<Vector2Int>();
                    FloodFillConnect(x, y, visited, group);
                    groups.Add(group);
                }
            }
        }

        List<Vector2Int> largestGroup = new List<Vector2Int>();
        int largestGroupIndex = -1;

        for (int i = 0; i < groups.Count; i++){
            if (groups[i].Count > largestGroup.Count){
                largestGroup = groups[i];
                largestGroupIndex = i;
            }
        }

        if (largestGroupIndex != -1){
            groups.RemoveAt(largestGroupIndex);

            foreach (List<Vector2Int> group in groups){
                foreach (Vector2Int cell in group){
                    grid[(int)cell.x, (int)cell.y] = Wall;
                }
            }
        }
        UpdateGrid();
    }

    void FloodFillConnect(int x, int y, bool[,] visited, List<Vector2Int> group){
        if (x < 0 || x >= width || y < 0 || y >= height || visited[x, y]){
            return;
        }

        visited[x, y] = true;
        group.Add(new Vector2Int(x, y));

        FloodFillConnect(x + 1, y, visited, group);
        FloodFillConnect(x - 1, y, visited, group);
        FloodFillConnect(x, y + 1, visited, group);
        FloodFillConnect(x, y - 1, visited, group);
    }

    IEnumerator PerlinNoiseActivate(){
        int enemySpawnedCount = 0;
        int chestSpawnedCount = 0;
        int portalSpawnedCount = 0;
        int coinSpawnedCount = 0;
        int debrisSpawnedCount = 0;
        int potionSpawnedCount = 0;
        int coinCount = 0;
        int debrisCount = 0;
        int potionCount = 0;
        enemySpawns.Clear();
        coinSpawns.Clear();
        debrisSpawns.Clear();
        potionSpawns.Clear();
        chestSpawns.Clear();
        portalSpawns.Clear();

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (grid[x, y] == Floor){
                    float perlinValue = Mathf.PerlinNoise(x * perlinScale, y * perlinScale);
                    float modifiedValue = perlinValue * 3f;
                    float randomChance = Random.Range(0f, 100f);

                    if (modifiedValue >= 0 && modifiedValue <= 0.5f){
                        Item.SetTile(tilePosition, Perlin1);
                        if (randomChance <= 60f){
                            debrisSpawns.Add(tilePosition);
                            debrisCount++;
                        }
                        else if(randomChance >= 99f){
                            potionSpawns.Add(tilePosition);
                            potionCount++;
                        }
                    }
                    else if (modifiedValue > 0.5f && modifiedValue <= 1.25f){
                        Item.SetTile(tilePosition, Perlin2);
                        if (randomChance <= 50f){
                            Item.SetTile(tilePosition, EnemyLocation);
                            enemySpawns.Add(tilePosition);
                        }
                        else if (randomChance >= 90f && randomChance <= 94.49f){
                            debrisSpawns.Add(tilePosition);
                            debrisCount++;
                        }
                        else if (randomChance >= 95f){
                            coinSpawns.Add(tilePosition);
                            coinCount++;
                        }
                    }
                    else if (modifiedValue > 1.25f && modifiedValue <= 1.75f){
                        Item.SetTile(tilePosition, Perlin3);
                        if (randomChance <= 10f){
                            if (randomChance <= 35f){
                                Item.SetTile(tilePosition, EnemyLocation);
                                enemySpawns.Add(tilePosition);
                            }
                            else if (randomChance >= 75f && randomChance <= 89.9f){
                                debrisSpawns.Add(tilePosition);
                                debrisCount++;
                            }
                            else if (randomChance >= 90f){
                                coinSpawns.Add(tilePosition);
                                coinCount++;
                            }
                        }
                    }
                    else if (modifiedValue > 1.75f && modifiedValue <= 3){
                        if (randomChance >= 50f){
                            Item.SetTile(tilePosition, ChestLocation);
                            chestSpawns.Add(tilePosition);
                        }
                        else{
                            Item.SetTile(tilePosition, PortalLocation);
                            portalSpawns.Add(tilePosition);
                        }
                    }
                }
                if (!OneLine){
                    if (cooldownOnPerlin){
                        yield return new WaitForSeconds(perlinCooldown);
                    }
                }
            }
            if (cooldownOnPerlin){
                if (OneLine){
                    yield return new WaitForSeconds(perlinCooldown);
                }
            }
        }
        yield return new WaitForSeconds(cooldown);
        SpawnObjects(coinSpawns, Coin, ref coinSpawnedCount, coinCount, 1f);
        yield return new WaitForSeconds(cooldown);
        SpawnObjects(potionSpawns, Potion, ref potionSpawnedCount, potionCount, 1f);
        yield return new WaitForSeconds(cooldown);
        SpawnObjects(debrisSpawns, Debris, ref debrisSpawnedCount, debrisCount, 1f);
        yield return new WaitForSeconds(cooldown);
        SpawnObjects(enemySpawns, Enemy, ref enemySpawnedCount, enemyCount, statScale);
        yield return new WaitForSeconds(cooldown);
        SpawnObjects(chestSpawns, Chest, ref chestSpawnedCount, chestCount, 1f);
        yield return new WaitForSeconds(cooldown);
        SpawnObjects(portalSpawns, Portal, ref portalSpawnedCount, portalCount, 1f);
        yield return new WaitForSeconds(cooldown);
        perlinNoiseFinished = true;
    }

    void SpawnObjects(List<Vector3Int> spawnPositions, GameObject prefab, ref int spawnedCount, int totalCount, float scale){
        while (spawnedCount < totalCount && spawnPositions.Count > 0) {
            Vector3Int spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];
            spawnPositions.Remove(spawnPosition);
            Item.SetTile(spawnPosition, null);

            GameObject obj = Instantiate(prefab, Tilemap.GetCellCenterWorld(spawnPosition), Quaternion.identity);

            HealthManager healthManager = obj.GetComponent<HealthManager>();
            Enemy enemyScript = obj.GetComponent<Enemy>();

            if (healthManager != null) {
                healthManager.maxHealth *= (1 + statScale * (floorCount-1)) ;
                healthManager.health = healthManager.maxHealth;
            }
            if (enemyScript != null){
                enemyScript.baseDamage *= (1 + statScale * (floorCount-1));
            }
            spawnedCount++;
        }
    }

    public void MoveWalls(){
        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                TileBase currentTile = Tilemap.GetTile(tilePosition);

                if (currentTile != null && currentTile.name == Wall.name){
                    WallCollision.SetTile(tilePosition, currentTile);
                    Tilemap.SetTile(tilePosition, null);
                }
            }
        }  
    }

    public void PositionPlayer(){
        List<Vector3Int> floorTiles = new List<Vector3Int>();
        HashSet<GameObject> removedObjects = new HashSet<GameObject>();
        int enemyRemoved = 0;
        int chestRemoved = 0;
        int portalRemoved = 0;
        int enemyReposition;
        int chestReposition;
        int portalReposition;

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (grid[x, y] == Floor){
                    floorTiles.Add(tilePosition);
                }
            }
        }

        if (floorTiles.Count > 0){
            Vector3Int playerSpawnPoint = Vector3Int.zero;

            if (bossFloor) {
                playerSpawnPoint = FindLowestMiddleFloor(floorTiles);
                playerSpawnPoint += Vector3Int.up;
            } else {
                Vector3Int randomFloorTile;

                do {
                    randomFloorTile = floorTiles[Random.Range(0, floorTiles.Count)];
                } while (HasWallsInRadius(randomFloorTile));

                playerSpawnPoint = randomFloorTile;
            }

            Vector3 spawnPosition = Tilemap.GetCellCenterWorld(playerSpawnPoint);

            GameObject playerInstance = Instantiate(Player, spawnPosition, Quaternion.identity);
            CircleCollider2D collider = playerInstance.GetComponent<CircleCollider2D>();
            float radius = collider.radius;

            Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(spawnPosition, radius);
            foreach (Collider2D nearbyObject in nearbyObjects){
                if ((nearbyObject.CompareTag("Enemy") || nearbyObject.CompareTag("Chest") || nearbyObject.CompareTag("Portal"))
                    && !removedObjects.Contains(nearbyObject.gameObject)){
                    if (nearbyObject.CompareTag("Enemy")) {
                        enemyRemoved++;
                    } else if (nearbyObject.CompareTag("Chest")) {
                        chestRemoved++;
                    } else if (nearbyObject.CompareTag("Portal")) {
                        portalRemoved++;
                    }
                    removedObjects.Add(nearbyObject.gameObject);
                    Destroy(nearbyObject.gameObject);
                }
            }
            RemoveSpawnPos(spawnPosition, radius);

            enemyReposition = enemyCount - enemyRemoved;
            chestReposition = chestCount - chestRemoved;
            portalReposition = portalCount - portalRemoved;
            int miniBossCount = 0;
            int bossCount = 0;

            SpawnObjects(enemySpawns, Enemy, ref enemyReposition, enemyCount, statScale);
            SpawnObjects(chestSpawns, Chest, ref chestReposition, chestCount, 1f);
            SpawnObjects(portalSpawns, Portal, ref portalReposition, portalCount, 1f);
            if (miniBossFloor){
                SpawnObjects(enemySpawns, Miniboss, ref miniBossCount, 1, 1f);
            }
            if (bossFloor){
                SpawnObjects(enemySpawns, Boss, ref bossCount, 1, 1f);
            }
        }
        else{
            Debug.LogWarning("No valid floor tiles to spawn the player.");
        }
    }


    void RemoveSpawnPos(Vector3 spawnPosition, float radius){
        SpawnRemover(spawnPosition, radius, enemySpawns);
        SpawnRemover(spawnPosition, radius, chestSpawns);
        SpawnRemover(spawnPosition, radius, portalSpawns);
    }

    Vector3Int FindLowestMiddleFloor(List<Vector3Int> floorTiles) {
        int lowestY = int.MaxValue;
        int middleX = width / 2;
        Vector3Int lowestMiddleFloor = Vector3Int.zero;

        foreach (Vector3Int floorTile in floorTiles) {
            if (floorTile.x == middleX) {
                if (floorTile.y < lowestY) {
                    lowestY = floorTile.y;
                    lowestMiddleFloor = floorTile;
                }
            }
        }

        return lowestMiddleFloor;
    }

    void SpawnRemover(Vector3 center, float radius, List<Vector3Int> spawnPositions){
        List<Vector3Int> positionsToRemove = new List<Vector3Int>();

        foreach (Vector3Int spawnPosition in spawnPositions){
            Vector3 spawnPositionWorld = Tilemap.GetCellCenterWorld(spawnPosition);
            if (Vector3.Distance(center, spawnPositionWorld) <= radius){
                positionsToRemove.Add(spawnPosition);
            }
        }

        foreach (Vector3Int positionToRemove in positionsToRemove){
            spawnPositions.Remove(positionToRemove);
            Item.SetTile(positionToRemove, null);
        }
    }

    bool HasWallsInRadius(Vector3Int centerTile){
        for (int xOffset = -1; xOffset <= 1; xOffset++){
            for (int yOffset = -1; yOffset <= 1; yOffset++){
                if (xOffset == 0 && yOffset == 0) continue;
                    int x = centerTile.x + xOffset;
                    int y = centerTile.y + yOffset;
                if (x >= 0 && x < width && y >= 0 && y < height){
                    if (grid[x, y] == Wall){
                        return true;
                    }
                }
                else{
                    return true;
                }
            }
        }
        return false;
    }

    private void ClearScene() {
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        List<string> excludeNames = new List<string>();

        excludeNames.Add(Camera.main.gameObject.name);
        excludeNames.Add("1");
        excludeNames.Add(FindObjectOfType<MapGenerationShowcase>().gameObject.name);
        excludeNames.Add("Grid");
        excludeNames.Add("UIDocument");
        foreach (GameObject obj in allObjects) {
            if (!excludeNames.Contains(obj.name)) {
                Destroy(obj);
            }
        }
    }


    public void ClearTiles() {
        ClearScene();
        Tilemap.ClearAllTiles();
        WallCollision.ClearAllTiles();
        Item.ClearAllTiles();
    }
    public void celluarCDChange(bool toggle){
        cooldownOnCellular = toggle;
    }
    public void noiseCDChange(bool toggle){
        cooldownOnNoise = toggle;
    }
    public void perlinCDChange(bool toggle){
        cooldownOnPerlin = toggle;
    }
    public void iterationCDChange(bool toggle){
        cooldownOnIteration = toggle;
    }
}
