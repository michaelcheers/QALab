using UnityEngine;
using System.Collections;

public class PacmanScript : MonoBehaviour
{
    public GameObject GridManager;

	// Use this for initialization
	void Start () {
        GetComponent<MovementScript>().MoveListener += OnMove;
	}

    private void OnMove(int x, int y)
    {
        GridManager gridManager = GridManager.GetComponent<GridManager>();
        if (gridManager.grid[x, y] == global::GridManager.GridItem.Dot)
            gridManager.RemoveAt(x, y);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
