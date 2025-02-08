using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound")]
public class Sound : ScriptableObject
{
    public string soundName;

    public AudioClip clip;

    [Range(0.1f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public bool shouldLoop;

    [HideInInspector]
    public AudioSource source;
}
