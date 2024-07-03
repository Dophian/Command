using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;
    float axisH = 0.0f;
    public float speed = 1.0f;
    public float sprint = 3.0f;

    public float jump = 3.0f;
    public LayerMask grondLayer;
    bool goJump = false;
    bool onGround = false;

    // 따닥
    int buttonPresses = 0;
    float buttonPressTimer = 0.5f;

    // 애니메이션
    Animator animator;
    public string stopAnime = "KirbyIdle";
    public string walkAnime = "KirbyWalk";
    public string jumpAnime = "KirbyJump";
    string nowAnime = "";
    string oldAnime = "";

    private void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;
    }

    private void Update()
    {
        axisH = Input.GetAxisRaw("Horizontal");

        if (axisH > 0.0f)
        {
            Debug.Log("오른쪽 이동");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            Debug.Log("왼쪽 이동");
            transform.localScale = new Vector2(-1, 1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (axisH == 0)
        {
            rbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // 따닥


    }

    private void FixedUpdate()
    {
        // 바닥면과 충돌했는지 확인하는 코드.
        // onGround가 true로 설정돼어야 지면에 있다고 판단해서 걸어다님.
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.01f), grondLayer);

        if (onGround || axisH != 0)
        {
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);
        }

        if (onGround && goJump)
        {
            Debug.Log("점프!");
            Vector2 jumpPw = new Vector2(0, jump);
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;
        }

        if (onGround)
        {
            if (axisH == 0)
            {
                nowAnime = stopAnime;
            }
            else
            {
                nowAnime = walkAnime;
            }
        }

        else
        {
            nowAnime = jumpAnime;
        }

        if (nowAnime != oldAnime)
        {
            Debug.Log(nowAnime);
            Debug.Log(oldAnime);

            oldAnime = nowAnime;
            animator.Play(nowAnime);
        }
    }

    public void Jump()
    {
        goJump = true;
        Debug.Log("점프 버튼 눌림!");
    }
}
