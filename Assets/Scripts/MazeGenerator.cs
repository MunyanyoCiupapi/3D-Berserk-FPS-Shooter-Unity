using System.Collections;
using System.Collections.Generic;//Generic and Colelctions duomenu strukturos 1t
using System.Net.NetworkInformation;
using UnityEngine;


public sealed class MazeGenerator : MonoBehaviour
{
    [Range(5, 500)] public int mazeWidth = 5, mazeHeight = 5;
    public int startX, startY;


    private MazeCell[,] maze;
    private Vector2Int currentCell;


    private List<Vector2Int> directions = new List<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public MazeCell[,] GetMaze()
    {
        maze = new MazeCell[mazeWidth, mazeHeight] ?? new MazeCell[5, 5];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = maze[x, y] ?? new MazeCell(x, y); //Operatoriai ?? 0.5t
            }
        }

        CarvePath(startX, startY);

        return maze;
    }
  

    List<Vector2Int> GetShuffledDirections()
    {
        List<Vector2Int> shuffled = new List<Vector2Int>(directions);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            Vector2Int temp = shuffled[i];
            shuffled[i] = shuffled[rnd];
            shuffled[rnd] = temp;
        }
        return shuffled;
    }

    bool IsCellValid(Vector2Int cell)
    {
        if (cell.x < 0 || cell.y < 0 || cell.x >= mazeWidth || cell.y >= mazeHeight) return false;
        return !maze[cell.x, cell.y].visited;
    }


    void CarvePath(int startX, int startY)
    {
        currentCell = new Vector2Int(startX, startY);
        Stack<Vector2Int> pathStack = new Stack<Vector2Int>();

        maze[startX, startY].visited = true;

        while (true)
        {
            Vector2Int nextCell = FindNextCell();

            if (nextCell == currentCell) 
            {
                if (pathStack.Count == 0) break; 
                currentCell = pathStack.Pop(); 
            }
            else
            {
                BreakWalls(currentCell, nextCell);
                pathStack.Push(currentCell); 
                currentCell = nextCell;
                maze[currentCell.x, currentCell.y].visited = true;
            }
        }
    }


    Vector2Int FindNextCell()
    {
        foreach (Vector2Int direction in GetShuffledDirections())
        {
            Vector2Int neighbor = currentCell + direction;
            if (IsCellValid(neighbor)) return neighbor;
        }
        return currentCell; 
    }

    void BreakWalls(Vector2Int primary, Vector2Int secondary)
    {
        Vector2Int delta = secondary - primary;

        //switch su when 0.5
        switch (delta)
        {
            case Vector2Int _ when delta == Vector2Int.right:
                maze[primary.x, primary.y].rightWall = false;
                maze[secondary.x, secondary.y].leftWall = false;
                break;

            case Vector2Int _ when delta == Vector2Int.left:
                maze[primary.x, primary.y].leftWall = false;
                maze[secondary.x, secondary.y].rightWall = false;
                break;

            case Vector2Int _ when delta == Vector2Int.up:
                maze[primary.x, primary.y].topWall = false;
                maze[secondary.x, secondary.y].bottomWall = false;
                break;

            case Vector2Int _ when delta == Vector2Int.down:
                maze[primary.x, primary.y].bottomWall = false;
                maze[secondary.x, secondary.y].topWall = false;
                break;
        }
    }
}

public sealed class MazeCell //Sealed class 0.5
{
    public bool visited;
    public bool topWall = true, bottomWall = true, leftWall = true, rightWall = true;
    public int x, y;

    //Konstruktorius
    public MazeCell(int x, int y)
    {
        this.x = x;
        this.y = y;
        visited = false;
    }
}



