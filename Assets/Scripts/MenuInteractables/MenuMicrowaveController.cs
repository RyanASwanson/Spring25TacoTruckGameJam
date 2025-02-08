using PrimeTween;
using UnityEngine.InputSystem;
using UnityEngine;

public class MenuMicrowaveController : MonoBehaviour
{
    public static MenuMicrowaveController Instanace;
    
    [SerializeField] private Transform _microwavePlate;
    [SerializeField] private float _microwaveOpenDelay = 0.75f;
    [SerializeField] private Quaternion _microwaveLeftRotation;
    [SerializeField] private Quaternion _microwaveRightRotation;
    [SerializeField] private float _angularRotationSpeed = -2f;

    private Animator _microwaveAnimator;
    private PlayerCameraInputActionMap _menuInteractions;

    private Tween _microwaveRotationTween;

    private void Awake()
    {
        if (Instanace == null)
        {
            Instanace = this;
        }
        else
        {
            Destroy(this);
        }
        
        _menuInteractions = new PlayerCameraInputActionMap();
        _menuInteractions.Enable();
        
        _menuInteractions.MainMenu.RotateMicrowaveRight.performed += ctx => RotateMicrowaveRight();
        _menuInteractions.MainMenu.RotateMicrowaveLeft.performed += ctx => RotateMicrowaveLeft();
        _menuInteractions.MainMenu.StopMicrowave.performed += ctx => CancelledRotation();
    }

    private void OnDestroy()
    {
        DisableMicrowaveControls();
    }
    
    private void Start()
    {
        // Must have microwave animator to animate
        if (!TryGetComponent(out _microwaveAnimator))
        {
            Debug.LogError("Microwave Animator not found");
            return;
        }
        
        //Invoke("OpenMicrowave", _microwaveOpenDelay);
    }

    public void DisableMicrowaveControls()
    {
        _menuInteractions.MainMenu.RotateMicrowaveRight.performed -= ctx => RotateMicrowaveRight();
        _menuInteractions.MainMenu.RotateMicrowaveLeft.performed -= ctx => RotateMicrowaveLeft();
        _menuInteractions.MainMenu.StopMicrowave.performed += ctx => CancelledRotation();
        
        _menuInteractions.Disable();
    }

    private void OpenMicrowave()
    {
        _microwaveAnimator?.SetTrigger("ShouldOpen");
    }

    private void RotateMicrowaveLeft()
    {
        _microwaveRotationTween.Stop();
        _microwaveRotationTween =
            Tween.EulerAngles(_microwavePlate, _microwavePlate.eulerAngles, 
                _microwavePlate.eulerAngles - _microwaveLeftRotation.eulerAngles, 4, Ease.Linear).OnComplete(() => RotateMicrowaveLeft());
    }

    private void RotateMicrowaveRight()
    {
        _microwaveRotationTween.Stop();
        _microwaveRotationTween =
            Tween.EulerAngles(_microwavePlate, _microwavePlate.eulerAngles,
                _microwavePlate.eulerAngles + _microwaveRightRotation.eulerAngles, 4, Ease.Linear).OnComplete(() => RotateMicrowaveRight());
    }

    public void CancelledRotation()
    {
        _microwaveRotationTween.Stop();
    }
    
}
