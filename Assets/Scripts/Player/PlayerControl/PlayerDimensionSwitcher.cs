using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDimensionSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        CameraSwitching._on2DSwitchEvent.AddListener(SwitchTo2D);
        CameraSwitching._on3DSwitchEvent.AddListener(SwitchTo3D);
    }

    private void SwitchTo2D()
    {
        
    }

    private void SwitchTo3D()
    {
        
    }
}
