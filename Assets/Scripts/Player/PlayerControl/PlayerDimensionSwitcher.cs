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
        Set2DScale();
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

    private void Update()
    {
        if(!CameraSwitching.IsIn3D)
        {
            Set2DScale();
        }
    }

    private void Set2DScale()
    {
        _playerCollider.size = new Vector3(_playerCollider.size.x, _playerCollider.size.y, DetermineNearestWall() * 1.97f);
    }

    private float DetermineNearestWall()
    {
        float dist = _hitboxDepth;
        if (Physics.BoxCast(transform.position, Vector3.one*.3f, Vector3.back, out RaycastHit hit, Quaternion.identity, _hitboxDepth, _wallBlockers))
        {
            float newDist = Vector3.Distance(transform.position, hit.point);
            if (newDist < dist)
            {
                dist = newDist;
            }
        }
        if (Physics.BoxCast(transform.position, Vector3.one*.3f, Vector3.forward, out RaycastHit hit1, Quaternion.identity, _hitboxDepth, _wallBlockers))
        {
            float newDist = Vector3.Distance(transform.position, hit1.point);
            if (newDist < dist)
            {
                dist = newDist;
            }
            //print(newDist);
        }

        Debug.DrawLine(transform.position, transform.position + (Vector3.forward * _hitboxDepth),Color.red,2f);
        /*if(Physics.Raycast(transform.position, Vector3.back, out RaycastHit hit, _hitboxDepth, _wallBlockers))
        {
            
        }*/

        /*if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit2, _hitboxDepth, _wallBlockers))
        {
            float newDist = Vector3.Distance(transform.position, hit2.point);
            if (newDist < dist)
            {
                dist = newDist;
            }
        }*/

        return dist;
    }

    private void SwitchTo3D()
    {
        _playerCollider.size = new Vector3(1,1,1);
    }
}
