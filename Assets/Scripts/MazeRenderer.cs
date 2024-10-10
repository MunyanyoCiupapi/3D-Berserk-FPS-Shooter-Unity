using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.AI.Navigation;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject mazeCellPrefab;
    [SerializeField] NavMeshSurface navMeshSurface;
    [SerializeField] GameObject enemyPrefab; 

    public float cellSize = 1f;
    public int numberOfEnemies;

    private void Start()
    {
        RenderMaze();
        BakeNavMesh();
        SpawnEnemies();
    }

    void RenderMaze()
    {
        MazeCell[,] maze = mazeGenerator.GetMaze();

        Action<MazeCell, GameObject> initializeCell = (cell, newCell) => //lambda funkcija 1t. 
        {
            MazeCellObject mazeCellObject = newCell.GetComponent<MazeCellObject>();
            mazeCellObject?.Init(cell.topWall, cell.bottomWall, cell.leftWall, cell.rightWall); 
        };

        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                MazeCell cell = maze[x, y];
                GameObject newCell = Instantiate(mazeCellPrefab, new Vector3(x * cellSize, 0f, y * cellSize), Quaternion.identity, transform);
                initializeCell(cell, newCell);
            }
        }
    }
    void BakeNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh(); 
        }
    }
    void SpawnEnemies()
    {
        MazeCell[,] maze = mazeGenerator.GetMaze();
        int enemiesSpawned = 0;

        while (enemiesSpawned < numberOfEnemies)
        {
            int randomX = UnityEngine.Random.Range(0, mazeGenerator.mazeWidth);
            int randomY = UnityEngine.Random.Range(0, mazeGenerator.mazeHeight);
            MazeCell randomCell = maze[randomX, randomY];

            if (!randomCell.visited || !IsCellOpenForSpawning(randomCell)) continue;

            Vector3 spawnPosition = new Vector3(randomX * cellSize, 0, randomY * cellSize);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            enemiesSpawned++;
        }
    }

    bool IsCellOpenForSpawning(MazeCell cell)
    {
        return !(cell.topWall && cell.bottomWall && cell.leftWall && cell.rightWall);
    }
}

