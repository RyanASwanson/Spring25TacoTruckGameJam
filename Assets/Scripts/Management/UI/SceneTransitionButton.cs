using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionButton : MonoBehaviour
{
    public void StartSceneTransition(int id)
    {
        MenuMicrowaveController.Instanace.CancelledRotation();
        MenuMicrowaveController.Instanace.DisableMicrowaveControls();
        GeneralCamera.Instance.EnableGameCameras();
        PlayerController.Instance.EnablePlayerControls();
        SceneManager.UnloadSceneAsync(0);
    }
}
