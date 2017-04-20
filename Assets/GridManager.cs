using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public struct GridPosition
{
    public int x;
    public int y;
    public GridPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override int GetHashCode()
    {
        return x * 10000 + y;
    }

    public override bool Equals(object obj)
    {
        if (obj is GridPosition)
            return this == (GridPosition)obj;
        else
            return false;
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return a.x != b.x || a.y != b.y;
    }

    public bool Adjacent(GridPosition other)
    {
        if (x == other.x)
        {
            if (Math.Abs(y - other.y) == 1)
            {
                return true;
            }
        }
        else if (y == other.y)
        {
            if (Math.Abs(x - other.x) == 1)
            {
                return true;
            }
        }

        return false;
    }
}

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    public GridItem[,] grid;
    public GameObject[] Prefabs = new GameObject[3];
    List<GameObject> prefabs = new List<GameObject>();

    public bool InBounds(int x, int y)
    {
        return (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1));
    }

    public bool IsFull(int x, int y)
    {
        return InBounds(x, y) && grid[x, y] == GridItem.Wall;
    }

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        InitWalls(pacmanThing);
    }
    
    public char[] gridRep = new char[]
    {
        ' ',
        '.',
        '#'
    };

    void InitWalls(string[] wallTemplate)
    {
        grid = new GridItem[wallTemplate[0].Length, wallTemplate.Length];
        int maxY = wallTemplate.Length-1;
        for(int y = 0; y < wallTemplate.Length; y++)
        {
            string line = wallTemplate[maxY-y];
            for(int x = 0; x < line.Length; x++)
            {
                GridItem item = grid[x, y] = (GridItem)Array.IndexOf(gridRep, line[x]);
                switch (item)
                {
                    case GridItem.Nothing:
                        break;
                    case GridItem.Dot:
                    case GridItem.Wall:
                        Spawn(x, y, item);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    internal void Spawn(int x, int y, GridItem item)
    {
        GameObject gridItem = Instantiate(Prefabs[(int)item]);
        gridItem.transform.position = new Vector3(x, y);
        prefabs.Add(gridItem);
	}

    public string[] pacmanThing;

    internal void ClearWalls()
    {
        foreach(GameObject g in prefabs)
        {
            Destroy(g);
        }
        prefabs.Clear();
    }

    public void RemoveAt (int x, int y)
    {
        var @object = prefabs.First(v => v.transform.position.x == x && y == v.transform.position.y);
        Destroy(@object);
        prefabs.Remove(@object);
        grid[x, y] = GridItem.Nothing;
    }

    public enum GridItem
    {
        Nothing,
        Dot,
        Wall
    }

    struct GridMove
    {
        public readonly GridPosition firstStep;
        public readonly GridPosition endPoint;
        public GridMove(GridPosition first, GridPosition end)
        {
            firstStep = first;
            endPoint = end;
        }
    }

    public GridPosition GetMove(int fromX, int fromY, int toX, int toY)
    {
        return GetMove(new GridPosition(fromX, fromY), new GridPosition(toX, toY));
    }

    GridPosition GetMove(GridPosition from, GridPosition to)
    {
        HashSet<GridPosition> visited = new HashSet<GridPosition>();
        Queue<GridMove> moves = new Queue<GridMove>();
        visited.Add(from);
        AddBaseMove(moves, new GridPosition(from.x + 1, from.y), visited);
        AddBaseMove(moves, new GridPosition(from.x - 1, from.y), visited);
        AddBaseMove(moves, new GridPosition(from.x, from.y + 1), visited);
        AddBaseMove(moves, new GridPosition(from.x, from.y - 1), visited);

        while (moves.Count > 0)
        {
            GridMove current = moves.Dequeue();
            visited.Add(current.endPoint);
            if (current.endPoint == to)
            {
                // since we're exploring in breadth-first order, the path we find first is the best possible.
                return current.firstStep;
            }

            AddMove(moves, current.firstStep, new GridPosition(current.endPoint.x + 1, current.endPoint.y), visited);
            AddMove(moves, current.firstStep, new GridPosition(current.endPoint.x - 1, current.endPoint.y), visited);
            AddMove(moves, current.firstStep, new GridPosition(current.endPoint.x, current.endPoint.y + 1), visited);
            AddMove(moves, current.firstStep, new GridPosition(current.endPoint.x, current.endPoint.y - 1), visited);
        }

        // if we failed to find a path, I have no idea what's happening... give up? Signal this failure somehow?
        return from;
    }

    void AddBaseMove(Queue<GridMove> moves, GridPosition firstStep, HashSet<GridPosition> visited)
    {
        AddMove(moves, firstStep, firstStep, visited);
    }

    void AddMove(Queue<GridMove> moves, GridPosition firstStep, GridPosition target, HashSet<GridPosition> visited)
    {
        if (IsFull(target.x, target.y))
            return;

        if (!visited.Contains(target))
            moves.Enqueue(new GridMove(firstStep, target));
    }
}
