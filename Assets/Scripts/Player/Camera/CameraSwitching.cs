using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CameraSwitching : MonoBehaviour
{
    [SerializeField] private bool _isOnMainMenu = false;
    [SerializeField] private GameObject _2DCamera;
    [SerializeField] private GameObject _3DCamera;

    internal static bool IsIn3D = false;

    public static UnityEvent _on2DSwitchEvent = new();
    public static UnityEvent _on3DSwitchEvent = new();

    private PlayerCameraInputActionMap _playerCameraInput;

    // Start is called before the first frame update
    void Awake()
    {
        if (!_isOnMainMenu)
        {
            _playerCameraInput = new PlayerCameraInputActionMap();
            _playerCameraInput.Camera.Enable();
            _playerCameraInput.Camera.SwitchView.started += CameraShift;
        }
        else
        {
            IsIn3D = true;
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            IsIn3D = true;
        }
        else
        {
            IsIn3D = false;
        }
    }

    private void OnDestroy()
    {
        if (_playerCameraInput != null)
        {
            _playerCameraInput.Disable();
            _playerCameraInput.Camera.SwitchView.started -= CameraShift;
        }

        _on2DSwitchEvent.RemoveAllListeners();
        _on3DSwitchEvent.RemoveAllListeners();
    }

    private void CameraShift(InputAction.CallbackContext context)
    {
        IsIn3D = !IsIn3D;
        _2DCamera.SetActive(!IsIn3D);
        _3DCamera.SetActive(IsIn3D);

        if(!IsIn3D)
        {
            _on2DSwitchEvent?.Invoke();
            AudioManager.Instance.PlayOneShotSound("Swap2D");
        }
        else
        {
            _on3DSwitchEvent?.Invoke();
            AudioManager.Instance.PlayOneShotSound("Swap3D");
        }
    }

    
}
