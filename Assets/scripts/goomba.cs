using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyWalker : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private LayerMask solidMask;
    [SerializeField] private float wallCheckDistance = 0.1f;
    [SerializeField] private float sideOffset = 0.45f;

    private Rigidbody2D rb;
    private int dir = -1;
    private bool dead;

    public bool IsDead => dead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (dead) return;

        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);

        Vector2 origin = (Vector2)transform.position + Vector2.right * (dir * sideOffset);
        Vector2 castDir = dir > 0 ? Vector2.right : Vector2.left;

        bool hitWall = Physics2D.Raycast(origin, castDir, wallCheckDistance, solidMask);

        if (hitWall)
        {
            dir *= -1;
            FlipVisual();
        }
    }

    private void FlipVisual()
    {
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * dir;
        transform.localScale = s;
    }

    public void Die()
    {
        if (dead) return;

        dead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
}