using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    

    static bool _contactWall = false;
    static public bool ContactWall
    {
        get { return _contactWall; }
        set { _contactWall = value; }
    }


    static bool _onGround = true;
    static public bool OnGround
    {
        get { return _onGround; }
        set { _onGround = value; }
    }


    float currentSpeed
    {
        get
        {
            return _onGround ? _speed : _speed * 0.7f;
        }
    }

    [SerializeField]
    float _speed = 12f;
    [SerializeField]
    float _jumpForce = 6f;
    [SerializeField]
    float _rotationSpeed = 15f;



    float h, v;
    bool _moveToDest = false;
    Vector3 _destPos;

    float wait_run_ratio = 0;


    private Rigidbody myRigid;
    private Animator anim;

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
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        if (_moveToDest)
        {
            
            Vector3 dir = _destPos - transform.position;
            if(dir.magnitude < 0.0001f)
            {
                _moveToDest = false;
            }
            else
            {
                wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
                anim.SetFloat("wait_run_ratio", wait_run_ratio);
                anim.Play("WAIT_RUN");
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(dir.x,0,dir.z)), _rotationSpeed * Time.deltaTime);
                if (_contactWall) myRigid.velocity = new Vector3(0, myRigid.velocity.y, 0);
                else
                {
                    float moveDist = Mathf.Clamp(currentSpeed * Time.deltaTime, 0 , dir.magnitude);
                    
                    myRigid.MovePosition(transform.position + dir.normalized * moveDist);
                }
            }
        }
        else if(h==0 && v ==0)
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
        Move(evt);
    }

    void Move()
    {
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
        }
        return;
    }

    void Move(Define.MouseEvent evt)
    {
        if (evt != Define.MouseEvent.Click) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Ground")))
        {
            _destPos = hit.point;
            _moveToDest = true;
            //Debug.Log($"Raycast Camera @{hit.collider.gameObject.name}");

        }
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
