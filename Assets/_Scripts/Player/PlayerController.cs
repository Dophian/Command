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
    private float _doubleTapTime = 0.25f;


    Rigidbody2D rigidbody;
    public float speed;
    public float defaultspeed;

    // 대쉬
    public bool isDash;
    public float dashSpeed;
    public float defaultTime;
    private float dashTime;

    SpriteRenderer spriteRenderer;

    private bool isJumping;
    private int maxJumps = 3;
    private int remainingJumps;

    public float jump = 3.0f;
    public LayerMask grondLayer;
    bool goJump = false;
    bool onGround = false;
    Animator anim;

    private void Start()
    {
        defaultspeed = speed;
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();


        _tapInfos[0].onDoubleTap += () => Debug.Log("A 키 더블탭");
        _tapInfos[1].onDoubleTap += () => Debug.Log("D 키 더블탭");
    }

    private void Update()
    {
        for (int i = 0; i < _tapInfos.Length; i++)
        {
            if (Input.GetKeyDown(_tapInfos[i].keyCode))
            {
                anim.SetBool("IsDash", true);

                float elapsedTime = Time.time - _tapInfos[i].tapTime;

                if (elapsedTime <= _doubleTapTime)
                    defaultspeed = speed * 3;
                _tapInfos[i].tapTime = Time.time;
            }
            if (Input.GetKeyUp(_tapInfos[i].keyCode))
            defaultspeed = speed;
            anim.SetBool("IsDash", false);

        }

        float hor = Input.GetAxis("Horizontal");
        rigidbody.velocity = new Vector2(hor * defaultspeed, rigidbody.velocity.y);

        // 걷기 애니메이션.
        if (rigidbody.velocity.normalized.x == 0)
            anim.SetBool("IsWalking", false);
        else
            anim.SetBool("IsWalking", true);

        // 공중점프
        if (onGround)

        // 점프&애니메이션.
        if (Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            anim.SetBool("IsJump", true);
        }

        // 방향 전환.
        if (Mathf.Abs(hor) > 0)
            spriteRenderer.flipX = hor < 0;
    }

    private void FixedUpdate()
    {
        // 바닥면과 충돌했는지 확인하는 코드.
        // onGround가 true로 설정돼어야 지면에 있다고 판단해서 걸어다님.
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.01f), grondLayer);

        if (rigidbody.velocity.y < 0)
        {
            Debug.DrawRay(rigidbody.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigidbody.position, Vector3.down, 1, LayerMask.GetMask("Ground"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("IsJump", false);
            }
        }

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    isDash = true;
        //    anim.SetBool("IsDash", true);
        //}

        //if(dashTime <= 0)
        //{
        //    defaultspeed = speed;
        //    if (isDash)
        //    {
        //        dashTime = defaultTime;
        //    }
            
        //}
        //else
        //{
            
        //    dashTime -= Time.deltaTime;
        //    defaultspeed = dashSpeed;
        //}
        //isDash = false;
        //anim.SetBool("IsDash", false);
    }
}
