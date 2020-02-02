using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasPuzzleValve : MonoBehaviour
{
    public int Power;
    public bool Active;
    public GasPuzzle RelatedPuzzle;
    public bool Locked;

    private bool rotating;

    public void Interact()
    {
        if (rotating || Locked) return;

        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        AudioController.Instance.PlayValveSoundClip();
        float dir = Active ? -1.0f : 1.0f;
        rotating = true;
        float t = 0.0f;
        Quaternion initial = transform.rotation;
        Quaternion target = initial;
        Vector3 euler = target.eulerAngles;
        euler.z += dir * 180;
        target.eulerAngles = euler;

        while (t < 1.0f)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(initial, target, t);
            yield return null;
        }

        rotating = false;
        Active = !Active;
        Debug.Log(RelatedPuzzle);
        RelatedPuzzle.CheckCompletion();
    }


}
