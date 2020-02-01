using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    public List<AudioClip> AudioClips { get { return audioClips; } }

    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    private void Awake()
    {
        Instance = this;
    }
}