using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // Needs one-shots, looping, and semi-persistent sounds
    // Pitch variation
    [SerializeField] private Sound[] _sounds;

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
        }
    }

    public void PlayOneShotSound(string soundName)
    {
        Sound s = Array.Find(_sounds, sound => sound.name == soundName);

        if (s != null) { return; }

        s.source.PlayOneShot(s.clip);
    }

    public void PlaySound(string soundName)
    {
        Sound s = Array.Find(_sounds, sound => sound.name == soundName);

        if (s != null) { return; }

        s.source.Play();
    }

    public void StopSound(string soundName)
    {
        Sound s = Array.Find(_sounds, sound => sound.name == soundName);

        if (s != null) { return; }

        s.source.Stop();
    }
}
