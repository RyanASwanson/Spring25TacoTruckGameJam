using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollisionChecker : MonoBehaviour
{
    [SerializeField] private ObstacleController _obstacleController;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _obstacleController.TryHitPlayer();
        }
    }
}
