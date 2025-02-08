using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDimensionSwitcher : MonoBehaviour
{
    [SerializeField] private BoxCollider _playerCollider;
    [SerializeField] private float _hitboxDepth;
    [SerializeField] private LayerMask _wallBlockers;

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
        DetermineNearestWall();
        _playerCollider.size = new Vector3(_playerCollider.size.x, _playerCollider.size.y, _hitboxDepth);
    }

    private void DetermineNearestWall()
    {
        float dist = _hitboxDepth;
        if(Physics.Raycast(transform.position, Vector3.back, out RaycastHit hit, _hitboxDepth, _wallBlockers))
        {
            print(hit.transform.gameObject.name);
            float newDist = Vector3.Distance(transform.position, hit.point);
            if (newDist < dist)
            {
                dist = newDist;
            }
        }

        if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit2, _hitboxDepth, _wallBlockers))
        {
            print(hit2.transform.gameObject.name);
            float newDist = Vector3.Distance(transform.position, hit.point);
            if (newDist < dist)
            {
                dist = newDist;
            }
        }

        Debug.DrawRay(transform.position, Vector3.forward, Color.red, 5);
        
        

        print(dist);
    }

    private void SwitchTo3D()
    {
        _playerCollider.size = new Vector3(_playerCollider.size.x, _playerCollider.size.y, _defaultHitboxDepth);
    }
}
