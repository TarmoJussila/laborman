﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fuse
{
    public bool On = false;
    public MeshRenderer MeshRenderer;

    public void Toggle(Material onMaterial, Material offMaterial)
    {
        On = !On;
        MeshRenderer.material = On ? onMaterial : offMaterial;
    }
}

public class ElectricPuzzle : Puzzle
{
    [SerializeField] private List<Fuse> fuses = new List<Fuse>();
    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;

    public void ToggleFuse(string fuseName)
    {
        for (int i = 0; i < 5; i++)
        {
            if (fuseName.EndsWith((i + 1).ToString()))
            {
                fuses[i].Toggle(onMaterial, offMaterial);
            }
        }
    }
}
