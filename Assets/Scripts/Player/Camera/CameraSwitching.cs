using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitching : MonoBehaviour
{
    [SerializeField] private GameObject _2DCamera;
    [SerializeField] private GameObject _3DCamera;

    internal bool IsIn3D = false;

    private PlayerCameraInputActionMap _playerCameraInput;

    // Start is called before the first frame update
    void Start()
    {
        _playerCameraInput = new PlayerCameraInputActionMap();
        _playerCameraInput.Camera.Enable();
        _playerCameraInput.Camera.SwitchView.started += CameraShift;
    }

    private void OnDestroy()
    {
        _playerCameraInput.Camera.SwitchView.started -= CameraShift;
    }

    private void CameraShift(InputAction.CallbackContext context)
    {
        IsIn3D = !IsIn3D;
        _2DCamera.SetActive(!IsIn3D);
        _3DCamera.SetActive(IsIn3D);
    }
}
