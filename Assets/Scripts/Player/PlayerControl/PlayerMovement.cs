using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;
using PrimeTween;

public enum PlayerMovementState
{ 
    Walk,
    Climb,
    Ragdoll
};


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _rotationDuration = 0.2f;
    [SerializeField] private float _climbSpeed;
    
    private Rigidbody _rigidBody;
    [SerializeField] private Transform _childHitbox;

    private Vector2 controlDir;

    private GameObject _lastCollisionObject;

    private PlayerCameraInputActionMap _playerCameraInput;

    private bool _canJump = true;
    private bool _jumpQueued = false;

    private PlayerMovementState _movementState = PlayerMovementState.Walk;

    private const string FLOOR_LAYER = "Floor";
    private const string CLIMBABLE_WALL_LAYER = "ClimbableWall";

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
            controlDir = tempDir;
            Tween.Rotation(transform.GetChild(0), Quaternion.LookRotation(new Vector3(controlDir.x, 0, controlDir.y)), _rotationDuration);
            //Tween.Rotation(_childHitbox, Quaternion.LookRotation(Vector3.zero), _rotationDuration);
        }
    }

    private void HaltMovement()
    {
        controlDir = Vector2.zero;
        _rigidBody.velocity = new Vector3(0, _rigidBody.velocity.y, 0);
        Tween.StopAll(transform.GetChild(0));
        //Tween.StopAll(_childHitbox);
    }
    
    private void Jump()
    {
        if (_movementState != PlayerMovementState.Walk) return;

        if (_canJump)
        {
            print("jump");
            _canJump = false;
            _jumpQueued = true;
            Tween.PunchScale(transform.GetChild(0), new Vector3(.1f, .5f, .1f), .3f);
            Tween.PunchLocalRotation(transform.GetChild(0), new Vector3(-30, 0, 0), .5f);
        }
    }

    private void Update()
    {
        WallCheck();
        FloorCheck();
    }

    private void FloorCheck()
    {
        if (_movementState == PlayerMovementState.Ragdoll || WallRayCast()) return;

        if (Physics.Raycast(transform.position, Vector2.down, .75f, LayerMask.GetMask(FLOOR_LAYER)))
        {
            _canJump = true;
        }
        else
        {
            _canJump = false;
        }

        _movementState = PlayerMovementState.Walk;
    }

    private void WallCheck()
    {
        if (_movementState == PlayerMovementState.Ragdoll) return;

        if (WallRayCast())
        {
            _movementState = PlayerMovementState.Climb;
            _rigidBody.useGravity = false;
        }

        _rigidBody.useGravity = true;
    }

    private bool WallRayCast()
    {
        return Physics.Raycast(transform.position, transform.forward, .75f, LayerMask.GetMask(CLIMBABLE_WALL_LAYER));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(FLOOR_LAYER))
        {
            if (collision.GetContact(collision.contactCount-1).point.y > transform.position.y - .3f) return;

            if(!CameraSwitching.IsIn3D && collision.gameObject != _lastCollisionObject)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, collision.gameObject.transform.position.z);
            }
            _lastCollisionObject = collision.gameObject;
        }
    }

    private void FixedUpdate()
    {
        Vector3 tempVel;

        if (_movementState == PlayerMovementState.Climb)
        {
            tempVel = new Vector3(_rigidBody.velocity.x, _climbSpeed, _rigidBody.velocity.z);
        }
        else
        {
            if (!CameraSwitching.IsIn3D)
            {
                controlDir.y = 0;
            }

            tempVel = new Vector3(controlDir.x * _moveSpeed, _rigidBody.velocity.y, controlDir.y * _moveSpeed);

            if (_jumpQueued)
            {
                _jumpQueued = false;
                tempVel.y = _jumpForce;
            }
        }

        _rigidBody.velocity = tempVel;
    }
}
