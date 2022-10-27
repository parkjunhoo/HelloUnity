using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuaterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0, 6, -5);

    [SerializeField]
    GameObject _player = null;
    void Start()
    {
        
    }


    void LateUpdate()
    {
        if (_mode.Equals(Define.CameraMode.QuaterView))
        {
            Vector3 playerPos = _player.transform.position + Vector3.up * 3f;
            if (Physics.Raycast(playerPos, _delta, out RaycastHit hit, _delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - playerPos).magnitude * 0.8f;
                transform.position = playerPos + _delta.normalized * dist;
            }
            else
            {
                transform.position = playerPos + _delta;
                transform.LookAt(playerPos);
            }
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuaterView;
        _delta = delta;
    }
}
