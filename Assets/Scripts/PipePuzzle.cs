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

    [SerializeField]
    Material greenMat;
    [SerializeField]
    GameObject redLight;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PuzzleBlocks.Length; i++)
        {
            PipePuzzleBlock block = PuzzleBlocks[i].GetComponent<PipePuzzleBlock>();
            block.RelatedPuzzle = this;
            Blocks[i % 4, (int)Mathf.Floor(i / 4)] = block;
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

    public void CheckCompletion()
    {
        Side previousExit = Side.Down;
        bool keepChecking = true;
        bool win = false;
        int[] lastCell = new int[] { 0, 0 };
        while (keepChecking)
        {
            int[] nextCell = GetNextCell(lastCell, previousExit);

            if (nextCell[0] == 3 && nextCell[1] == 3)
            {
                win = true;
                keepChecking = false;
                break;
            }
            PipePuzzleBlock block = Blocks[nextCell[0], nextCell[1]];
            Side required = PipePuzzleBlock.GetOpposite(previousExit);
            bool match = block.Sides.IndexOf(required) > -1;
            if (match)
            {
                previousExit = block.Sides.Find(item => item != required);
                lastCell = nextCell;
                keepChecking = true;
            }
            else
            {
                keepChecking = false;
            }
        }
        if (win)
        {
            foreach (PipePuzzleBlock block in Blocks)
            {
                if (block.PipeObject)
                    block.PipeObject.GetComponent<BoxCollider>().enabled = false;
            }
            redLight.GetComponent<MeshRenderer>().material = greenMat;
            Solved();
        }
    }

    public int[] GetNextCell(int[] current, Side exit)
    {
        switch (exit)
        {
            case Side.Up:
                {
                    current[1]--;
                    break;
                }
            case Side.Right:
                {
                    current[0]++;
                    break;
                }
            case Side.Down:
                {
                    current[1]++;
                    break;
                }
            case Side.Left:
                {
                    current[0]--;
                    break;
                }
        }
        return current;
    }

}

public struct Coordinate
{
    int x;
    int y;
}
