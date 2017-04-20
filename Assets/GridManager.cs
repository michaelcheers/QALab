using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    public GridItem[,] grid;
    public GameObject[] Prefabs = new GameObject[3];
    List<GameObject> prefabs = new List<GameObject>();

    public bool IsFull (int x, int y)
    {
        if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
            return grid[x, y] == GridItem.Wall;
        else
            return true;
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
}
