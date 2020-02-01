using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public LayerMask GrabbableLayer;

    private bool holding = false;
    private readonly float maxDistance = 2.0f;
    private RaycastHit hit;
    private Rigidbody holdingRb;
    private float DragForce = 10.0f;

    // Update is called once per frame
    void Update()
    {
        if (holding)
        {
            Vector3 targetPos = Camera.main.transform.position + Camera.main.transform.forward * maxDistance;
            holdingRb.AddForce(targetPos - holdingRb.transform.position * Time.deltaTime * DragForce);
            if (Input.GetKeyDown(KeyCode.E))
            {
                holding = false;
                holdingRb = null;
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("try grab");
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, GrabbableLayer))
            {
                Debug.Log("raycast hit");
                holdingRb = hit.collider.GetComponent<Rigidbody>();
                holdingRb.isKinematic = false;
                holding = true;
            }
        }
    }
}
