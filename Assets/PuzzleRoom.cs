using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRoom : MonoBehaviour
{
    private void Awake()
    {
        RenovationController.Instance.RegisterRoom(this);
    }

    public Puzzle SpawnPuzzle()
    {
        GameObject prefab = RenovationController.Instance.RoomPuzzlePrefabs[Random.Range(0, RenovationController.Instance.RoomPuzzlePrefabs.Length)];
        GameObject instance = Instantiate(prefab);        

        instance.transform.position = transform.position;
        instance.transform.forward = transform.forward;
        Puzzle puzzle = instance.GetComponent<Puzzle>();        
        return puzzle;
    }

    public void SpawnRoom()
    {
        GameObject prefab = RenovationController.Instance.RoomPrefabs[Random.Range(0, RenovationController.Instance.RoomPrefabs.Length)];
        GameObject instance = Instantiate(prefab);

        instance.transform.position = transform.position;
        instance.transform.forward = transform.forward;
    }

}
