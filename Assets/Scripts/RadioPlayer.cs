using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource.loop = false;
        audioSource.clip = GetRandomMusicClip();
        audioSource.Play();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = GetRandomMusicClip();
            audioSource.Play();
        }
    }

    private AudioClip GetRandomMusicClip()
    {
        return AudioController.Instance.MusicClips[Random.Range(0, AudioController.Instance.MusicClips.Count)];
    }
}