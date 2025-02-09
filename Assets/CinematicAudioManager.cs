using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class CinematicAudioManager : MonoBehaviour
{
    public static CinematicAudioManager Instance;

    public Sound CinematicTrack;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
        DontDestroyOnLoad(this);

        CinematicTrack.source = gameObject.AddComponent<AudioSource>();
        CinematicTrack.source.clip = CinematicTrack.clip;
        CinematicTrack.source.volume = CinematicTrack.volume;
        CinematicTrack.source.pitch = CinematicTrack.pitch;
        CinematicTrack.source.loop = CinematicTrack.shouldLoop;
    }

    private void Start()
    {
        CinematicTrack.source.Play();
    }
}
