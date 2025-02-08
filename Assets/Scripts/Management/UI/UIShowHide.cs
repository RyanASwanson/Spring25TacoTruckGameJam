using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShowHide : MonoBehaviour
{
    [SerializeField] private CanvasGroup _associatedUI;

    [Header("Animation")]
    [SerializeField] private bool _animateUI;
    [SerializeField] private string _animateInTrigger;
    [SerializeField] private string _animateOutTrigger;

    private bool _showingAssociatedUI = false;
    private Animator _associatedAnimator;

    private void Start()
    {
        _associatedAnimator = _associatedUI.GetComponent<Animator>();
        if (_associatedAnimator == null)
            _associatedAnimator = GetComponentInParent<Animator>();
    }

    public void ButtonPress()
    {
        _showingAssociatedUI = !_showingAssociatedUI;

        _associatedUI.alpha = _showingAssociatedUI ? 1 : 0;

        MainMenuController.Instance.ShowingMainMenuUI = _showingAssociatedUI ? this : null;

        if (!_animateUI) return;

        if(_showingAssociatedUI)
        {
            _associatedAnimator.SetTrigger(_animateInTrigger);
            return;
        }
        _associatedAnimator.SetTrigger(_animateOutTrigger);
    }
}
