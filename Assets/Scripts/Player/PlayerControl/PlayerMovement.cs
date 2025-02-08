using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    
    private Rigidbody _rigidBody;

    private Vector2 controlDir;
    private Vector3 _movementInpulse;

    private PlayerCameraInputActionMap _playerCameraInput;

    // Start is called before the first frame update
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _playerCameraInput = new PlayerCameraInputActionMap();
        _playerCameraInput.Camera.Enable();
        _playerCameraInput.Player.Enable();
        _playerCameraInput.Player.Movement.started += PlayerMovementInput;
        _playerCameraInput.Player.Movement.canceled += ctx => HaltMovement();
    }

    private void OnDestroy()
    {
        _playerCameraInput.Player.Movement.started -= PlayerMovementInput;
        _playerCameraInput.Player.Movement.canceled -= ctx => HaltMovement();
    }

    private void PlayerMovementInput(InputAction.CallbackContext context)
    {
        controlDir = context.ReadValue<Vector2>();
    }

    private void HaltMovement()
    {
        controlDir = Vector2.zero;
        _rigidBody.velocity = new Vector3(0, _rigidBody.velocity.y, 0);
    }

    private void Update()
    {
        print(controlDir);
        if (CameraSwitching.IsIn3D)
        {
            controlDir.y = 0;
        }

        Vector3 tempVel = new Vector3(controlDir.x, _rigidBody.velocity.y, controlDir.y);

        _rigidBody.velocity = tempVel * _moveSpeed;
    }
}
