using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public AIScript AI;
    public bool Rotate = true;
    public float Speed;

    Vector3 movingFrom;
    Vector3 movingTo;
    float lerpFraction = 1;
    float frameStep { get { return Speed * Time.fixedDeltaTime; } }

    public event Action<int, int> MoveListener;

    void Start()
    {
        movingFrom = new Vector3((int)transform.position.x, (int)transform.position.y);
        movingTo = movingFrom;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lerpFraction < 1)
        {
            transform.position = Vector3.Lerp(movingFrom, movingTo, lerpFraction);
            lerpFraction += frameStep;

            if (lerpFraction < 1)
                return;

            if (MoveListener != null)
                MoveListener.Invoke((int)movingTo.x, (int)movingTo.y);
        }

        GridPosition move = AI.GetMove();

        if (move.x == 0 && move.y == 0)
        {
            transform.position = movingTo;
            lerpFraction = 1;
            return;
        }

        movingFrom = transform.position;
        if (move.x == 0)
            movingFrom.x = (float)Math.Round(transform.position.x);
        if (move.y == 0)
            movingFrom.y = (float)Math.Round(transform.position.y);

        movingTo = new Vector3((float)Math.Round(movingFrom.x) + move.x, (float)Math.Round(movingFrom.y) + move.y);

        lerpFraction = 0;

        if (Rotate)
        {
            int[] degrees = new int[] { -1, 180, -1, 270, -1, 90, -1, 0, -1 };
            int dirId = move.x * 3 + move.y + 4;
            transform.rotation = Quaternion.Euler(0, 0, degrees[(int)dirId]);
        }
    }
}
