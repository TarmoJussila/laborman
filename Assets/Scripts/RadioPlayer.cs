using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private bool didPlayRadioStaticSound = false;
    private int lastMusicClipIndex = -1;

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
            if (!didPlayRadioStaticSound)
            {
                audioSource.clip = GetRandomRadioStaticClip();
                audioSource.Play();
                didPlayRadioStaticSound = true;
            }
            else
            {
                audioSource.clip = GetRandomMusicClip();
                audioSource.Play();
                didPlayRadioStaticSound = false;
            }
        }
    }

    private AudioClip GetRandomMusicClip()
    {
        int newMusicClipIndex = Random.Range(0, AudioController.Instance.MusicClips.Count);

        while (newMusicClipIndex == lastMusicClipIndex && AudioController.Instance.MusicClips.Count > 1)
        {
            newMusicClipIndex = Random.Range(0, AudioController.Instance.MusicClips.Count);
        }

        lastMusicClipIndex = newMusicClipIndex;
        return AudioController.Instance.MusicClips[newMusicClipIndex];
    }

    private AudioClip GetRandomRadioStaticClip()
    {
        return AudioController.Instance.RadioStaticClips[Random.Range(0, AudioController.Instance.RadioStaticClips.Count)];
    }

    public void ForceChangeMusicClip()
    {
        if (!didPlayRadioStaticSound)
        {
            audioSource.clip = GetRandomRadioStaticClip();
            audioSource.Play();
            didPlayRadioStaticSound = true;
        }
    }
}