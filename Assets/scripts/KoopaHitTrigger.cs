using UnityEngine;

public class KoopaHitTrigger : MonoBehaviour
{
    private enum KoopaState
    {
        Walking,
        ShellIdle,
        ShellMoving
    }

    [Header("Refs")]
    [SerializeField] private EnemyWalker enemyWalker;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private Animator animator;

    [Header("Sprites")]
    [SerializeField] private Sprite shellSprite;

    [Header("Stomp")]
    [SerializeField] private float stompBounceForce = 10f;
    [SerializeField] private float stompTolerance = 0.2f;

    [Header("Shell Move")]
    [SerializeField] private float shellSpeed = 8f;
    [SerializeField] private LayerMask solidMask;
    [SerializeField] private float wallCheckDistance = 0.12f;
    [SerializeField] private float sideOffset = 0.45f;

    private KoopaState state = KoopaState.Walking;
    private int shellDir = 1;

    private void Awake()
    {
        if (enemyWalker == null)
            enemyWalker = GetComponentInParent<EnemyWalker>();

        if (rb == null)
            rb = GetComponentInParent<Rigidbody2D>();

        if (bodyCollider == null)
            bodyCollider = GetComponentInParent<Collider2D>();

        if (visual == null)
            visual = GetComponentInParent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponentInParent<Animator>();
    }

    private void FixedUpdate()
    {
        if (state != KoopaState.ShellMoving) return;

        rb.linearVelocity = new Vector2(shellDir * shellSpeed, rb.linearVelocity.y);

        Vector2 origin = (Vector2)transform.root.position + Vector2.right * (shellDir * sideOffset);
        Vector2 castDir = shellDir > 0 ? Vector2.right : Vector2.left;

        bool hitWall = Physics2D.Raycast(origin, castDir, wallCheckDistance, solidMask);

        if (hitWall)
        {
            shellDir *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root == transform.root) return;

        ShellTarrjet enemyTarget = other.GetComponentInParent<ShellTarrjet>();

        if (enemyTarget != null)
        {
            HandleEnemyInteraction(enemyTarget);
            return;
        }

        VIdaMario marioHealth = other.GetComponentInParent<VIdaMario>();
        if (marioHealth == null) return;

        Rigidbody2D marioRb = marioHealth.GetComponent<Rigidbody2D>();
        if (marioRb == null) return;

        bool marioIsFalling = marioRb.linearVelocity.y <= 0f;
        bool marioIsAbove = other.bounds.min.y > bodyCollider.bounds.max.y - stompTolerance;

        if (state == KoopaState.Walking)
        {
            if (marioIsFalling && marioIsAbove)
            {
                TurnIntoShell(marioRb);
            }
            else
            {
                marioHealth.TakeDamage();
            }

            return;
        }

        if (state == KoopaState.ShellIdle)
        {
            KickShell(marioRb);
            return;
        }

        if (state == KoopaState.ShellMoving)
        {
            if (marioIsFalling && marioIsAbove)
            {
                StopShell(marioRb);
            }
            else
            {
                marioHealth.TakeDamage();
            }
        }
    }

    private void HandleEnemyInteraction(ShellTarrjet enemyTarget)
    {
        if (state == KoopaState.ShellMoving)
        {
            enemyTarget.KillByShell();
            return;
        }

        if (state == KoopaState.ShellIdle)
        {
            int dir = enemyTarget.GetMoveDirection(transform.root.position.x);
            StartMovingShell(dir);
            return;
        }
    }

    private void TurnIntoShell(Rigidbody2D marioRb)
    {
        state = KoopaState.ShellIdle;

        if (enemyWalker != null)
            enemyWalker.enabled = false;

        rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.enabled = false;

        if (visual != null && shellSprite != null)
            visual.sprite = shellSprite;

        marioRb.linearVelocity = new Vector2(marioRb.linearVelocity.x, stompBounceForce);
    }

    private void KickShell(Rigidbody2D marioRb)
    {
        float marioDirection = marioRb.linearVelocity.x;

        int dir;

        if (Mathf.Abs(marioDirection) > 0.1f)
        {
            dir = marioDirection > 0 ? 1 : -1;
        }
        else
        {
            dir = marioRb.transform.position.x < transform.root.position.x ? 1 : -1;
        }

        StartMovingShell(dir);
    }

    private void StartMovingShell(int dir)
    {
        state = KoopaState.ShellMoving;

        shellDir = dir;

        if (shellDir == 0)
            shellDir = 1;

        rb.linearVelocity = new Vector2(shellDir * shellSpeed, rb.linearVelocity.y);
    }

    private void StopShell(Rigidbody2D marioRb)
    {
        state = KoopaState.ShellIdle;

        rb.linearVelocity = Vector2.zero;

        marioRb.linearVelocity = new Vector2(marioRb.linearVelocity.x, stompBounceForce);
    }
}