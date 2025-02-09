using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicPlayer : MonoBehaviour
{
    [SerializeField] string _musicName;

    private void Start()
    {
        AudioManager.Instance.PlaySound(_musicName);
    }
}
