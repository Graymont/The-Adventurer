using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="Scriptable Object/Sound Effect")]
public class SoundEffect : ScriptableObject
{
    public new string name;
    public AudioClip audio;
    public float pitch = 1;
    public float volume = 1;
    public bool playOnAwake;
    public bool loop;
}
