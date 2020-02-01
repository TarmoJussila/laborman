using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPanel : MonoBehaviour
{
    public bool Covered = true;
    private Rigidbody coverRb;

    public void DetachCover()
    {
        Covered = false;
        coverRb.isKinematic = false;
    }

    public void AttachCover(GameObject Cover)
    {
        Cover.transform.position = transform.position;
        Cover.transform.forward = transform.forward;
        Covered = true;
        coverRb.isKinematic = true;
    }
}
