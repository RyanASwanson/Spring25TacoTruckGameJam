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
    [SerializeField] private UnityEvent _onPlayerContact;

    private const string PLAYER_TAG = "Player";

    // Start is called before the first frame update
    void Start()
    {
        
    }

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

        if(_destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }
}
