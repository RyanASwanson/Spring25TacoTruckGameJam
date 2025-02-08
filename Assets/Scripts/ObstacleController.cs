using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] float _knockbackForce = 5f;
    [SerializeField] Vector3 _knockbackDirection = Vector3.forward;
    
    private Animator _obstacleAnimator;
    private bool _canHitPlayer = false;

    private void Awake()
    {
        _obstacleAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _obstacleAnimator.SetTrigger("ShouldOpen");
            _canHitPlayer = true;
        }
    }

    public void TryHitPlayer()
    {
        if (_canHitPlayer)
        {
            PlayerMovement.Instance.ReceiveKnockback(_knockbackForce, _knockbackDirection);
        }
    }

    public void ResetCollisionChecking()
    {
        _canHitPlayer = false;
    }
}
