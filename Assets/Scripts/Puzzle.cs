using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public bool LockWall;

    public void Solved()
    {
        RenovationController.Instance.SolvePuzzle(this);
    }
}
