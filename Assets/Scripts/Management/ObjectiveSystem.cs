using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _objectiveText;
    [SerializeField] string[] _objectivesList;

    private int _currentIndex = 0;

    public static ObjectiveSystem Instance;

    private Animator _objectiveTextAnimator;

    private void Awake()
    {
        Instance = this;

        _objectiveTextAnimator = GetComponent<Animator>();
        _objectiveText.text = _objectivesList[0];
    }

    public void UpdateObjective(int objectiveIndex)
    {
        if (objectiveIndex == _currentIndex + 1 && objectiveIndex <= _objectivesList.Length)
        {
            _currentIndex = objectiveIndex;
            _objectiveText.text = _objectivesList[_currentIndex];
            _objectiveTextAnimator.SetTrigger("ShouldRepeat");
        }
    }
}
