﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzleBlock : MonoBehaviour
{
    public GameObject PipeObject;

    public BlockType Type;
    public Flow FlowDir;
    public PipeType Pipe;
    public Side[] Sides;

    private Outline outline;
    [SerializeField]
    private MeshRenderer blockRenderer;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        blockRenderer = GetComponent<MeshRenderer>();
    }

    public void SetType(BlockType type)
    {
        Type = type;
        outline.enabled = false;
        blockRenderer.enabled = false;
        switch (type)
        {
            case BlockType.Start:
                {
                    FlowDir = Flow.DownUp;
                    Pipe = PipeType.Straight;
                    Sides = new Side[] { Side.Up, Side.Down };
                    break;
                }
            case BlockType.End:
                {
                    FlowDir = Flow.LeftRight;
                    Pipe = PipeType.Straight;
                    Sides = new Side[] { Side.Left, Side.Right };
                    break;
                }
            case BlockType.Obstacle:
                {
                    blockRenderer.enabled = true;
                    Color c = blockRenderer.material.color;
                    c.a = 1f;
                    blockRenderer.material.color = c;
                    break;
                }
            case BlockType.AlwaysEmpty:
            case BlockType.Empty:
                {
                    blockRenderer.enabled = true;
                    Color c = blockRenderer.material.color;
                    c.a = 0.01f;
                    blockRenderer.material.color = c;
                    outline.enabled = true;
                    break;
                }
            default:
                {
                    GetComponentInChildren<MeshRenderer>(true).enabled = false;
                    break;
                }
        }
    }

    public void Rotate()
    {
        if (Pipe == PipeType.Corner)
        {
            //PipeObject.transform.RotateAround
            switch (FlowDir)
            {
                case Flow.LeftDown:
                    {
                        FlowDir = Flow.LeftUp;
                        break;
                    }
                case Flow.LeftUp:
                    {
                        FlowDir = Flow.UpRight;
                        break;
                    }
                case Flow.UpRight:
                    {
                        FlowDir = Flow.RightDown;
                        break;
                    }
                case Flow.RightDown:
                    {
                        FlowDir = Flow.LeftDown;
                        break;
                    }
            }
        }
        else if (Pipe == PipeType.Corner)
        {

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Type != BlockType.Empty && Type != BlockType.AlwaysEmpty) return;

        bool straight = other.CompareTag("StraightPipe");
        bool corner = other.CompareTag("CornerPipe");
        if (straight || corner)
        {
            PuzzlePipe pipe = other.GetComponent<PuzzlePipe>();
            if (!pipe.Placed && !pipe.Grabbed)
            {
                PipeObject = other.gameObject;
                PipeObject.transform.position = transform.position;
                PipeObject.transform.forward = transform.forward;
                Grabber.Instance.Release();
                pipe.PlacePipe(this);

                if (straight)
                {
                    FlowDir = Flow.LeftRight;
                    Sides = new Side[] { Side.Left, Side.Right };
                }
                else if (corner)
                {
                    FlowDir = Flow.UpRight;
                    Sides = new Side[] { Side.Up, Side.Right };
                }

            }
        }
    }
}

[System.Serializable]
public enum BlockType
{
    Start = 0,
    End,
    Empty,
    AlwaysEmpty,
    Obstacle,
    Occupied
}

[System.Serializable]
public enum Flow
{
    UpRight = 0,
    RightDown,
    LeftRight,
    LeftDown,
    LeftUp,
    DownUp
}

[System.Serializable]
public enum PipeType
{
    Straight = 0,
    Corner
}

[System.Serializable]
public enum Side
{
    Up = 0,
    Right,
    Down,
    Left
}