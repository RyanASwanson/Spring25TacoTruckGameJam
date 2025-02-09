using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTransition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneChanger.Instance.LoadScene(2);
    }
}
