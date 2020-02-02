using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasPuzzle : Puzzle
{
    public GasPuzzleValve[] Valves;

    public Transform Needle;

    public int[] Powers;

    public int TargetPower;

    public int CurrentTotalPower = 0;

    public float RisktimeLeft = 15.0f;

    public GameObject RedLight;
    public Material GreenLight;

    public ParticleSystem Explosion;

    public bool Lock = false;

    public float needlePos = 0.0f;

    private float needleSafeMinX = -0.18f;
    private float needleSafeMaxX = 0.027f;

    private float needleRiskMinX = 0.11f;
    private float needleRiskMaxX = 0.19f;

    private float needleCorrectX = 0.075f;


    private Vector3 needleLocation;

    private void Start()
    {
        needleLocation = Needle.transform.localPosition;
        List<GasPuzzleValve> UnusedValves = new List<GasPuzzleValve>(Valves);
        List<int> UnusedPowers = new List<int>(Powers);

        TargetPower = Random.Range(1, 11);

        while (UnusedValves.Count > 0)
        {
            GasPuzzleValve valve = UnusedValves[0];
            UnusedValves.Remove(valve);
            int index = Random.Range(0, UnusedPowers.Count);
            valve.Power = UnusedPowers[index];
            UnusedPowers.RemoveAt(index);
            valve.RelatedPuzzle = this;
        }
    }

    private void Update()
    {
        /*if (CurrentTotalPower > TargetPower + 1 && !Lock)
        {
            RisktimeLeft -= Time.deltaTime;
            if (RisktimeLeft <= 0)
            {
                Debug.LogWarning("locked!");
                Lock = true;
                //Explosion.Play();
                AudioController.Instance.PlayAngrySoundClip();
                foreach (GasPuzzleValve valve in Valves)
                {
                    valve.Locked = true;
                }
            }
        }*/

        Needle.transform.localPosition = Vector3.Lerp(Needle.transform.localPosition, needleLocation, 0.5f);

    }

    public void CheckCompletion()
    {
        if (Lock) return;
        CurrentTotalPower = 0;
        foreach (GasPuzzleValve valve in Valves)
        {
            if (valve.Active) CurrentTotalPower += valve.Power;
        }

        needlePos = Mathf.Clamp01(Mathf.InverseLerp(0, TargetPower + 4, CurrentTotalPower));
        needleLocation = Needle.transform.localPosition;
        if (CurrentTotalPower < TargetPower)
        {
            float needleSafeLocation = Mathf.InverseLerp(0, TargetPower - 1, CurrentTotalPower);
            needleLocation.x = Mathf.Lerp(needleSafeMinX, needleSafeMaxX, needleSafeLocation);
        }
        else if (CurrentTotalPower == TargetPower)
        {
            needleLocation.x = needleCorrectX;
            RedLight.GetComponent<MeshRenderer>().material = GreenLight;
            if (CurrentTotalPower == TargetPower) Solved();
        }
        else if (CurrentTotalPower > TargetPower)
        {
            float needleRiskLocation = Mathf.InverseLerp(TargetPower + 1, 10, CurrentTotalPower);
            needleLocation.x = Mathf.Lerp(needleRiskMinX, needleRiskMaxX, needleRiskLocation);
        }

    }
}
