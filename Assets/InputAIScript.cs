using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class InputAIScript : AIScript
{
    public GridManager Grid;
    public string Horizontal = "Horizontal";
    public string Vertical = "Vertical";

    public override GridPosition GetMove()
    {
        float xT = Input.GetAxis(Horizontal);
        float yT = Input.GetAxis(Vertical);
        float absX = Math.Abs(xT);
        float absY = Math.Abs(yT);
        int xR;
        int yR;
        if (absX > absY)
        {
            xR = (int)Math.Ceiling(xT);
            yR = 0;
        }
        else
        {
            xR = 0;
            yR = (int)Math.Ceiling(yT);
        }

        if (GridManager.instance.IsFull(xR + (int)Math.Round(transform.position.x), yR + (int)Math.Round(transform.position.y)))
            return new GridPosition(0,0);

        return new GridPosition(xR, yR);
	}
}
