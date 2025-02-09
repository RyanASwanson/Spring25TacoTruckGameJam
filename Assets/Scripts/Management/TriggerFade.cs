using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TriggerFade : MonoBehaviour
{
    [SerializeField] private GameObject fadeObject;
    public void BeginFade()
    {
        fadeObject.SetActive(true);
    }

    public void LoadFinalEnd()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
