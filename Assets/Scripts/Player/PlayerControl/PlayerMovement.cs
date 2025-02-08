using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;
using PrimeTween;
using Unity.VisualScripting;

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
    [SerializeField] private float _climbMantleSpeed;
    [SerializeField] private float _launchTime = 0.2f;
    
    private Rigidbody _rigidBody;
    [SerializeField] private Transform _childHitbox;

    private Vector2 controlDir;

    private GameObject _lastCollisionObject;

    private PlayerCameraInputActionMap _playerCameraInput;

    private bool _canJump = true;
    private bool _jumpQueued = false;
    private bool _atWallMax = false;

    private bool _wallMaxApplied = false;

    [SerializeField] private float _regrabWallWait;
    private bool _canClimb = true;

    private bool _shouldLaunch = false;
    private Vector3 _launchForce = Vector3.zero;
    private bool _isBeingLaunched = false;

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
            _canJump = false;
            _jumpQueued = true;
            Tween.PunchScale(transform.GetChild(0).GetChild(0), new Vector3(.1f, .5f, .1f), .3f);
            Tween.PunchLocalRotation(transform.GetChild(0).GetChild(0), new Vector3(-30, 0, 0), .5f);
        }
    }

    private void Update()
    {
        WallCheck();
        FloorCheck();
    }

    private void FloorCheck()
    {
        if (_movementState == PlayerMovementState.Ragdoll || WallRayCast(out RaycastHit hit)) return;

        if (Physics.Raycast(transform.position, Vector2.down, .75f, LayerMask.GetMask(FLOOR_LAYER)) ||
            Physics.Raycast(transform.position, Vector2.down, .75f, LayerMask.GetMask(CLIMBABLE_WALL_LAYER)))
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
        if (!_canClimb || _movementState == PlayerMovementState.Ragdoll) return;

        RaycastHit hit;

        if (WallRayCast(out hit))
        {
            _movementState = PlayerMovementState.Climb;
            _rigidBody.useGravity = false;


        }
        else if (_movementState == PlayerMovementState.Climb)
        {
            _movementState = PlayerMovementState.Walk;
            _rigidBody.useGravity = true;
            _rigidBody.AddForce(_climbMantleSpeed * Vector3.up);
        }
    }

    private bool WallRayCast(out RaycastHit hit)
    {
        bool hitWall =Physics.Raycast(
            new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z),
            transform.GetChild(0).forward, out hit, 0.75f, LayerMask.GetMask(CLIMBABLE_WALL_LAYER));
        return hitWall;
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
        Vector3 tempVel = _rigidBody.velocity;

        if (_shouldLaunch)
        {
            _shouldLaunch = false;
            _rigidBody.AddForce(_launchForce, ForceMode.Impulse);
            Invoke(nameof(ResetLaunch), _launchTime);
        }

        if(_atWallMax)
        {
            return;
        }

        if (_movementState == PlayerMovementState.Climb)
        {
            if (_rigidBody.velocity.y < _climbSpeed)
            {
                tempVel = new Vector3(_rigidBody.velocity.x, _climbSpeed, _rigidBody.velocity.z);
            }
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

        if (_isBeingLaunched) { return; }

        _rigidBody.velocity = tempVel;
    }

    public void ReceiveKnockback(float force, Vector3 direction)
    {
        _launchForce = direction * force;
        _shouldLaunch = true;
        _isBeingLaunched = true;
        _movementState = PlayerMovementState.Ragdoll;
    }

    private void ResetLaunch()
    {
        _isBeingLaunched = false;
        _movementState = PlayerMovementState.Walk;
    }
}
