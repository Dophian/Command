using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ����
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

    // �뽬
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


        _tapInfos[0].onDoubleTap += () => Debug.Log("A Ű ������");
        _tapInfos[1].onDoubleTap += () => Debug.Log("D Ű ������");
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

        // �ȱ� �ִϸ��̼�.
        if (rigidbody.velocity.normalized.x == 0)
            anim.SetBool("IsWalking", false);
        else
            anim.SetBool("IsWalking", true);

        // ��������
        if (onGround)

        // ����&�ִϸ��̼�.
        if (Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            anim.SetBool("IsJump", true);
        }

        // ���� ��ȯ.
        if (Mathf.Abs(hor) > 0)
            spriteRenderer.flipX = hor < 0;
    }

    private void FixedUpdate()
    {
        // �ٴڸ�� �浹�ߴ��� Ȯ���ϴ� �ڵ�.
        // onGround�� true�� �����ž�� ���鿡 �ִٰ� �Ǵ��ؼ� �ɾ�ٴ�.
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
