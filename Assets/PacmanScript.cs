using UnityEngine;
using System.Collections;

public class PacmanScript : MonoBehaviour {
    public GameObject Ref;
	// Use this for initialization
	void Start () {
        GetComponent<InputScript>().MoveScript += OnMove;
	}

    private void OnMove(int x, int y)
    {
        GridManager gridManager = Ref.GetComponent<GridManager>();
        if (gridManager.grid[x, y] == GridManager.GridItem.Dot)
            gridManager.RemoveAt(x, y);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
