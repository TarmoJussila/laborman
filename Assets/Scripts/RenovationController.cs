using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenovationController : MonoBehaviour
{
    public static RenovationController Instance { get; private set; }

    [SerializeField]
    private int WallPuzzles;
    [SerializeField]
    private int RoomPuzzles;

    List<HouseWall> Walls = new List<HouseWall>();
    List<PuzzleRoom> Rooms = new List<PuzzleRoom>();

    public GameObject[] PuzzlePrefabs;
    public GameObject[] RoomPrefabs;
    public GameObject[] RoomPuzzlePrefabs;

    [SerializeField]
    private List<Puzzle> UnsolvedPuzzles = new List<Puzzle>();

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        RandomizeWalls();
        RandomizeRooms();
    }

    public void RegisterWall(HouseWall wall)
    {
        Walls.Add(wall);
    }

    public void RegisterRoom(PuzzleRoom room)
    {
        Rooms.Add(room);
    }

    private void RandomizeWalls()
    {
        List<HouseWall> UnusedWalls = new List<HouseWall>(Walls);
        for (int i = 0; i < WallPuzzles; i++)
        {
            int index = Random.Range(0, UnusedWalls.Count);
            HouseWall wall = UnusedWalls[index];
            UnusedWalls.Remove(wall);
            Puzzle puzzle = wall.SpawnPuzzle();
            UnsolvedPuzzles.Add(puzzle);
        }
    }

    private void RandomizeRooms()
    {
        List<PuzzleRoom> UnusedRooms = new List<PuzzleRoom>(Rooms);
        for (int i = 0; i < RoomPuzzles; i++)
        {
            int index = Random.Range(0, UnusedRooms.Count);
            PuzzleRoom room = UnusedRooms[index];
            UnusedRooms.Remove(room);
            Puzzle puzzle = room.SpawnPuzzle();
            if (puzzle)
                UnsolvedPuzzles.Add(puzzle);
        }
        while (UnusedRooms.Count > 0)
        {
            PuzzleRoom room = UnusedRooms[0];
            UnusedRooms.Remove(room);
            room.SpawnRoom();
        }
    }

    public void SolvePuzzle(Puzzle puzzle)
    {
        UnsolvedPuzzles.Remove(puzzle);
    }

}
