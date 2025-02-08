using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] _sounds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;

        foreach (var sound in _sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.shouldLoop;
        }
    }

    public void PlayOneShotSound(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound.soundName == name);

        if (s == null) { return; }

        s.source.PlayOneShot(s.clip);
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound.soundName == name);

        if (s == null) { return; }

        s.source.Play();
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound.soundName == name);

        if (s == null) { return; }

        s.source.Stop();
    }
}
