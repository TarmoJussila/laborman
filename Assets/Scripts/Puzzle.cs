using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public bool LockWall;

    public void Solved()
    {
        AudioController.Instance.PlayPleasedSoundClip();
        RenovationController.Instance.SolvePuzzle(this);
    }
}
