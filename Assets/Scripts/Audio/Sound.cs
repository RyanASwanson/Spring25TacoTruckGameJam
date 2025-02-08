using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound : MonoBehaviour
{
    public string name;

    public AudioClip clip; 

    public float volume;
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}
