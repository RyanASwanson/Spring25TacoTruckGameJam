using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FollowPlayer());
    }

    private IEnumerator FollowPlayer()
    {
        while(true)
        {
            transform.position = new Vector3(PlayerMovement.Instance.transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
