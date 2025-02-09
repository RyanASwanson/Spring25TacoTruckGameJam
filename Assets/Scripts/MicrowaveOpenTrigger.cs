using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveOpenTrigger : MonoBehaviour
{
    bool _endSequenceTriggered = false;

    [SerializeField] Animator _microwaveAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !_endSequenceTriggered)
        {
            print("collision worked");
            _endSequenceTriggered = true;
            EndSequence();
        }
    }

    private void EndSequence()
    {
        print("end sequence triggerd");
        _microwaveAnimator.Play("MW_Open");
    }
}
