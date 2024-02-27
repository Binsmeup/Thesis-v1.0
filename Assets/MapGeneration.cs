using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    public Tilemap Tilemap;
    public Tile Floor;
    public Tile Wall;
    public Tile Object1;
    public Tile Object2;

    public Tilemap WallCollision;


    public int width;
    public int height;
    public int density;
    public int iteration;
    public float perlinScale = 0.1f;

    private TileBase[,] grid;

    void Start()
    {
        CreateGrid();
        GenerateNoise();
        ApplyCellularAutomata();
        FloodFill();
        ApplyPerlinNoise();
        MoveWalls();
    }

    void CreateGrid()
    {
        grid = new TileBase[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                grid[x, y] = Floor;
                Tilemap.SetTile(tilePosition, Floor);
            }
        }
    }

    void GenerateNoise()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (Random.Range(0, 100) < density)
                {
                    grid[x, y] = Wall;
                    Tilemap.SetTile(tilePosition, Wall);
                }
            }
        }
    }

    void ApplyCellularAutomata()
{
    for (int currentIteration = 0; currentIteration < iteration; currentIteration++)
    {
        TileBase[,] newGrid = CopyGrid();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighborWallCount = CountNeighboringWalls(newGrid, x, y);
                bool hasOutOfBoundsNeighbor = HasOutOfBoundsNeighbor(x, y);

                if (neighborWallCount > 4 || hasOutOfBoundsNeighbor)
                {
                    grid[x, y] = Wall;
                }
                else
                {
                    grid[x, y] = Floor;
                }
            }
        }

        UpdateGrid();
    }
}
    private bool HasOutOfBoundsNeighbor(int x, int y)
{
    for (int i = x - 1; i <= x + 1; i++)
    {
        for (int j = y - 1; j <= y + 1; j++)
        {
            if ((i != x || j != y) && (i < 0 || i >= width || j < 0 || j >= height))
            {
                return true;
            }
        }
    }

    return false;
}

    private int CountNeighboringWalls(TileBase[,] currentGrid, int x, int y)
    {
        int wallCount = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if ((i != x || j != y))
                {
                    if (i >= 0 && i < width && j >= 0 && j < height)
                    {
                        if (currentGrid[i, j] == Wall)
                        {
                            wallCount++;
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }
        }

        return wallCount;
    }

    private TileBase[,] CopyGrid()
    {
        TileBase[,] newGrid = new TileBase[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                newGrid[x, y] = grid[x, y];
            }
        }

        return newGrid;
    }

    void UpdateGrid()
    {
        Tilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                Tilemap.SetTile(tilePosition, grid[x, y]);
            }
        }
    }
 void FloodFill()
    {
        List<List<Vector2Int>> groups = new List<List<Vector2Int>>();

        bool[,] visited = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                visited[x, y] = grid[x, y] == Wall;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!visited[x, y])
                {
                    List<Vector2Int> group = new List<Vector2Int>();
                    FloodFillConnect(x, y, visited, group);
                    groups.Add(group);
                }
            }
        }

        List<Vector2Int> largestGroup = new List<Vector2Int>();
        int largestGroupIndex = -1;

        for (int i = 0; i < groups.Count; i++)
        {
            if (groups[i].Count > largestGroup.Count)
            {
                largestGroup = groups[i];
                largestGroupIndex = i;
            }
        }

        if (largestGroupIndex != -1)
        {
            groups.RemoveAt(largestGroupIndex);

            foreach (List<Vector2Int> group in groups)
            {
                foreach (Vector2Int cell in group)
                {
                    grid[(int)cell.x, (int)cell.y] = Wall;
                }
            }
        }

        UpdateGrid();
    }

    void FloodFillConnect(int x, int y, bool[,] visited, List<Vector2Int> group)
    {
        if (x < 0 || x >= width || y < 0 || y >= height || visited[x, y])
        {
            return;
        }

        visited[x, y] = true;
        group.Add(new Vector2Int(x, y));

        FloodFillConnect(x + 1, y, visited, group);
        FloodFillConnect(x - 1, y, visited, group);
        FloodFillConnect(x, y + 1, visited, group);
        FloodFillConnect(x, y - 1, visited, group);
    }
    void ApplyPerlinNoise()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (grid[x, y] == Floor)
                {
                    float perlinValue = Mathf.PerlinNoise(x * perlinScale, y * perlinScale);
                    float modifiedValue = perlinValue * 3f; 
                    float randomChance = Random.Range(0f, 100f);

                    if (modifiedValue >= 0 && modifiedValue < 1 && randomChance <= 5f)
                    {
                        Tilemap.SetTile(tilePosition, Object1);
                    }
                    else if (modifiedValue >= 1 && modifiedValue < 2 && randomChance <= 10f)
                    {
                        Tilemap.SetTile(tilePosition, Object1);
                    }
                    else if (modifiedValue >= 2 && modifiedValue < 3 && randomChance <= 15f)
                    {
                        Tilemap.SetTile(tilePosition, Object2);
                    }
                    else if (modifiedValue == 3 && randomChance <= 30f)
                    {
                        Tilemap.SetTile(tilePosition, Object2);
                    }
                }
            }
        }
    }
void MoveWalls()
{
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            Vector3Int tilePosition = new Vector3Int(x, y, 0);

            TileBase currentTile = Tilemap.GetTile(tilePosition);

            if (currentTile != null && currentTile.name == Wall.name)
            {
                WallCollision.SetTile(tilePosition, currentTile);
                Tilemap.SetTile(tilePosition, null);
            }
        }
    }
}
}
