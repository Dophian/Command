using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rigid;
    public float speed;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(speed, rigid.velocity.y);
    }
}
