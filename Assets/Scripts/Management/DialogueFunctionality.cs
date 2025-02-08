using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueFunctionality : MonoBehaviour
{
    public TMP_Text DialogueTextUI;

    public void PlayDialogue(IndividualDialogue individualDialogue)
    {
        DialogueTextUI.text = individualDialogue.DialogueText;
    }

}
