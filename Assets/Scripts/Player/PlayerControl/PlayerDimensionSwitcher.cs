using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDimensionSwitcher : MonoBehaviour
{
    [SerializeField] private BoxCollider _playerCollider;
    [SerializeField] private float _hitboxDepth;

    private float _defaultHitboxDepth;

    // Start is called before the first frame update
    void Start()
    {
        _defaultHitboxDepth = _playerCollider.size.z;
        _playerCollider.size = new Vector3(_playerCollider.size.x, _playerCollider.size.y, _hitboxDepth);
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        CameraSwitching._on2DSwitchEvent.AddListener(SwitchTo2D);
        CameraSwitching._on3DSwitchEvent.AddListener(SwitchTo3D);
    }

    private void SwitchTo2D()
    {
        _playerCollider.size = new Vector3(_playerCollider.size.x, _playerCollider.size.y, _hitboxDepth);
    }

    private void DetermineNearestWall()
    {
        //float dist = 
    }

    private void SwitchTo3D()
    {
        _playerCollider.size = new Vector3(_playerCollider.size.x, _playerCollider.size.y, _defaultHitboxDepth);
    }
}
