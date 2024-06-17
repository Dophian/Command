using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;
    float axisH = 0.0f;
    public float speed = 1.0f;

    public float jump = 3.0f;
    public LayerMask grondLayer;
    bool goJump = false;
    bool onGround = false;

    private bool CanDash = true;
    private bool IsDashing;
    [SerializeField] private float DashPower = 5f;
    [SerializeField] private float DashCooldown = 1f;
    [SerializeField] private float DashTime = 0.4f;

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

        CanDash = true;
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && CanDash)
        {
            StartCoroutine(Dash());
        }

    }

    private void FixedUpdate()
    {
        onGround = Physics2D.Linecast(transform.position,
                                      transform.position - (transform.up * 0.01f),
                                      grondLayer);
        
        if (onGround || axisH != 0)

        {
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);
        }
        if(onGround && goJump)
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

        if(nowAnime != oldAnime)
        {
            Debug.Log(nowAnime);
            Debug.Log(oldAnime);

            oldAnime = nowAnime;
            animator.Play(nowAnime);
            
        }

        if (IsDashing)
        {
            return;
        }

        rbody.velocity = new Vector2(axisH * speed, rbody.velocity.y);
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        IsDashing = true;
        float OriginalGravity = rbody.gravityScale;
        rbody.gravityScale = 0f;
        rbody.velocity = new Vector2(axisH * DashPower, 0f);
        yield return new WaitForSeconds(DashTime);
        rbody.gravityScale = OriginalGravity;
        IsDashing = false;
        yield return new WaitForSeconds(DashCooldown);
        CanDash = true;
    }

    public void Jump()
    {
        goJump = true;
        Debug.Log("점프 버튼 눌림!");
    }
}
