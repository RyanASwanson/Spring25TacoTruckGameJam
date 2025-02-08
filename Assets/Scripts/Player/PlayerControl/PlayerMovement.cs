using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidBody;

    private Vector2 controlDir;
    private Vector3 _movementInpulse;

    private PlayerCameraInputActionMap _playerCameraInput;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _playerCameraInput = new PlayerCameraInputActionMap();
        _playerCameraInput.Camera.Enable();
        _playerCameraInput.Camera.SwitchView.started += PlayerMovementInput;
    }

    private void OnDestroy()
    {
        _playerCameraInput.Camera.SwitchView.started -= PlayerMovementInput;
    }

    private void PlayerMovementInput(InputAction.CallbackContext context)
    {
        controlDir = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (CameraSwitching.IsIn3D)
        {
            controlDir.y = 0;
        }

        Vector3 tempVel = new Vector3(controlDir.x, _rigidBody.velocity.y, controlDir.y);

        _rigidBody.velocity = tempVel;
    }
}
