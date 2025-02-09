using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private const string PLAYER_LAYER = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_LAYER))
        {
            PlayerMovement.Instance.RespawnPlayer();
        }
    }
}
