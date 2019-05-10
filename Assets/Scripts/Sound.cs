using UnityEngine.Audio;
using UnityEngine;
using System;

//Sound class will contain all the sound effects that will be used by AudioManger in all scenes, code by Brackeys
[Serializable]
public class Sound
{
    //Sound clip name
    public string name;
    public AudioClip clip;

    //Volume and pitch
    [Range(0f,1f)]
    public float volume;
    [Range(.1f, 1f)]
    public float pitch;

    //Loop indicator
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
