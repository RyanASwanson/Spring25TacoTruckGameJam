using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _jumpForce = 5f;
    
    private Rigidbody _rigidBody;

    private Vector2 controlDir;
    private Vector3 _movementInpulse;

    private PlayerCameraInputActionMap _playerCameraInput;

    private bool _canJump = true;
    private bool _jumpQueued = false;

    // Start is called before the first frame update
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _playerCameraInput = new PlayerCameraInputActionMap();
        _playerCameraInput.Camera.Enable();
        _playerCameraInput.Player.Enable();
        _playerCameraInput.Player.Movement.performed += PlayerMovementInput;
        _playerCameraInput.Player.Movement.canceled += ctx => HaltMovement();
        _playerCameraInput.Player.Jump.performed += ctx => Jump();
    }

    private void OnDestroy()
    {
        _playerCameraInput.Player.Movement.performed -= PlayerMovementInput;
        _playerCameraInput.Player.Movement.canceled -= ctx => HaltMovement();
        _playerCameraInput.Player.Jump.performed -= ctx => Jump();
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
    
    private void Jump()
    {
        if (_canJump)
        {
            _canJump = false;
            _jumpQueued = true;
        }
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector2.down, 1.75f, LayerMask.GetMask("Floor")))
        {
            _canJump = true;
        }
        else
        {
            _canJump = false;
        }
    }

    private void FixedUpdate()
    {
        if (!CameraSwitching.IsIn3D)
        {
            controlDir.y = 0;
        }

        Vector3 tempVel = new Vector3(controlDir.x * _moveSpeed, _rigidBody.velocity.y, controlDir.y * _moveSpeed);

        if (_jumpQueued)
        {
            _jumpQueued = false;
            tempVel.y =  _jumpForce;
        }


        _rigidBody.velocity = tempVel;
    }
}
