using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMicrowaveTrigger : MonoBehaviour
{
    [SerializeField] private int _sceneIndexToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneChanger.Instance.LoadScene(_sceneIndexToLoad);
        }
    }
}
