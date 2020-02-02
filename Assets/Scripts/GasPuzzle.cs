using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasPuzzle : Puzzle
{
    public GasPuzzleValve[] Valves;

    public Transform Needle;

    private int[] Powers;

    public int TargetPower;

    public int CurrentTotalPower = 0;

    public float RisktimeLeft = 10.0f;

    public GameObject RedLight;
    public Material GreenLight;

    public ParticleSystem Explosion;

    public bool Lock = false;

    public float needlePos = 0.0f;

    private float needleMinX = -0.18f;
    private float needleMaxX = 0.19f;

    private Vector3 needleLocation;

    private void Start()
    {
        List<GasPuzzleValve> UnusedValves = new List<GasPuzzleValve>(Valves);
        List<int> UnusedPowers = new List<int>(Powers);

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
        if (CurrentTotalPower > TargetPower + 1 && !Lock)
        {
            RisktimeLeft -= Time.deltaTime;
            if (RisktimeLeft <= 0)
            {
                Lock = true;
                //Explosion.Play();
                AudioController.Instance.PlayAngrySoundClip();
                foreach (GasPuzzleValve valve in Valves)
                {
                    valve.Locked = true;
                }
            }
        }

        needleLocation = Needle.transform.position;
        needleLocation.x = Mathf.Lerp(needleMinX, needleMaxX, needlePos);
        Needle.transform.position = Vector3.Lerp(Needle.transform.position, needleLocation, 0.5f);

    }

    public void CheckCompletion()
    {
        if (Lock) return;
        CurrentTotalPower = 0;
        foreach (GasPuzzleValve valve in Valves)
        {
            if (valve.Active) CurrentTotalPower += valve.Power;
        }

        needlePos = Mathf.InverseLerp(0, TargetPower + 1, CurrentTotalPower);

        RedLight.GetComponent<MeshRenderer>().material = GreenLight;
        if (CurrentTotalPower == TargetPower) Solved();
    }
}
