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

    public float speed;
    public float jump;
    //public Transform player;
    [SerializeField] LayerMask grondLayer;
    private bool facingRight = true;
    private float moveDirection = 1;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] float circleRadius;
    private bool checkingWall;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //UpdateAI();
        //rigid.velocity = new Vector2(speed, rigid.velocity.y);
        checkingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, grondLayer); 
        Patrolling();
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
        rigid.velocity = new Vector2(speed * moveDirection, rigid.velocity.y);

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
    //private void JumpAttack()
    //{
    //    float distanceFromPlayer = player.position.x - transform.position.x;
            
    //}

     void Flip()
    {
        moveDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius);
    }
}