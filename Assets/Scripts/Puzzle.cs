using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public void Solved()
    {
        RenovationController.Instance.SolvePuzzle(this);
    }
}
