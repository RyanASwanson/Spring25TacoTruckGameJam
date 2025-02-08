using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCamera : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        transform.position = PlayerMovement.Instance.transform.position;
        StartCoroutine(FollowPlayer());
    }

    private IEnumerator FollowPlayer()
    {
        while(true)
        {
            transform.position = PlayerMovement.Instance.transform.position;
            yield return null;
        }
    }
}
