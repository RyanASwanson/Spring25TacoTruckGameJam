using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingMicrowave : MonoBehaviour
{
    [SerializeField] float _doorOpenDelay = 1f;

    Animator _microwaveAnimator;

    private void Awake()
    {
        _microwaveAnimator = GetComponent<Animator>();

        Invoke(nameof(OpenDoor), _doorOpenDelay);
    }

    private void OpenDoor()
    {
        _microwaveAnimator.SetTrigger("ShouldOpen");
    }
}
