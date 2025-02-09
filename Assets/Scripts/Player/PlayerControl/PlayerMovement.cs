using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using TMPro.EditorUtilities;
#endif

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
    [SerializeField] private float _moveAnimSpeed = 0.3f;
    
    private Rigidbody _rigidBody;
    [SerializeField] private Transform _childHitbox;

    private Vector2 controlDir;

    private GameObject _lastCollisionObject;

    private PlayerCameraInputActionMap _playerCameraInput;

    private bool _canJump = true;
    private bool _jumpQueued = false;
    private bool _atWallMax = false;

    private bool _inAir = false;

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

    private bool _stretched = false;
    private bool _isAnimatingMovement = false;

    public Vector3 RespawnPoint = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);

        Instance = this;

        _rigidBody = GetComponent<Rigidbody>();

        _playerCameraInput = new PlayerCameraInputActionMap();
        _playerCameraInput.Camera.Enable();
        _playerCameraInput.Player.Enable();
        _playerCameraInput.Player.Movement.performed += PlayerMovementInput;
        //_playerCameraInput.Player.Movement.performed += ctx => StartCoroutine(nameof(MovementAnimation));
        _playerCameraInput.Player.Movement.canceled += ctx => HaltMovement();
        _playerCameraInput.Player.Movement.canceled += ctx => StopAllCoroutines();
        _playerCameraInput.Player.Movement.canceled += ctx => ResetStretch();
        _playerCameraInput.Player.Jump.performed += ctx => Jump();

        RespawnPoint = transform.position;
    }

    private void OnDestroy()
    {
        _playerCameraInput.Disable();
        _playerCameraInput.Player.Movement.performed -= PlayerMovementInput;
        //_playerCameraInput.Player.Movement.performed -= ctx => StartCoroutine(nameof(MovementAnimation));
        _playerCameraInput.Player.Movement.canceled -= ctx => HaltMovement();
        _playerCameraInput.Player.Movement.canceled -= ctx => StopAllCoroutines();
        _playerCameraInput.Player.Movement.canceled -= ctx => ResetStretch();
        _playerCameraInput.Player.Jump.performed -= ctx => Jump();
    }

    private void PlayerMovementInput(InputAction.CallbackContext context)
    {
        StartCoroutine(nameof(MovementAnimation));

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
        if (_rigidBody != null)
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
            if (!_canJump)
            {
                Landed();
            }
            _canJump = true;
        }
        else
        {
            _canJump = false;
        }

        _movementState = PlayerMovementState.Walk;
    }

    private void Landed()
    {
        Tween.PunchScale(transform.GetChild(0).GetChild(0), new Vector3(0, -.8f, 0), .15f);
        AudioManager.Instance.PlayOneShotSound("Land");
        //Tween.PunchLocalRotation(transform.GetChild(0).GetChild(0), new Vector3(0, 0, 30), .5f);
    }

    private void WallCheck()
    {
        if (!_canClimb || _movementState == PlayerMovementState.Ragdoll) return;

        RaycastHit hit;

        if (WallRayCast(out hit))
        {
            if(_movementState != PlayerMovementState.Climb)
            {
                GameObject go = transform.GetChild(0).GetChild(0).GetChild(0).transform.gameObject;
                Tween.LocalEulerAngles(go.transform, Vector3.zero, new Vector3(-90, 0, 0), .4f);
            }

            _movementState = PlayerMovementState.Climb;
            _rigidBody.useGravity = false;

            //transform.GetChild(0).GetChild(0).GetChild(0).transform.localEulerAngles = hit.normal;
            //transform.GetChild(0).GetChild(0).GetChild(0).transform.localEulerAngles = new Vector3(-90,0,0);
            
        }
        else if (_movementState == PlayerMovementState.Climb)
        {
            _movementState = PlayerMovementState.Walk;
            _rigidBody.useGravity = true;
            _rigidBody.AddForce(_climbMantleSpeed * Vector3.up);

            GameObject go = transform.GetChild(0).GetChild(0).GetChild(0).transform.gameObject;
            Tween.LocalEulerAngles(go.transform, new Vector3(-90, 0, 0), Vector3.zero,.4f);
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
            if (collision.GetContact(collision.contactCount-1).point.y > transform.position.y - .2f) return;

            if(!CameraSwitching.IsIn3D && collision.gameObject != _lastCollisionObject)
            {
                if (collision.GetContact(collision.contactCount -1).normal.y <= 0.5f) { return; }

                float dir = Mathf.Clamp((collision.GetContact(collision.contactCount - 1).point.z - transform.position.z), -1, 1);

                transform.position = new Vector3(transform.position.x, transform.position.y, collision.gameObject.transform.position.z);
                /*transform.position = new Vector3(transform.position.x, 
                    transform.position.y, collision.GetContact(collision.contactCount-1).point.z);*/
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
            transform.GetChild(0).GetChild(0).GetChild(0).transform.localEulerAngles = Vector3.zero;
            if (!CameraSwitching.IsIn3D)
            {
                controlDir.y = 0;
            }

            tempVel = new Vector3(controlDir.x * _moveSpeed, _rigidBody.velocity.y, controlDir.y * _moveSpeed);

            if (_jumpQueued)
            {
                _jumpQueued = false;
                tempVel.y = _jumpForce;
                AudioManager.Instance.PlayOneShotSound("Jump");
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

    IEnumerator MovementAnimation()
    {
        if (!_isAnimatingMovement)
        {
            _isAnimatingMovement = true;

            while (true)
            {
                if (!_stretched)
                {
                    _stretched = true;
                    AudioManager.Instance.PlayOneShotSound("Move1");
                    yield return Tween.Scale(transform.GetChild(0).GetChild(0),
                        new Vector3(1.2f, .8f, 1.2f), _moveAnimSpeed).ToYieldInstruction();
                }
                else
                {
                    _stretched = false;
                    AudioManager.Instance.PlayOneShotSound("Move2");
                    yield return Tween.Scale(transform.GetChild(0).GetChild(0),
                        new Vector3(1f, 1f, 1f), _moveAnimSpeed).ToYieldInstruction();
                }
            }
        }
    }

    private void ResetStretch()
    {
        _isAnimatingMovement = false;
        _stretched = false;
        Tween.Scale(transform.GetChild(0).GetChild(0), new Vector3(1f, 1f, 1f), .3f, Ease.Default, 1, CycleMode.Yoyo);
    }

    public void RespawnPlayer()
    {
        _rigidBody.velocity = Vector3.zero;
        transform.position = RespawnPoint;
    }
}
