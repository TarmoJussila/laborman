using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fuse
{
    public bool On = false;
    public MeshRenderer MeshRenderer;
    public int FuseIndex = -1;
    public int PairedFuseIndex = -2;

    private Fuse pairedFuse = null;

    public void Initialize(Fuse pairedFuse, int fuseIndex, int pairedFuseIndex)
    {
        this.pairedFuse = pairedFuse;
        FuseIndex = fuseIndex;
        PairedFuseIndex = pairedFuseIndex;
    }

    public void Toggle(Material onMaterial, Material offMaterial, float onPositionY, float offPositionY, bool doTogglePairedFuse = true)
    {
        On = !On;
        MeshRenderer.material = On ? onMaterial : offMaterial;
        MeshRenderer.transform.localPosition = new Vector3(MeshRenderer.transform.localPosition.x, On ? onPositionY : offPositionY,
            MeshRenderer.transform.localPosition.z);

        if (doTogglePairedFuse && FuseIndex != PairedFuseIndex && PairedFuseIndex >= 0 && pairedFuse != null)
        {
            if (Random.Range(0, 2) == 0)
            {
                pairedFuse.Toggle(onMaterial, offMaterial, onPositionY, offPositionY, false);
            }
        }
    }
}

public class ElectricPuzzle : Puzzle
{
    [SerializeField] private List<Fuse> fuses = new List<Fuse>();
    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;
    [SerializeField] private float onPositionY;
    [SerializeField] private float offPositionY;

    private bool isSolved = false;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < 5; i++)
        {
            int siblingIndex = transform.GetSiblingIndex();
            int originalSeed = Random.seed;
            Random.seed = originalSeed + siblingIndex + i;

            var randomFuseIndex = Random.Range(0, 5);
            fuses[i].Initialize(fuses[randomFuseIndex], i, randomFuseIndex);

            Random.seed = originalSeed;
        }

        // Scramble fuse box at start.
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < Random.Range(1, 3); j++)
            {
                fuses[i].Toggle(onMaterial, offMaterial, onPositionY, offPositionY);
            }
        }

        // Make sure all not on at start.
        bool areAllOn = true;
        for (int i = 0; i < 5; i++)
        {
            if (!fuses[i].On)
            {
                areAllOn = false;
                break;
            }
        }

        if (areAllOn)
        {
            fuses[2].Toggle(onMaterial, offMaterial, onPositionY, offPositionY);
        }
    }

    public void ToggleFuse(string fuseName)
    {
        if (isSolved)
        {
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            if (fuseName.EndsWith((i + 1).ToString()))
            {
                fuses[i].Toggle(onMaterial, offMaterial, onPositionY, offPositionY);
                break;
            }
        }

        AudioController.Instance.PlayElectricSoundClip();

        bool areAllOn = true;
        for (int i = 0; i < 5; i++)
        {
            if (!fuses[i].On)
            {
                areAllOn = false;
                break;
            }
        }

        if (areAllOn)
        {
            isSolved = true;
            Solved();
        }
    }
}