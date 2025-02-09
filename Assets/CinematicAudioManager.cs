using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class CinematicAudioManager : MonoBehaviour
{
    public static CinematicAudioManager Instance;

    public Sound[] CinematicTrack;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
        DontDestroyOnLoad(this);

        foreach (var sound in CinematicTrack)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.shouldLoop;
        }
    }

    private void Start()
    {
        foreach (var sound in CinematicTrack)
        {
            sound.source.Play();
        }
    }
}
