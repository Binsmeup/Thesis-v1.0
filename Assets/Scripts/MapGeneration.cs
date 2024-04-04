using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour{
    public Tilemap Tilemap;
    public Tile Floor;
    public Tile Wall;
    public Tile Object1;

    public GameObject Player;
    public GameObject Enemy;
    public GameObject Portal;
    public GameObject Chest;

    public Tilemap WallCollision;
    public Tilemap Item;

    public int floorCount;
    public int defaultWidth;
    public int defaultHeight;
    public int defaultDensity;
    public int defaultIteration;
    public int defaultEnemyCount;
    public float perlinScale = 0.1f;

    private int width;
    private int height;
    private int density;
    private int iteration;
    private int enemyCount;
    private int chestCount;
    private int portalCount = 1;
    private float statScale;

    private TileBase[,] grid;

    private List<Vector3Int> enemySpawns = new List<Vector3Int>();
    private List<Vector3Int> chestSpawns = new List<Vector3Int>();
    private List<Vector3Int> portalSpawns = new List<Vector3Int>();
    

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
            if (width >= 360){
                width = 360;
            }
        height = defaultHeight + (4 * floorCount);
        if (height >= 360){
                height = 360;
            }
        density = defaultDensity;
        iteration = defaultIteration;
        enemyCount = defaultEnemyCount + (3 * floorCount);
        if (enemyCount >= 210){
                enemyCount = 210;
            }
        switch (floorCount){
            case int n when n >= 1 && n <= 4:
                statScale = 0.1f;
                chestCount = 1;
            break;
            case int n when n >= 6 && n <= 9:
                statScale = 0.15f;
                chestCount = 2;
            break;
            case int n when n >= 11 && n <= 14:
                statScale = 0.2f;
                chestCount = 3;
            break;
            case int n when n >= 16 && n <= 19:
                statScale = 0.25f;
                chestCount = 4;
            break;
            case 5:
                statScale = 0.1f;
                chestCount = 0;
                width = 40;
                height = 40;
                density = 50;
                iteration = 10;
                enemyCount = 0;
                chestCount = 0;
            break;
            case 10:
                statScale = 0.15f;
                chestCount = 0;
                width = 40;
                height = 40;
                density = 50;
                iteration = 10;
                enemyCount = 0;
                chestCount = 0;
            break;
            case 15:
                statScale = 0.2f;
                chestCount = 0;
                width = 40;
                height = 40;
                density = 50;
                iteration = 10;
                enemyCount = 0;
                chestCount = 0;
            break;
            case 20:
                statScale = 0.25f;
                chestCount = 0;
                width = 40;
                height = 40;
                density = 50;
                iteration = 10;
                enemyCount = 0;
                chestCount = 0;
            break;

            default:
                statScale = 0.3f + ((floorCount-20)*0.01f);
                chestCount = 0;
            break;
        }   
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
        int enemySpawnedCount = 0;
        int chestSpawnedCount = 0;
        int portalSpawnedCount = 0;
        enemySpawns.Clear();
        chestSpawns.Clear();
        portalSpawns.Clear();

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (grid[x, y] == Floor){
                    float perlinValue = Mathf.PerlinNoise(x * perlinScale, y * perlinScale);
                    float modifiedValue = perlinValue * 3f; 
                    float randomChance = Random.Range(0f, 100f);

                    if (modifiedValue >= 0 && modifiedValue < 1 && randomChance <= 5f){
                        Item.SetTile(tilePosition, Object1);
                    }
                    else if (modifiedValue >= 1 && modifiedValue < 2){
                        enemySpawns.Add(tilePosition);
                    }
                    else if (modifiedValue >= 2 && modifiedValue < 3 && randomChance <= 15f){
                        //coin
                    }
                    else if (modifiedValue >= 2 && modifiedValue < 3){
                        chestSpawns.Add(tilePosition);
                        portalSpawns.Add(tilePosition);
                    }
                }
            }
        }

        SpawnObjects(enemySpawns, Enemy, ref enemySpawnedCount, enemyCount, statScale);
        SpawnObjects(chestSpawns, Chest, ref chestSpawnedCount, chestCount, 1f);
        SpawnObjects(portalSpawns, Portal, ref portalSpawnedCount, portalCount, 1f);
    }

    void SpawnObjects(List<Vector3Int> spawnPositions, GameObject prefab, ref int spawnedCount, int totalCount, float scale){
        while (spawnedCount < totalCount && spawnPositions.Count > 0) {
            Vector3Int spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];
            spawnPositions.Remove(spawnPosition);

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
            Vector3Int randomFloorTile;

            do{
                randomFloorTile = floorTiles[Random.Range(0, floorTiles.Count)];
            } while (HasWallsInRadius(randomFloorTile));

            Player.transform.position = Tilemap.GetCellCenterWorld(randomFloorTile);

            Vector3 center = Tilemap.GetCellCenterWorld(randomFloorTile);
            CircleCollider2D collider = Player.GetComponent<CircleCollider2D>();
            float radius = collider.radius;

            Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(center, radius);
            foreach (Collider2D nearbyObject in nearbyObjects){
                if ((nearbyObject.CompareTag("Enemy") || nearbyObject.CompareTag("Chest") || nearbyObject.CompareTag("Portal"))
                    && !removedObjects.Contains(nearbyObject.gameObject)){
                    Debug.Log("Removing nearby object: " + nearbyObject.gameObject.name);
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

            SpawnObjects(enemySpawns, Enemy, ref enemyReposition, enemyCount, statScale);
            SpawnObjects(chestSpawns, Chest, ref chestReposition, chestCount, 1f);
            SpawnObjects(portalSpawns, Portal, ref portalReposition, portalCount, 1f);
        }
        else{
            Debug.LogWarning("bad");
        }
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


    public void ClearTiles() {
        Tilemap.ClearAllTiles();
        WallCollision.ClearAllTiles();
        Item.ClearAllTiles();
    }
}
