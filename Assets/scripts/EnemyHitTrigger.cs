using UnityEngine;
using UnityEngine.VFX;

public class EnemyHitTrigger : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private EnemyWalker enemy;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private Animator animator;

    [Header("Death")]
    [SerializeField] private Sprite deadSprite;
    [SerializeField] private float destroyDelay = 0.6f;

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

        if (animator == null)
            animator = GetComponentInParent<Animator>();
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

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayGoombaFlat();

        if (enemy != null)
            enemy.Die();

        marioRb.linearVelocity = new Vector2(marioRb.linearVelocity.x, stompBounceForce);

        if (animator != null)
            animator.enabled = false;

        if (visual != null && deadSprite != null)
            visual.sprite = deadSprite;

        if (enemyCollider != null)
            enemyCollider.enabled = false;

        Collider2D myTrigger = GetComponent<Collider2D>();
        if (myTrigger != null)
            myTrigger.enabled = false;

        Destroy(transform.root.gameObject, destroyDelay);
    }
}