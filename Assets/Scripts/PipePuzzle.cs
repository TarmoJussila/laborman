using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzle : Puzzle
{
    [SerializeField]
    GameObject[] PuzzleBlocks;
    PipePuzzleBlock[,] Blocks = new PipePuzzleBlock[4, 4];

    [SerializeField]
    PipePuzzleBlock[] PossibleObstacles;

    [SerializeField]
    PipePuzzleBlock[] AlwaysEmpty;

    [SerializeField]
    PipePuzzleBlock[] Empty;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PuzzleBlocks.Length; i++)
        {
            Blocks[i % 4, (int)Mathf.Floor(i / 4)] = PuzzleBlocks[i].GetComponent<PipePuzzleBlock>();
        }

        Blocks[0, 0].SetType(BlockType.Start);
        Blocks[3, 3].SetType(BlockType.End);

        foreach (PipePuzzleBlock block in Empty)
        {
            block.SetType(BlockType.Empty);
        }

        foreach (PipePuzzleBlock block in AlwaysEmpty)
        {
            block.SetType(BlockType.AlwaysEmpty);
        }

        for (int i = 0; i < 2; i++)
        {
            PossibleObstacles[Random.Range(0, PossibleObstacles.Length)].SetType(BlockType.Obstacle);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public struct Coordinate
{
    int x;
    int y;
}
