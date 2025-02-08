using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentalPlayerTrigger : MonoBehaviour
{
    [SerializeField] private bool _destroyOnCollision;

    [Space]
    [SerializeField] private UnityEvent _onPlayerContact;

    private const string PLAYER_TAG = "Player";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            PlayerContact();
        }
    }

    private void PlayerContact()
    {
        _onPlayerContact.Invoke();

        if(_destroyOnCollision)
        {
            Destroy(gameObject);
        }
    }
}
