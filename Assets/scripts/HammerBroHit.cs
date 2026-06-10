using UnityEngine;

public class HammerBroHit : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private HammerBroShooter shooter;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private SpriteRenderer visual;

    [Header("Stomp")]
    [SerializeField] private float stompBounceForce = 10f;
    [SerializeField] private float stompTolerance = 0.2f;

    private bool dead;

    private void Awake()
    {
        if (shooter == null)
            shooter = GetComponentInParent<HammerBroShooter>();

        if (bodyCollider == null)
            bodyCollider = GetComponentInParent<Collider2D>();

        if (visual == null)
            visual = GetComponentInParent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dead) return;

        VIdaMario marioHealth = other.GetComponentInParent<VIdaMario>();
        if (marioHealth == null) return;

        Rigidbody2D marioRb = marioHealth.GetComponent<Rigidbody2D>();
        if (marioRb == null) return;

        bool marioIsFalling = marioRb.linearVelocity.y <= 0f;
        bool marioIsAbove = other.bounds.min.y > bodyCollider.bounds.max.y - stompTolerance;

        if (marioIsFalling && marioIsAbove)
        {
            KillHammerBro(marioRb);
        }
        else
        {
            marioHealth.TakeDamage();
        }
    }

    private void KillHammerBro(Rigidbody2D marioRb)
    {
        dead = true;

        marioRb.linearVelocity = new Vector2(marioRb.linearVelocity.x, stompBounceForce);

        if (shooter != null)
            shooter.Die();
        else
            Destroy(transform.root.gameObject, 0.15f);
    }
}