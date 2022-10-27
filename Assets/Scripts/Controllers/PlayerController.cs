using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    Animator anim;

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

    [SerializeField]
    float _speed = 12f;
    [SerializeField]
    float _jumpForce = 6f;
    [SerializeField]
    float _rotationSpeed = 15f;


    float currentSpeed
    {
        get
        {
            return _onGround ? _speed : _speed * 0.7f;
        }
    }

    bool _moveToDest = false;
    float wait_run_ratio = 0;
    Vector3 _destPos;

    float h, v;

    private Rigidbody myRigid;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction += OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

        myRigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
            anim.SetFloat("wait_run_ratio", wait_run_ratio);
            anim.Play("WAIT_RUN");
        }

        if (_moveToDest)
        {
            wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
            anim.SetFloat("wait_run_ratio", wait_run_ratio);
            anim.Play("WAIT_RUN");
            Vector3 dir = _destPos - transform.position;
            if(dir.magnitude < 0.0001f)
            {
                _moveToDest = false;
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(dir.x,0,dir.z)), _rotationSpeed * Time.deltaTime);
                if (_contactWall) myRigid.velocity = new Vector3(0, myRigid.velocity.y, 0);
                else
                {
                    float moveDist = Mathf.Clamp(currentSpeed * Time.deltaTime, 0 , dir.magnitude);
                    
                    myRigid.MovePosition(transform.position + dir.normalized * moveDist);
                }
            }
        }
        else if(Input.GetAxisRaw("Horizontal")==0f && Input.GetAxisRaw("Vertical") ==0f)
        {
            wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
            anim.SetFloat("wait_run_ratio", wait_run_ratio);
            anim.Play("WAIT_RUN");
        }

        
    }

    void OnKeyboard()
    {
        Move();
        Jump();
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (evt != Define.MouseEvent.Click) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Ground")))
        {
            _destPos = hit.point;
            _moveToDest = true;
            //Debug.Log($"Raycast Camera @{hit.collider.gameObject.name}");

        }
    }
    void Move()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        if (h != 0f || v != 0f)
        {
            _moveToDest = false;
            wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
            anim.SetFloat("wait_run_ratio", wait_run_ratio);
            anim.Play("WAIT_RUN");
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
