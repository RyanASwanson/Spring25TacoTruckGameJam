using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUIPopup : MonoBehaviour
{
    private Animator _animator;
    public string AnimationInTrigger;
    public string AnimationOutTrigger;

    public void StartPopUp()
    {
        _animator = GetComponentInChildren<Animator>();

        _animator.SetTrigger(AnimationInTrigger);
    }

    public void StopPopUp()
    {
        _animator.SetTrigger(AnimationOutTrigger);
        Destroy(gameObject, 3f);
    }
}
