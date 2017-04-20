using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIMove // (xR * 3 + yR) + 4
{
    SouthWest = 0,
    West = 1,
    NorthWest = 2,

    South = 3,
    None = 4,
    North = 5,

    SouthEast = 6,
    East = 7,
    NorthEast = 8,
}

public abstract class AIScript : MonoBehaviour
{
    public abstract GridPosition GetMove();
}
