using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobFinder : MonoBehaviour
{
    public Text AmountText;
    public Text DistanceText;

    private float updateTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (RenovationController.Instance.CurrentGameState != GameState.Game) return;

        updateTimer += Time.deltaTime;
        if (updateTimer < 0.5f)
            return;
        else
            updateTimer = 0.0f;

        AmountText.text = RenovationController.Instance.UnsolvedPuzzles.Count.ToString();

        RenovationController.Instance.UnsolvedPuzzles.Sort((a, b) =>
        {
            float distA = Vector3.Distance(transform.position, a.transform.position);
            float distB = Vector3.Distance(transform.position, b.transform.position);
            return Mathf.FloorToInt(distA - distB);
        });

        DistanceText.text = Mathf.FloorToInt(Vector3.Distance(transform.position, RenovationController.Instance.UnsolvedPuzzles[0].transform.position)).ToString() + "m";
    }
}
