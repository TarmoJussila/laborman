using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseWall : MonoBehaviour
{
    [SerializeField]
    private List<WallPanel> Panels = new List<WallPanel>();
    [SerializeField]
    private GameObject WindowObject;

    public bool CanHavePuzzles = true;
    public bool CanBeWindow = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWindow(bool isWindow)
    {
        WindowObject.SetActive(isWindow && CanBeWindow);
    }
}
