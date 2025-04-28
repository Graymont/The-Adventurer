using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public List<SoundEffect> soundEffects = new List<SoundEffect>();

    [SerializeField] AudioSource audioSource;
    public GameObject soundBox;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string name, float volume, float pitch)
    {
        audioSource.volume = 1;
        audioSource.pitch = 1;

        foreach (SoundEffect sound in soundEffects)
        {
            if (name == sound.name)
            {
                audioSource.volume = volume + (sound.volume-1);
                audioSource.pitch = pitch + (sound.pitch - 1);
                audioSource.PlayOneShot(sound.audio); 
                break;
            }
        }
    }

    public AudioClip GetAudioClip(string name)
    {
        AudioClip audioClip = soundEffects[0].audio;

        foreach (SoundEffect sound in soundEffects)
        {
            if (name == sound.name)
            {
                audioClip = sound.audio;
                break;
            }
        }

        return audioClip;
    }
}
