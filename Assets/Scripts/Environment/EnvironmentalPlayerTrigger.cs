using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentalPlayerTrigger : MonoBehaviour
{
    [SerializeField] private float _activationDelay;
    [SerializeField] private bool _destroyOnCollision;
    private bool _hasContacted = false;

    [Space]
    [SerializeField] private GameObject _spawnHint;
    [SerializeField] private IndividualDialogue _dialogueFunctionality;
    DialogueFunctionality _spawnedHint;
    private bool _hasHintSpawned = false;

    [Space]
    [SerializeField] private UnityEvent _onPlayerContact;


    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG) && !_hasContacted)
        {
            PlayerContact();
        }
    }

    private void PlayerContact()
    {
        if (_destroyOnCollision)
            _hasContacted = true;

        StartCoroutine(DelayedContact());
    }

    private IEnumerator DelayedContact()
    {
        yield return new WaitForSeconds(_activationDelay);
        _onPlayerContact.Invoke();
        SpawnUIElement();

        if(_destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnUIElement()
    {
        if(_spawnHint != null && !_hasHintSpawned)
        {
            _hasHintSpawned = true;
            _spawnedHint = Instantiate(_spawnHint, transform).GetComponentInChildren<DialogueFunctionality>();
            _spawnedHint.PlayDialogue(_dialogueFunctionality);
        }
    }

    public void RemoveUIElement()
    {
        _spawnedHint.OutroAnimation();
    }
}
