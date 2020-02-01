using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzleBlock : MonoBehaviour
{
    public GameObject PipeObject = null;

    public PipePuzzle RelatedPuzzle;

    public BlockType Type;
    public Flow FlowDir;
    public PipeType Pipe;
    public List<Side> Sides;

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
                    Sides = new List<Side>(new Side[] { Side.Up, Side.Down });
                    break;
                }
            case BlockType.End:
                {
                    FlowDir = Flow.LeftRight;
                    Pipe = PipeType.Straight;
                    Sides = new List<Side>(new Side[] { Side.Left, Side.Right });
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
            PipeObject.transform.Rotate(Vector3.forward, -90.0f);
            switch (FlowDir)
            {
                case Flow.LeftDown:
                    {
                        FlowDir = Flow.LeftUp;
                        Sides = new List<Side>( new Side[] { Side.Left, Side.Up });
                        break;
                    }
                case Flow.LeftUp:
                    {
                        FlowDir = Flow.UpRight;
                        Sides = new List<Side>( new Side[] { Side.Up, Side.Right });
                        break;
                    }
                case Flow.UpRight:
                    {
                        FlowDir = Flow.RightDown;
                        Sides = new List<Side>( new Side[] { Side.Down, Side.Right });
                        break;
                    }
                case Flow.RightDown:
                    {
                        FlowDir = Flow.LeftDown;
                        Sides = new List<Side>( new Side[] { Side.Left, Side.Down });
                        break;
                    }
            }
        }
        else if (Pipe == PipeType.Straight)
        {
            FlowDir = FlowDir == Flow.LeftRight ? Flow.DownUp : Flow.LeftRight;
            PipeObject.transform.Rotate(Vector3.forward, -90.0f);
            if (FlowDir == Flow.LeftRight)
            {
                Sides = new List<Side>( new Side[] { Side.Left, Side.Right });
            }
            else
            {
                Sides = new List<Side>( new Side[] { Side.Down, Side.Up });
            }
        }

        RelatedPuzzle.CheckCompletion();
    }

    private void OnTriggerStay(Collider other)
    {
        if ((Type != BlockType.Empty && Type != BlockType.AlwaysEmpty) || PipeObject != null) return;

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
                    Pipe = PipeType.Straight;
                    FlowDir = Flow.LeftRight;
                    Sides = new List<Side>( new Side[] { Side.Left, Side.Right });
                }
                else if (corner)
                {
                    Pipe = PipeType.Corner;
                    FlowDir = Flow.UpRight;
                    Sides = new List<Side>( new Side[] { Side.Up, Side.Right });
                }
                RelatedPuzzle.CheckCompletion();

            }
        }
    }

    public void DetachPipe()
    {
        PipeObject = null;
    }

    public static Side GetOpposite(Side side)
    {
        switch (side)
        {
            case Side.Down:
                {
                    return Side.Up;
                }
            case Side.Up:
                {
                    return Side.Down;
                }
            case Side.Left:
                {
                    return Side.Right;
                }
            case Side.Right:
                {
                    return Side.Left;
                }
            default:
                {
                    return Side.Down;
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