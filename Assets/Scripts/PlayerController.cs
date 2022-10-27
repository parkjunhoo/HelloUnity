using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    static bool _contactWall = false;
    static bool _onGround = true;

    static public bool ContactWall
    {
        get { return _contactWall; }
        set { _contactWall = value; }
    }

    static public bool OnGround
    {
        get { return _onGround; }
        set { _onGround = value; }
    }

    float _speed = 12f;
    float _jumpForce = 6f;
    
    float _rotationSpeed = 15f;
    float h, v;

    private Rigidbody myRigid;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;

        myRigid = GetComponent<Rigidbody>();

    }


    void Update()
    {
        
    }

    void OnKeyboard()
    {
        Move();
        Jump();
    }
    void Move()
    {
        float currentSpeed = _onGround ? _speed : _speed * 0.7f;
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        if (h != 0f || v != 0f)
        {
            Vector3 inputDir = new Vector3(h, 0, v);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputDir), _rotationSpeed * Time.deltaTime);
            if (_contactWall) myRigid.velocity = new Vector3(0,myRigid.velocity.y,0);
            else
            {
                myRigid.MovePosition(transform.position + new Vector3(h, 0, v) * Time.deltaTime * currentSpeed);
            }
            //myRigid.MovePosition(transform.position + new Vector3(h, 0, v) * Time.deltaTime * currentSpeed);
            //transform.position += inputDir * Time.deltaTime * currentSpeed;
            //

        }
        return;
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_onGround) return;
            myRigid.AddForce( new Vector3(h, _jumpForce , v) , ForceMode.VelocityChange);
            Debug.Log("Jump");
        }
        return;
    }
}
