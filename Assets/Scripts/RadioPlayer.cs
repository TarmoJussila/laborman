using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource.clip = GetRandomAudioClip();
        audioSource.Play();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = GetRandomAudioClip();
            audioSource.Play();
        }
    }

    private AudioClip GetRandomAudioClip()
    {
        return AudioController.Instance.AudioClips[Random.Range(0, AudioController.Instance.AudioClips.Count)];
    }
}