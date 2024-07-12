using UnityEngine;
using UnityEngine.Animations;

public class EnemyController : MonoBehaviour
{
    //enum AI
    //{
    //    Idle,
    //    Patrol,
    //    FollowTarget,
    //    AttackTarget,
    //}
    //private AI _ai;
    private Rigidbody2D rigid;

    [Header("Basic Move")]
    public float moveSpeed;
    private float moveDirection = 1;
    private bool facingRight = true;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float circleRadius;
    private bool checkingWall;

    [Header("Jump Attack")]
    [SerializeField] float jumpHeight;
    public Transform player { get; private set; }
    [SerializeField] Transform groundCheck;
    [SerializeField] Vector2 boxSize;
    private bool isGrounded;

    [Header("Seeing Player")]
    [SerializeField] Vector2 lineOfSite;
    [SerializeField] Transform Sight;
    [SerializeField] LayerMask playerLayer;
    //[SerializeField] float detectOffset;
    private bool canSeePlayer;

    [Header("Other")]
    private Animator enemyAnim;
    public float cooldown;
    float lastjump;

    //[SerializeField] SpriteRenderer spriteRenderer;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        //UpdateAI();
        //rigid.velocity = new Vector2(speed, rigid.velocity.y);
        checkingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, groundLayer);
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0,groundLayer);
        //int direction = spriteRenderer.flipX ? -1 : 1;
        canSeePlayer = Physics2D.OverlapBox(Sight.position, lineOfSite, 0, playerLayer);
        AnimationController();
        if (!canSeePlayer && isGrounded)
        {
            Patrolling();
        }
        //FlipTowardsPlayer();
    }

    /*private void UpdateAI()
    {
        switch (_ai)
        {
            case AI.Idle:
                break;
            case AI.Patrol:
                {

                }
                break;
            case AI.FollowTarget:
                break;
            case AI.AttackTarget:
                break;
            default:
                break;
        }
    }*/

    private void Patrolling()
    {
        rigid.velocity = new Vector2(moveSpeed * moveDirection, rigid.velocity.y);

        if (checkingWall)
        {
            if (facingRight)
            {
                Flip();
            }
            else if (!facingRight)
            {
                Flip();
            }
        }
    }
    private void JumpAttack()
    {
        if (Time.time - lastjump < cooldown)
        {
            return;
        }
        lastjump = Time.time;
        float distanceFromPlayer = player.position.x - transform.position.x;
        if (isGrounded)
        {
            rigid.AddForce(new Vector2(distanceFromPlayer, jumpHeight),ForceMode2D.Impulse);
        }

    }

    void FlipTowardsPlayer()
    {
        float playerPosition = player.position.x - transform.position.x;
        if (playerPosition < 0 && facingRight)
        {
            Flip();
        }
        else if (playerPosition > 0 && !facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        moveDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    void AnimationController()
    {
        enemyAnim.SetBool("canSeePlayer",canSeePlayer);
        enemyAnim.SetBool("isGrounded",isGrounded);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(groundCheck.position,boxSize);

        Gizmos.color = Color.red;
        //int direction = spriteRenderer.flipX ? -1 : 1;
        Gizmos.DrawWireCube(Sight.position, lineOfSite);
    }
}