using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MushroomPowerUp : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float speed = 2.2f;
    [SerializeField] private LayerMask solidMask;
    [SerializeField] private float wallCheckDistance = 0.08f;
    [SerializeField] private float sideOffset = 0.35f;

    private Rigidbody2D rb;
    private int dir = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);

        Vector2 origin = (Vector2)transform.position + Vector2.right * (dir * sideOffset);
        Vector2 castDir = dir > 0 ? Vector2.right : Vector2.left;

        bool hitWall = Physics2D.Raycast(origin, castDir, wallCheckDistance, solidMask);
        if (hitWall)
            dir *= -1;
    }
}