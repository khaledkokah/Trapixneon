using UnityEngine.Audio;
using UnityEngine;
using System;

//== AudioManager class based on Brackeys https://github.com/Brackeys
public class AudioManager : MonoBehaviour
{
    //Static instance of AudioManager which allows it to be accessed by  other scripts
    public static AudioManager instance=null;

    public Sound[] sounds;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        else if (instance != this)
            //then destroy this, this enforces singleton pattern, meaning there can only ever be one instance of an AudioManager
            Destroy(gameObject);

        //Loop through all defined sounds
        foreach (Sound s in sounds)
        {
            //Assign the AudioSource and clip
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            //Assign the sound properties 
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //Play the sound by name
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " was not found.");
            return;
        }
        s.source.Play();
    }

    //Stop the sound 
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " was not found.");
            return;
        }
        s.source.Stop();
    }

}
