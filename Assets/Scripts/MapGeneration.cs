using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour{
    public Tilemap Tilemap;
    public Tile Floor;
    public Tile Wall;
    public GameObject Player;
    public GameObject Basic;

    public GameObject Charger;
    public GameObject Fast;
    public GameObject RangedEnemy;
    public GameObject Sniper;
    public GameObject Tank;
    public GameObject Boss;
    public GameObject Portal;
    public GameObject Chest;

    public GameObject Coin;
    public GameObject Debris;
    public GameObject Potion;

    public Tilemap WallCollision;

    public int killCount;

    public int floorCount;
    public int defaultWidth;
    public int defaultHeight;
    public int defaultDensity;
    public int defaultIteration;
    public int defaultEnemyCount;
    public float perlinScale = 0.1f;

    public enum EnemyType {
        Charger,
        Fast,
        Ranged,
        Tank,
        Sniper
    }

    private int width;
    private int height;
    private int density;
    private int iteration;
    private int enemyCount;
    private int chestCount;
    private int portalCount = 1;
    private bool miniBossFloor;
    private bool bossFloor;
    private float statScale;
    private bool enableSpeedScaling;
    
    private bool enableCharger;
    private bool enableFast;
    private bool enableRanged;
    private bool enableTank;
    private bool enableSniper;

    private TileBase[,] grid;

    private List<Vector3Int> enemySpawns = new List<Vector3Int>();
    private List<Vector3Int> chestSpawns = new List<Vector3Int>();
    private List<Vector3Int> portalSpawns = new List<Vector3Int>();
    private List<Vector3Int> coinSpawns = new List<Vector3Int>();
    private List<Vector3Int> debrisSpawns = new List<Vector3Int>();
    private List<Vector3Int> potionSpawns = new List<Vector3Int>();
    

    void Start(){
        GenerateMap();
    }

    public void GenerateMap(){
        ClearTiles();
        VariableSetUp();
        CreateGrid();
        GenerateNoise();
        ApplyCellularAutomata();
        FloodFill();
        ApplyPerlinNoise();
        MoveWalls();
        PositionPlayer();
    }

    void VariableSetUp(){
        width = defaultWidth + (4 * floorCount);
            if (width >= 150){
                width = 150;
            }
        height = defaultHeight + (4 * floorCount);
        if (height >= 150){
                height = 150;
            }
        density = defaultDensity;
        iteration = defaultIteration;
        miniBossFloor = false;
        bossFloor = false;
        portalCount = 1;
        enemyCount = defaultEnemyCount + (3 * floorCount);
        if (enemyCount >= 150){
                enemyCount = 150;
            }
        switch (floorCount){
            case int n when n >= 1 && n <= 5:
                statScale = 0.1f;
                chestCount = 1;
                switch (floorCount){
                    case 4:
                    miniBossFloor = true;
                    break;
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
                enableCharger = true;
                enableFast = true;
                switch (floorCount){
                    case 9:
                    miniBossFloor = true;
                    break;
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
                enableRanged = true;
                enableCharger = true;
                enableFast = true;
                enableSpeedScaling = true;
                switch (floorCount){
                    case 14:
                    miniBossFloor = true;
                    break;
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
                enableCharger = true;
                enableFast = true;
                enableRanged = true;
                enableSniper = true;
                enableTank = true;
                switch (floorCount){
                    case 19:
                    miniBossFloor = true;
                    break;
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
                enableCharger = true;
                enableFast = true;
                enableRanged = true;
                enableSniper = true;
                enableTank = true;
                miniBossFloor = true;
            break;
        }   
    }

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

    void CreateGrid(){
        grid = new TileBase[width, height];

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                grid[x, y] = Floor;
                Tilemap.SetTile(tilePosition, Floor);
            }
        }
    }

    void GenerateNoise(){
        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (Random.Range(0, 100) < density){
                    grid[x, y] = Wall;
                    Tilemap.SetTile(tilePosition, Wall);
                }
            }
        }
    }

    void ApplyCellularAutomata(){
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
                }
            }
            UpdateGrid();
        }
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
 void FloodFill(){
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

    void ApplyPerlinNoise(){
<<<<<<< HEAD
        int seed = Random.Range(0, 100000);
        PerlinNoiseGenerator perlin = new PerlinNoiseGenerator(seed);
=======
>>>>>>> bd17dcf6538a948a8e8c4520e565fefe839de3d4
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
<<<<<<< HEAD
                    float perlinValue = perlin.Perlin(x * perlinScale, y * perlinScale);
                    float modifiedValue = perlinValue * 1.5f; 
                    float randomChance = Random.Range(0f, 100f);

                    if (modifiedValue >= -1.5f && modifiedValue <= -1f){
=======
                    float perlinValue = Mathf.PerlinNoise(x * perlinScale, y * perlinScale);
                    float modifiedValue = perlinValue * 3f; 
                    float randomChance = Random.Range(0f, 100f);

                    if (modifiedValue >= 0 && modifiedValue <= 0.5f){
>>>>>>> bd17dcf6538a948a8e8c4520e565fefe839de3d4
                        if (randomChance <= 60f){
                            debrisSpawns.Add(tilePosition);
                            debrisCount++;
                        }
                        else if(randomChance >= 99f){
                            potionSpawns.Add(tilePosition);
                            potionCount++;
                        }
                    }
<<<<<<< HEAD
                    else if (modifiedValue > -1f && modifiedValue <= -0.25f){
=======
                    else if (modifiedValue > 0.5f && modifiedValue <= 1.25f){
>>>>>>> bd17dcf6538a948a8e8c4520e565fefe839de3d4
                        if (randomChance <= 50f){
                            enemySpawns.Add(tilePosition);
                        }
                        else if (randomChance >= 90f && randomChance <= 95f){
                            debrisSpawns.Add(tilePosition);
                            debrisCount++;
                        }
                        else if (randomChance > 95f){
                            coinSpawns.Add(tilePosition);
                            coinCount++;
                        }
                    }
<<<<<<< HEAD
                    else if (modifiedValue > -0.25f && modifiedValue <= 0.25f && randomChance <= 10f){
=======
                    else if (modifiedValue > 1.25f && modifiedValue <= 1.75f && randomChance <= 10f){
>>>>>>> bd17dcf6538a948a8e8c4520e565fefe839de3d4
                        if (randomChance <= 35f){
                            enemySpawns.Add(tilePosition);
                        }
                        else if (randomChance >= 75f && randomChance <= 90f){
                            debrisSpawns.Add(tilePosition);
                            debrisCount++;
                        }
                        else if (randomChance > 90f){
                            coinSpawns.Add(tilePosition);
                            coinCount++;
                        }
                    }
<<<<<<< HEAD
                    else if (modifiedValue > 0.25f && modifiedValue <= 1.5f){
=======
                    else if (modifiedValue > 1.75f && modifiedValue < 3){
>>>>>>> bd17dcf6538a948a8e8c4520e565fefe839de3d4
                        if (randomChance >= 50f){
                            chestSpawns.Add(tilePosition);
                        }
                        else{
                            portalSpawns.Add(tilePosition);
                        }
                    }
                }
            }
        }
        SpawnObjects(coinSpawns, Coin, ref coinSpawnedCount, coinCount, 1f, false);
        SpawnObjects(potionSpawns, Potion, ref potionSpawnedCount, potionCount, 1f, false);
        SpawnObjects(debrisSpawns, Debris, ref debrisSpawnedCount, debrisCount, 1f, false);
        if (enableCharger || enableFast || enableRanged || enableTank || enableSniper){
            int dividedEnemyCount = DivideEnemyCount();
            if (enableCharger){
                SpawnObjects(enemySpawns, Charger, ref enemySpawnedCount, dividedEnemyCount, statScale, false);
                enemySpawnedCount = 0;
            }
            if (enableFast){
                SpawnObjects(enemySpawns, Fast, ref enemySpawnedCount, dividedEnemyCount, statScale, false);
                enemySpawnedCount = 0;
            }
            if (enableRanged){
                SpawnObjects(enemySpawns, RangedEnemy, ref enemySpawnedCount, dividedEnemyCount, statScale, false);
                enemySpawnedCount = 0;
            }
            if (enableTank){
                SpawnObjects(enemySpawns, Tank, ref enemySpawnedCount, dividedEnemyCount, statScale, false);
                enemySpawnedCount = 0;
            }
            if (enableSniper){
                SpawnObjects(enemySpawns, Sniper, ref enemySpawnedCount, dividedEnemyCount, statScale, false);
                enemySpawnedCount = 0;
            }
            SpawnObjects(enemySpawns, Basic, ref enemySpawnedCount, dividedEnemyCount, statScale, false);
        }
        else{
            SpawnObjects(enemySpawns, Basic, ref enemySpawnedCount, enemyCount, statScale, false);
        }
        SpawnObjects(chestSpawns, Chest, ref chestSpawnedCount, chestCount, 1f, false);
        SpawnObjects(portalSpawns, Portal, ref portalSpawnedCount, portalCount, 1f, false);
    }

<<<<<<< HEAD
    public class PerlinNoiseGenerator
    {
        private int[] permutation;
        private const int permutationSize = 10000;

        public PerlinNoiseGenerator(int seed)
        {
            permutation = new int[permutationSize];
            var rng = new System.Random(seed);
            for (int i = 0; i < permutationSize; i++)
            {
                permutation[i] = i;
            }
            for (int i = 0; i < permutationSize; i++)
            {
                int j = rng.Next(permutationSize);
                int temp = permutation[i];
                permutation[i] = permutation[j];
                permutation[j] = temp;
            }
        }

        private float Fade(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private float Grad(int hash, float x, float y)
        {
            int h = hash & 15;
            float u = h < 8 ? x : y;
            float v = h < 4 ? y : x;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        public float Perlin(float x, float y)
        {
            int X = (int)Mathf.Floor(x) & 255;
            int Y = (int)Mathf.Floor(y) & 255;

            x -= Mathf.Floor(x);
            y -= Mathf.Floor(y);

            float u = Fade(x);
            float v = Fade(y);

            int A = permutation[X] + Y;
            int AA = permutation[A];
            int AB = permutation[A + 1];
            int B = permutation[X + 1] + Y;
            int BA = permutation[B];
            int BB = permutation[B + 1];

            return Mathf.Lerp(
                Mathf.Lerp(Grad(AA, x, y), Grad(BA, x - 1, y), u),
                Mathf.Lerp(Grad(AB, x, y - 1), Grad(BB, x - 1, y - 1), u),
                v
            );
        }
    }

=======
>>>>>>> bd17dcf6538a948a8e8c4520e565fefe839de3d4
    void SpawnObjects(List<Vector3Int> spawnPositions, GameObject prefab, ref int spawnedCount, int totalCount, float scale, bool miniBoss){
        float miniBossScaling = 1;
        float miniBossScalingATK = 1f;
        if (miniBoss){
            miniBossScaling = 3f;
            miniBossScalingATK = 2f;
        }
        while (spawnedCount < totalCount && spawnPositions.Count > 0) {
            Vector3Int spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];
            spawnPositions.Remove(spawnPosition);

            GameObject obj = Instantiate(prefab, Tilemap.GetCellCenterWorld(spawnPosition), Quaternion.identity);
            if (miniBoss) {
                obj.name = "MiniBoss_" + prefab.name;
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null) {
                    spriteRenderer.color = Color.red;
                }
            }

            HealthManager healthManager = obj.GetComponent<HealthManager>();
            Enemy enemyScript = obj.GetComponent<Enemy>();
            EnemyChase speedScript = obj.GetComponent<EnemyChase>();
            Loot lootScript = obj.GetComponent<Loot>();

            if (healthManager != null) {
                healthManager.maxHealth *= (1 + statScale * (floorCount-1))*(miniBossScaling);
                healthManager.health = healthManager.maxHealth;
            }
            if (enemyScript != null){
                enemyScript.baseDamage *= (1 + statScale * (floorCount-1))*(miniBossScaling/miniBossScalingATK);
            }
            if (speedScript != null){
                if (enableSpeedScaling){
                    speedScript.moveForce *= (1 + statScale) ;
                }
            }
            if (lootScript != null){
                if (miniBoss){
                    lootScript.goldMin += 3;
                    lootScript.goldMax += 5;
                    lootScript.itemDropChance = 100f;
                }
            }
            spawnedCount++;
        }
    }
    void MoveWalls(){
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

    void PositionPlayer(){
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

            Player.transform.position = Tilemap.GetCellCenterWorld(playerSpawnPoint);

            Vector3 center = Tilemap.GetCellCenterWorld(playerSpawnPoint);
            CircleCollider2D collider = Player.GetComponent<CircleCollider2D>();
            float radius = collider.radius;

            Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(center, radius);
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
            RemoveSpawnPos(center, radius);

            enemyReposition = enemyCount - enemyRemoved;
            chestReposition = chestCount - chestRemoved;
            portalReposition = portalCount - portalRemoved;
            int miniBossCount = 0;
            int bossCount = 0;

            SpawnObjects(enemySpawns, Basic, ref enemyReposition, enemyCount, statScale, false);
            SpawnObjects(chestSpawns, Chest, ref chestReposition, chestCount, 1f, false);
            SpawnObjects(portalSpawns, Portal, ref portalReposition, portalCount, 1f, false);
            if (miniBossFloor){
                string chosenType = ChooseType();
                switch (chosenType) {
                    case "Charger":
                    SpawnObjects(enemySpawns, Charger, ref miniBossCount, 1, 1f, true);
                    break;
                    case "Fast":
                    SpawnObjects(enemySpawns, Fast, ref miniBossCount, 1, 1f, true);
                    break;
                    case "Ranged":
                    SpawnObjects(enemySpawns, RangedEnemy, ref miniBossCount, 1, 1f, true);
                    break;
                    case "Sniper":
                    SpawnObjects(enemySpawns, Sniper, ref miniBossCount, 1, 1f, true);
                    break;
                    case "Tank":
                    SpawnObjects(enemySpawns, Tank, ref miniBossCount, 1, 1f, true);
                    break;
                    case "Basic":
                    SpawnObjects(enemySpawns, Basic, ref miniBossCount, 1, 1f, true);
                    break;
                }
            }
            if (bossFloor){
                SpawnObjects(enemySpawns, Boss, ref bossCount, 1, 1f, false);
            }
        }
        else{
            Debug.LogWarning("bad");
        }
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

    void RemoveSpawnPos(Vector3 center, float radius){
        SpawnRemover(center, radius, enemySpawns);
        SpawnRemover(center, radius, chestSpawns);
        SpawnRemover(center, radius, portalSpawns);
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

    public void addKill(){
        killCount++;
    }

    public void CreatePortal(){
        Vector3Int centerTile = new Vector3Int(width / 2, height / 2, 0);

        Vector3 portalPosition = Tilemap.GetCellCenterWorld(centerTile);
        GameObject portalObject = Instantiate(Portal, portalPosition, Quaternion.identity);
    }

    public void ClearTiles() {
        Tilemap.ClearAllTiles();
        WallCollision.ClearAllTiles();
    }
    int DivideEnemyCount(){
    bool[] enemyTypes = new bool[] { enableCharger, enableFast, enableRanged, enableTank, enableSniper };
    
    int enabledEnemyTypes = 1;

    foreach (bool enabled in enemyTypes) {
        if (enabled) {
            enabledEnemyTypes++;
        }
    }

    int dividedEnemyCount = enemyCount / enabledEnemyTypes;
    return dividedEnemyCount;
    }
    public string ChooseType() {
        List<EnemyType> enabledTypes = new List<EnemyType>();

        if (enableCharger) {
            enabledTypes.Add(EnemyType.Charger);
        }
        if (enableFast) {
            enabledTypes.Add(EnemyType.Fast);
        }
        if (enableRanged) {
            enabledTypes.Add(EnemyType.Ranged);
        }
        if (enableTank) {
            enabledTypes.Add(EnemyType.Tank);
        }
        if (enableSniper) {
            enabledTypes.Add(EnemyType.Sniper);
        }

        if (enabledTypes.Count == 0) {
            return "Basic";
        }

        int randomIndex = UnityEngine.Random.Range(0, enabledTypes.Count);
        return enabledTypes[randomIndex].ToString();
    }
}
