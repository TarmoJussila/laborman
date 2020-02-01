using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenovationController : MonoBehaviour
{
    public static RenovationController Instance { get; private set; }

    [SerializeField]
    private int TotalPuzzles;

    List<HouseWall> Walls = new List<HouseWall>();

    public GameObject[] PuzzlePrefabs;

    private List<Puzzle> UnsolvedPuzzles = new List<Puzzle>();

    private void Awake()
    {
        RenovationController.Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        RandomizeWalls();
    }

    public void RegisterWall(HouseWall wall)
    {
        Walls.Add(wall);
    }

    private void RandomizeWalls()
    {
        List<HouseWall> UnusedWalls = new List<HouseWall>(Walls);
        for (int i = 0; i < TotalPuzzles; i++)
        {
            int index = Random.Range(0, UnusedWalls.Count);
            HouseWall wall = UnusedWalls[index];
            UnusedWalls.Remove(wall);
            Puzzle puzzle = wall.SpawnPuzzle();
            UnsolvedPuzzles.Add(puzzle);
        }
    }

    public void SolvePuzzle(Puzzle puzzle)
    {
        UnsolvedPuzzles.Remove(puzzle);
    }



}
