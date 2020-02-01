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
        //RenovationController.Instance.RegisterWall(this);
    }

    public void SetWindow(bool isWindow)
    {
        WindowObject.SetActive(isWindow && CanBeWindow);
        InnerPanels[1].gameObject.SetActive(!isWindow);
        OuterPanels[1].gameObject.SetActive(!isWindow);
    }

    public Puzzle SpawnPuzzle()
    {
        return new Puzzle();
    }
}
