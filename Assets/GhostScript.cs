using UnityEngine;
using System.Collections;

public class GhostScript : AIScript
{
    public GameObject player;
    public GridManager gridManager;

    public override GridPosition GetMove()
    {
        GridPosition position = this.position;
        GridPosition targetPos = gridManager.GetMove(
            position.x, position.y,
            Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y)
        );

        return new GridPosition(targetPos.x - position.x, targetPos.y - position.y);
    }
}