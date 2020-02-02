using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    public List<AudioClip> MusicClips { get { return musicClips; } }
    public List<AudioClip> RadioStaticClips { get { return radioStaticClips; } }

    [SerializeField] private AudioSource playerAudioSource;

    // Radio audio source.
    [SerializeField] private List<AudioClip> musicClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> radioStaticClips = new List<AudioClip>();

    // Player audio source.
    [SerializeField] private List<AudioClip> angrySounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> pleasedSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> thinkingSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> windUpSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> throwSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> valveSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> electricSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> beerSounds = new List<AudioClip>();
    [SerializeField] private AudioClip failureSound;
    [SerializeField] private AudioClip successSound;

    private void Update()
    {
        if (!playerAudioSource.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayAngrySoundClip();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayPleasedSoundClip();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayThinkingSoundClip();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PlayWindUpSoundClip();
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public void PlayAngrySoundClip()
    {
        playerAudioSource.PlayOneShot(angrySounds[Random.Range(0, angrySounds.Count)]);
    }

    public void PlayPleasedSoundClip()
    {
        playerAudioSource.PlayOneShot(pleasedSounds[Random.Range(0, pleasedSounds.Count)]);
    }

    public void PlayThinkingSoundClip()
    {
        playerAudioSource.PlayOneShot(thinkingSounds[Random.Range(0, thinkingSounds.Count)]);
    }

    public void PlayWindUpSoundClip()
    {
        playerAudioSource.PlayOneShot(windUpSounds[Random.Range(0, windUpSounds.Count)]);
    }

    public void PlayThrowSoundClip()
    {
        playerAudioSource.PlayOneShot(throwSounds[Random.Range(0, throwSounds.Count)]);
    }

    public void PlayValveSoundClip()
    {
        playerAudioSource.PlayOneShot(valveSounds[Random.Range(0, valveSounds.Count)]);
    }

    public void PlayElectricSoundClip()
    {
        playerAudioSource.PlayOneShot(electricSounds[Random.Range(0, electricSounds.Count)]);
    }

    public void PlayBeerSoundClip()
    {
        playerAudioSource.PlayOneShot(beerSounds[Random.Range(0, beerSounds.Count)]);
    }

    public void PlayFailureSoundClip()
    {
        playerAudioSource.PlayOneShot(failureSound);
    }

    public void PlaySuccessSoundClip()
    {
        playerAudioSource.PlayOneShot(successSound);
    }
}