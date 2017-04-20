using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class InputScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }

    public string Horizontal = "Horizontal";
    public string Vertical = "Vertical";
    public bool Rotate = true;
    int hiddenAmountX = 0;
    int hiddenAmountY = 0;
    int frameMoveTimer = 0;
    List<int> rotations = new List<int>();
    int currentRotation = -1;
    const int animationTime = 10;
    bool lastFrameInput = false;
    public event Action<int, int> MoveScript;
	
	// Update is called once per frame
	void Update () {

        if (frameMoveTimer == 0)
        {
            transform.position = new Vector3((float)Math.Round(transform.position.x), (float)Math.Round(transform.position.y));
            MoveScript?.Invoke((int)transform.position.x, (int)transform.position.y);
            if (rotations.Count != 0)
            {
                currentRotation = rotations[rotations.Count - 1];
                rotations.RemoveAt(rotations.Count - 1);
                frameMoveTimer = animationTime;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x - ((float)Math.Round(Math.Sin((currentRotation * Math.PI) / 180)) / animationTime),
                                             transform.position.y + ((float)Math.Round(Math.Cos((currentRotation * Math.PI) / 180)) / animationTime));
            frameMoveTimer--;
            return;
        }
        float xT = Input.GetAxis(Horizontal);
        float yT = Input.GetAxis(Vertical);
        float absX = Math.Abs(xT);
        float absY = Math.Abs(yT);
        int xR = (absY > absX) ? 0 : (int)Math.Ceiling(xT);
        int yR = (absX > absY) ? 0 : (int)Math.Ceiling(yT);
        if (GridManager.instance.IsFull(xR + (int)Math.Round(transform.position.x), yR + (int)Math.Round(transform.position.y)))
            return;
        if (absX == absY)
        {
            hiddenAmountX = 0;
            hiddenAmountY = 0;
            return;
        }
        int dirId = (xR * 3 + yR) + 3;
        int[] degrees = new int[] { 180, -1, 270, -1, 90, -1, 0, -1 };
        int rotation = degrees[dirId];
        if (xR == 0 && yR == 0)
        {
            hiddenAmountX = 0;
            hiddenAmountY = 0;
        }
        if (!lastFrameInput && (xR != 0 || yR != 0))
        {
            hiddenAmountX = xR * 5;
            hiddenAmountY = yR * 5;
            lastFrameInput = true;
        }
        else
        {
            hiddenAmountX += xR;
            hiddenAmountY += yR;
            lastFrameInput = false;
        }
        if (hiddenAmountX / 5 != 0 || hiddenAmountY / 5 != 0)
        {
            rotations.Clear();
            rotations.Add(rotation - 90);
        }
        hiddenAmountX %= 5;
        hiddenAmountY %= 5;
        if (rotation == -1)
            return;
        if (Rotate)
        transform.rotation = Quaternion.Euler(0, 0, rotation);
	}
}
