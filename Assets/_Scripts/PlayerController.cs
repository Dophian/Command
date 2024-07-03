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

    // ����
    int buttonPresses = 0;
    float buttonPressTimer = 0.5f;

    // �ִϸ��̼�
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
            Debug.Log("������ �̵�");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            Debug.Log("���� �̵�");
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

        // ����


    }

    private void FixedUpdate()
    {
        // �ٴڸ�� �浹�ߴ��� Ȯ���ϴ� �ڵ�.
        // onGround�� true�� �����ž�� ���鿡 �ִٰ� �Ǵ��ؼ� �ɾ�ٴ�.
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.01f), grondLayer);

        if (onGround || axisH != 0)
        {
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);
        }

        if (onGround && goJump)
        {
            Debug.Log("����!");
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
        Debug.Log("���� ��ư ����!");
    }
}
