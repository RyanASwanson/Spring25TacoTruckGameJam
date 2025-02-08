using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueFunctionality : MonoBehaviour
{
    public TMP_Text DialogueTextUI;

    private Animator _animator;
    public string AnimationInTrigger;
    public string AnimationOutTrigger;

    public void PlayDialogue(IndividualDialogue individualDialogue)
    {
        _animator = GetComponentInChildren<Animator>();

        DialogueTextUI.text = individualDialogue.DialogueText;

        _animator.SetTrigger(AnimationInTrigger);
    }

    public void OutroAnimation()
    {
        _animator.SetTrigger(AnimationOutTrigger);
        Destroy(gameObject, 3f);
    }
}
