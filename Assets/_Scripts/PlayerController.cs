using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 따닥
    private struct Tapinfo
    {
        public Tapinfo(KeyCode keyCode, float tapTime)
        {
            this.keyCode = keyCode;
            this.tapTime = tapTime;
            onDoubleTap = null;
        }

        public KeyCode keyCode;
        public float tapTime;
        public Action onDoubleTap;
    }
    private Tapinfo[] _tapInfos =
        { 
        new Tapinfo(KeyCode.A, 0f),
        new Tapinfo(KeyCode.D, 0f)
        };

    private float _doubleTapTime = 0.5f;


    Rigidbody2D rigidbody;
    public float speed;
    public float defaultspeed;


    public float jump = 3.0f;
    public LayerMask grondLayer;
    bool goJump = false;
    bool onGround = false;

    private void Start()
    {
        defaultspeed = speed;
        rigidbody = GetComponent<Rigidbody2D>();

        _tapInfos[0].onDoubleTap += () => Debug.Log("W 키 더블탭");
        _tapInfos[1].onDoubleTap += () => Debug.Log("D 키 더블탭");
    }

    private void Update()
    {
        for (int i = 0; i < _tapInfos.Length; i++)
        {
            if (Input.GetKeyDown(_tapInfos[i].keyCode))
            {
                float elapsedTime = Time.time - _tapInfos[i].tapTime;

                if (elapsedTime <= _doubleTapTime)
                    defaultspeed = speed * 3;

                _tapInfos[i].tapTime = Time.time;
            }
        }

        float hor = Input.GetAxis("Horizontal");
        rigidbody.velocity = new Vector2(hor * defaultspeed , rigidbody.velocity.y);

        //if(Input.GetKey(KeyCode.LeftShift))
        //{
        //    defaultspeed = speed * 3;
        //}
        //else
        //{
        //    defaultspeed = speed;
        //}

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        // 바닥면과 충돌했는지 확인하는 코드.
        // onGround가 true로 설정돼어야 지면에 있다고 판단해서 걸어다님.
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.01f), grondLayer);


        if (onGround && goJump)
        {
            Debug.Log("점프!");
            Vector2 jumpPw = new Vector2(0, jump);
            rigidbody.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;
        }

    }

    public void Jump()
    {
        goJump = true;
        Debug.Log("점프 버튼 눌림!");
    }
}
