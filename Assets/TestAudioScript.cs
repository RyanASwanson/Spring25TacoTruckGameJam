using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudioScript : MonoBehaviour
{
    PlayerCameraInputActionMap _controls;

    private void Awake()
    {
        _controls = new PlayerCameraInputActionMap();
        _controls.Testing.Enable();
        _controls.Testing.PlayLoop.performed += ctx => AudioManager.Instance.PlaySound("TestLoop");
        _controls.Testing.PlayOneShot.performed += ctx => AudioManager.Instance.PlayOneShotSound("TestOneShot");
        _controls.Testing.PlaySound.performed += ctx => AudioManager.Instance.PlaySound("TestTempPlay");
        _controls.Testing.StopSound.performed += ctx => AudioManager.Instance.StopSound("TestTempPlay");
    }

    private void OnDisable()
    {
        _controls.Testing.PlayLoop.performed -= ctx => AudioManager.Instance.PlaySound("TestLoop");
        _controls.Testing.PlayOneShot.performed -= ctx => AudioManager.Instance.PlayOneShotSound("TestOneShot");
        _controls.Testing.PlaySound.performed -= ctx => AudioManager.Instance.PlaySound("TestTempPlay");
        _controls.Testing.StopSound.performed -= ctx => AudioManager.Instance.StopSound("TestTempPlay");
    }
}
