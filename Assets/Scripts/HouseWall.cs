using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseWall : MonoBehaviour
{
    [SerializeField]
    private List<WallPanel> InnerPanels = new List<WallPanel>();
    [SerializeField]
    private List<WallPanel> OuterPanels = new List<WallPanel>();
    [SerializeField]
    private GameObject WindowObject;

    public bool CanHavePuzzles = true;
    public bool CanBeWindow = false;

    // Start is called before the first frame update
    void Awake()
    {
        SetWindow(CanBeWindow);
        RenovationController.Instance.RegisterWall(this);
    }

    public void SetWindow(bool isWindow)
    {
        WindowObject.SetActive(isWindow && CanBeWindow);
        InnerPanels[1].gameObject.SetActive(!isWindow);
        OuterPanels[1].gameObject.SetActive(!isWindow);
    }

    public Puzzle SpawnPuzzle()
    {
        GameObject prefab = RenovationController.Instance.PuzzlePrefabs[Random.Range(0, RenovationController.Instance.PuzzlePrefabs.Length)];
        GameObject instance = Instantiate(prefab);

        WallPanel target;
        if (CanBeWindow)
        {
            float roll = Random.Range(0.0f, 1.0f);
            target = roll < 0.5f ? InnerPanels[0] : InnerPanels[2];
        }
        else
        {
            target = InnerPanels[Random.Range(0, InnerPanels.Count)];
        }

        instance.transform.position = target.transform.position;
        instance.transform.forward = target.transform.forward;
        Puzzle puzzle = instance.GetComponent<Puzzle>();
        if (puzzle.LockWall)
        {
            target.gameObject.transform.GetChild(0).gameObject.layer = 0;
        }
        return puzzle;
    }
}
