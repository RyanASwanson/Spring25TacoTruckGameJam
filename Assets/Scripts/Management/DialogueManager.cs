using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public IndividualDialogue[] AllDialogue;
    private static int _currentDialoguePosition;

    public GameObject DialoguePrefab;

    private bool _isDialogueActive = false;

    public static DialogueManager Instance;

    private Queue<IndividualDialogue> _currentDialogueChain = new Queue<IndividualDialogue>();

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            PlayNextMainDialogue();
        }
    }

    public void PlayNextMainDialogue()
    {
        if (_currentDialoguePosition < AllDialogue.Length)
        {
            PlayDialogue(AllDialogue[_currentDialoguePosition]);
        }
        _currentDialoguePosition++;
    }

    public void PlayDialogue(IndividualDialogue individualDialogue)
    {
        _currentDialogueChain.Enqueue(individualDialogue);

        if(!_isDialogueActive)
        {
            StartCoroutine(DialogueProcess());
        }
    }

    private IEnumerator DialogueProcess()
    {
        _currentDialogueChain.TryPeek(out IndividualDialogue individualDialogue);

        if (individualDialogue != null)
        {
            _isDialogueActive = true;
            individualDialogue = _currentDialogueChain.Dequeue();

            GameObject newestDialogueGameObject = Instantiate(DialoguePrefab, transform);
            newestDialogueGameObject.GetComponent<DialogueFunctionality>().PlayDialogue(individualDialogue);
            yield return new WaitForSeconds(individualDialogue.DialogueLength);
            Destroy(newestDialogueGameObject);

            _isDialogueActive = false;

            StartCoroutine(DialogueProcess());
        }
    }
}

[System.Serializable]
public class IndividualDialogue
{
    public AudioClip DialogueAudio;
    public float DialogueLength;
    public string DialogueText;
}