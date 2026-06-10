using UnityEngine;

public class MariovsEnemy : MonoBehaviour
{
    [SerializeField] private float stompBounce = 8f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Enemy")) return;

        foreach (var c in col.contacts)
        {
            if (c.normal.y > 0.5f)
            {
                Destroy(col.collider.gameObject);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * stompBounce, ForceMode2D.Impulse);
                return;
            }
        }
    }
}