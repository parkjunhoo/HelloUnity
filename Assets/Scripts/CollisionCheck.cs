using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{

    

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(7)) PlayerController.OnGround = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer.Equals(7))PlayerController.OnGround = true;
    }
    


    void Update()
    {
        WallCheck();
       //_GroundCheck();
    }


    
    private void WallCheck()
    {
        Vector3 pos = transform.position;
        Debug.DrawRay(pos ,transform.forward * 0.161f, Color.green);
        int layer = (1 << 6) | (1 << 7);
        if (Physics.Raycast(pos, transform.forward, 0.161f, layer))
        {
            Debug.Log("Wall Contact = true");
            PlayerController.ContactWall = true;
            return;
        }
        PlayerController.ContactWall = false;
        return;
    }
    private void _GroundCheck()
    {
        Debug.DrawRay(transform.position, -Vector3.up * 0.02f, Color.red);
        if (Physics.Raycast(transform.position, -transform.up, 0.02f))
        {
            PlayerController.OnGround = true;
            return;
        }
        PlayerController.OnGround = false;
        return;
    }
    
}
