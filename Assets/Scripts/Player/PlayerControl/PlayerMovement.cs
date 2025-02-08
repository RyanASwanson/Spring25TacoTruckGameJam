using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;
using PrimeTween;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _rotationDuration = 0.2f;
    
    private Rigidbody _rigidBody;
    [SerializeField] private Transform _childHitbox;

    private Vector2 controlDir;
    private Vector2 lastMovementDir;

    private PlayerCameraInputActionMap _playerCameraInput;

    private bool _canJump = true;
    private bool _jumpQueued = false;

    private const string FLOOR_LAYER = "Floor";

    public static PlayerMovement Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

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
        Vector2 tempDir = context.ReadValue<Vector2>();
        if (tempDir != controlDir)
        {
            lastMovementDir = controlDir;
            controlDir = tempDir;
            Tween.Rotation(transform, Quaternion.LookRotation(new Vector3(controlDir.x, 0, controlDir.y)), _rotationDuration);
            Tween.Rotation(_childHitbox, Quaternion.LookRotation(Vector3.zero), _rotationDuration);
        }
    }

    private void HaltMovement()
    {
        controlDir = Vector2.zero;
        _rigidBody.velocity = new Vector3(0, _rigidBody.velocity.y, 0);
        Tween.StopAll(transform);
        Tween.StopAll(_childHitbox);
    }
    
    private void Jump()
    {
        if (_canJump)
        {
            _canJump = false;
            _jumpQueued = true;
            Tween.PunchScale(transform.GetChild(0), new Vector3(.1f, .5f, .1f), .3f);
            Tween.PunchLocalRotation(transform.GetChild(0), new Vector3(-30, 0, 0), .5f);
        }
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector2.down, 1.75f, LayerMask.GetMask(FLOOR_LAYER)))
        {
            _canJump = true;
        }
        else
        {
            _canJump = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
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
