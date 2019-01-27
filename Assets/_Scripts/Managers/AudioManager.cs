using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;

            s.source.loop = s.loop;

            s.source.Stop();

            if (s.playOnAwake)
            {
                s.source.Play();
            }

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public static void Play (string audioName)
    {
        Sound s = Array.Find(instance.sounds, sound => sound.name == audioName);

        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public static void Stop (string audioName)
    {
        Sound s = Array.Find(instance.sounds, sound => sound.name == audioName);

        s.source.Stop();
    }
}

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0, 1)]
    public float volume;
    [Range(0, 1)]
    public float pitch;

    public bool playOnAwake;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}