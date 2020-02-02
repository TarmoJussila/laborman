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

    public List<Puzzle> UnsolvedPuzzles = new List<Puzzle>();

    public float TimeLeft = 600.0f;
    public UnityEngine.UI.Text TimeField;
    public UnityEngine.UI.Text SalaryField;
    public UnityEngine.UI.Text PenaltyField;

    public string PlayerName;

    [SerializeField]
    private GameObject startView;
    [SerializeField]
    private GameObject gameView;
    [SerializeField]
    private GameObject scoreView;

    public readonly int BaseSalary = 10000;
    public readonly int PenaltyPerMinute = 100;
    public readonly int PenaltyPerUnusedProp = 50;
    public readonly int PenaltyPerUnfinishedJob = 2000;
    public readonly int PenaltyPerDeath = 5000;

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController PlayerController;

    public GameState CurrentGameState { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGameState(GameState.Start);
        //StartGame();
    }

    private void Update()
    {
        TimeLeft -= Time.deltaTime;
        int totalseconds = Mathf.FloorToInt(TimeLeft);
        int minutes = Mathf.FloorToInt(totalseconds / 60);
        int seconds = Mathf.FloorToInt(totalseconds % 60);

        TimeField.text = (minutes > 9 ? "" + minutes : "0" + minutes) + ":" + (seconds > 9 ? "" + seconds : "0" + seconds);

        if (Input.GetKeyDown(KeyCode.P))
        {
            EndGame(false);
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.O))
#else
        if (Input.GetKeyDown(KeyCode.Escape))
#endif
        {
            StartCoroutine(restart());
        }
    }

    IEnumerator restart()
    {
        PlayerController.enabled = false;
        yield return null;
        PlayerController.ToggleMouseLock(false);
        yield return null;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        RandomizeWalls();
        RandomizeRooms();
        SetGameState(GameState.Game);
    }

    public void RegisterWall(HouseWall wall)
    {
        Walls.Add(wall);
    }

    public void RegisterRoom(PuzzleRoom room)
    {
        Rooms.Add(room);
    }

    public void SetPlayerName(string name)
    {
        PlayerName = name;
        Random.seed = name.GetHashCode();
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
        if (UnsolvedPuzzles.Count == 0)
        {
            EndGame(true);
        }
    }

    public void Die()
    {
        EndGame(false);
    }

    public void EndGame(bool win)
    {
        int salary = BaseSalary;
        int unfinishedPenalty = UnsolvedPuzzles.Count * PenaltyPerUnfinishedJob;
        salary -= unfinishedPenalty;

        int timePenalty = 0;

        if (TimeLeft < 0)
        {
            float overtime = Mathf.Abs(TimeLeft);
            int minutes = Mathf.FloorToInt(overtime / 60);
            timePenalty = minutes * PenaltyPerMinute;
            salary -= timePenalty;
        }
        
        SalaryField.text = salary.ToString() + "€";
        PenaltyField.text = "Time penalty: " + timePenalty.ToString() + "€\n" + "Unfinished jobs penalty: " + unfinishedPenalty.ToString() + "€";

        SetGameState(GameState.Score);

        if (salary > 0)
        {
            AudioController.Instance.PlayPleasedSoundClip();
            AudioController.Instance.PlaySuccessSoundClip();
        }
        else
        {
            AudioController.Instance.PlayAngrySoundClip();
            AudioController.Instance.PlayFailureSoundClip();
            SalaryField.color = Color.red;
        }
    }

    public void SetGameState(GameState state)
    {
        CurrentGameState = state;
        switch (state)
        {
            case GameState.Start:
                {
                    PlayerController.enabled = false;
                    startView.SetActive(true);
                    gameView.SetActive(false);
                    scoreView.SetActive(false);
                    break;
                }
            case GameState.Game:
                {
                    PlayerController.enabled = true;
                    startView.SetActive(false);
                    gameView.SetActive(true);
                    scoreView.SetActive(false);
                    break;
                }
            case GameState.Score:
                {
                    PlayerController.enabled = false;
                    startView.SetActive(false);
                    gameView.SetActive(false);
                    scoreView.SetActive(true);
                    break;
                }
        }

    }
}
public enum GameState
{
    Start = 0,
    Game,
    Score
}
