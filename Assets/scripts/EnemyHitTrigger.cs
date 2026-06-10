using UnityEngine;

public class EnemyHitTrigger : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private EnemyWalker enemy;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private SpriteRenderer visual;

    [Header("Death")]
    [SerializeField] private Sprite deadSprite;
    [SerializeField] private float destroyDelay = 0.25f;

    [Header("Stomp")]
    [SerializeField] private float stompBounceForce = 10f;
    [SerializeField] private float stompTolerance = 0.15f;

    private bool dead;

    private void Awake()
    {
        if (enemy == null)
            enemy = GetComponentInParent<EnemyWalker>();

        if (enemyCollider == null)
            enemyCollider = GetComponentInParent<Collider2D>();

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
        bool marioIsAbove = other.bounds.min.y > enemyCollider.bounds.max.y - stompTolerance;

        if (marioIsFalling && marioIsAbove)
        {
            StompEnemy(marioRb);
        }
        else
        {
            marioHealth.TakeDamage();
        }
    }

    private void StompEnemy(Rigidbody2D marioRb)
    {
        dead = true;

        if (enemy != null)
            enemy.Die();

        marioRb.linearVelocity = new Vector2(marioRb.linearVelocity.x, stompBounceForce);

        if (visual != null && deadSprite != null)
            visual.sprite = deadSprite;

        Destroy(transform.root.gameObject, destroyDelay);
    }
}