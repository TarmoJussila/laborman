using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePipe : MonoBehaviour
{
    private Rigidbody rb;

    public PipePuzzleBlock AttachedBlock;

    public bool Placed = false;
    public bool Grabbed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PlacePipe(PipePuzzleBlock block)
    {
        AttachedBlock = block;
        Placed = true;
        rb.isKinematic = true;
    }

    public void DetachPipe()
    {
        if (AttachedBlock)
            AttachedBlock.DetachPipe();

        AttachedBlock = null;
        Placed = false;
        rb.isKinematic = false;
    }
}
