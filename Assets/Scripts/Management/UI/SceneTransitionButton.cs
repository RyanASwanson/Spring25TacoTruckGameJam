using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionButton : MonoBehaviour
{
    public void StartSceneTransition(int id)
    {
        AudioManager.Instance.StopSound("MenuMusic");
        SceneChanger.Instance.LoadScene(id);
    }
}
