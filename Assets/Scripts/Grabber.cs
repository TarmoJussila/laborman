﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public static Grabber Instance { get; private set; }

    public LayerMask GrabbableLayer;
    public LayerMask InteractableLayer;
    public float DragForce = 100.0f;

    private bool holding = false;
    private float holdDistance = 2.5f;
    private RaycastHit hit;
    private Rigidbody holdingRb;

    [SerializeField]
    private AnimationCurve dragCurve;

    private readonly float maxDistance = 3.0f;
    private readonly float holdDistanceMin = 1.4f;
    private readonly float holdDistanceMax = 2.8f;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (holding)
        {
            Vector3 targetPos = Camera.main.transform.position + Camera.main.transform.forward * holdDistance;
            float distance = Vector3.Distance(holdingRb.transform.position, targetPos);
            Vector3 dir = targetPos - holdingRb.position;
            holdingRb.velocity = dir * dragCurve.Evaluate(distance);
            holdingRb.transform.forward = Camera.main.transform.forward;

            var mouseScroll = Input.mouseScrollDelta.y;

            holdDistance -= mouseScroll * Time.deltaTime * 2f;
            holdDistance = Mathf.Clamp(holdDistance, holdDistanceMin, holdDistanceMax);

            //holdingRb.transform.position = Vector3.Lerp(holdingRb.transform.position, targetPos, 0.5f);
            /*if (distance > 2)
            {
                holdingRb.transform.position = Vector3.Lerp(holdingRb.transform.position, targetPos, 0.9f);
            }
            else
            {
                holdingRb.velocity = (targetPos - holdingRb.transform.position) * DragForce;
                //holdingRb.AddForce(targetPos - holdingRb.transform.position * Time.deltaTime * DragForce);
            }*/

            if (Input.GetKeyDown(KeyCode.E))
            {
                Release();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ThrowRelease();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, GrabbableLayer))
            {
                holdingRb = hit.collider.GetComponent<Rigidbody>();
                holdingRb.isKinematic = false;
                holdingRb.transform.SetParent(null);
                holdingRb.useGravity = false;
                holding = true;
                if (holdingRb.tag.IndexOf("Pipe") > -1)
                {
                    PuzzlePipe pipe = holdingRb.GetComponent<PuzzlePipe>();
                    pipe.DetachPipe();
                    pipe.Grabbed = true;
                }
            }
            else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, InteractableLayer))
            {
                if (hit.collider.CompareTag("Electric"))
                {
                    hit.collider.GetComponentInParent<ElectricPuzzle>().ToggleFuse(hit.transform.name);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RaycastHit rayhit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out rayhit, maxDistance, GrabbableLayer))
            {
                if (rayhit.collider.tag.IndexOf("Pipe") > -1)
                {
                    rayhit.collider.GetComponent<PuzzlePipe>().AttachedBlock.Rotate();
                }
            }
        }
    }

    public void Release()
    {
        if (!holdingRb) return;
        holding = false;
        holdingRb.useGravity = true;
        if (holdingRb.tag.IndexOf("Pipe") > -1)
        {
            PuzzlePipe pipe = holdingRb.GetComponent<PuzzlePipe>();
            pipe.Grabbed = false;
        }
        holdingRb = null;
    }

    public void ThrowRelease()
    {
        if (!holdingRb) return;
        holding = false;
        holdingRb.useGravity = true;
        if (holdingRb.tag.IndexOf("Pipe") > -1)
        {
            PuzzlePipe pipe = holdingRb.GetComponent<PuzzlePipe>();
            pipe.Grabbed = false;
        }
        holdingRb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        holdingRb = null;
    }
}
