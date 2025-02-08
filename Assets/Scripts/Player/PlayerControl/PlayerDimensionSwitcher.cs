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
        
        
    }

    private void Update()
    {
        if(!CameraSwitching.IsIn3D)
        {
            _playerCollider.size = new Vector3(_playerCollider.size.x, _playerCollider.size.y, DetermineNearestWall() * .95f);
        }
    }

    private float DetermineNearestWall()
    {
        float dist = _hitboxDepth;
        if (Physics.BoxCast(transform.position, _playerCollider.size*.95f, Vector3.back, out RaycastHit hit, Quaternion.identity, _hitboxDepth, _wallBlockers))
        {
            print("Check");
            float newDist = Vector3.Distance(transform.position, hit.point);
            if (newDist < dist)
            {
                dist = newDist;
            }
        }
        if (Physics.BoxCast(transform.position, _playerCollider.size*.95f, Vector3.forward, out RaycastHit hit1, Quaternion.identity, _hitboxDepth, _wallBlockers))
        {
            print("Check1");
            float newDist = Vector3.Distance(transform.position, hit1.point);
            if (newDist < dist)
            {
                dist = newDist;
            }
            print(newDist);
        }
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
        _playerCollider.size = new Vector3(_playerCollider.size.x, _playerCollider.size.y, _defaultHitboxDepth);
    }
}
