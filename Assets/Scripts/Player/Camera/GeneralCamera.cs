using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralCamera : MonoBehaviour
{
    public static GeneralCamera Instance;
    private GameObject[] Cameras;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Cameras = new GameObject[2];
        Cameras[0] = transform.GetChild(0).gameObject;
        Cameras[1] = transform.GetChild(1).gameObject;

        foreach (GameObject Camera in Cameras)
        {
            Camera.SetActive(false);
        }
    }

    public void EnableGameCameras()
    {
        foreach (GameObject Camera in Cameras)
        {
            Camera.SetActive(true);
        }

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
