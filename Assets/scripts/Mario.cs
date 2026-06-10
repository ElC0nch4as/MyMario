using UnityEngine;

public class Mario : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.12f;
    [SerializeField] private LayerMask groundMask;

    [Header("Animation")]
    [SerializeField] private Animator smallAnimator;
    [SerializeField] private Animator bigAnimator;

    [SerializeField] private SpriteRenderer smallSpriteRenderer;
    [SerializeField] private SpriteRenderer bigSpriteRenderer;

    private Rigidbody2D rb;
    private bool controlsEnabled = true;
    private float inputX;

    public bool ControlsEnabled => controlsEnabled;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!controlsEnabled)
        {
            inputX = 0f;
            return;
        }

        inputX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        UpdateAnimation();
        FlipMario();
    }

    private void UpdateAnimation()
    {
        bool grounded = IsGrounded();
        float speed = Mathf.Abs(inputX);

        if (smallAnimator != null)
        {
            smallAnimator.SetFloat("Speed", speed);
            smallAnimator.SetBool("Grounded", grounded);
        }

        if (bigAnimator != null)
        {
            bigAnimator.SetFloat("Speed", speed);
            bigAnimator.SetBool("Grounded", grounded);
        }
    }

    private void FlipMario()
    {
        if (inputX > 0.01f)
        {
            if (smallSpriteRenderer != null)
                smallSpriteRenderer.flipX = false;

            if (bigSpriteRenderer != null)
                bigSpriteRenderer.flipX = false;
        }
        else if (inputX < -0.01f)
        {
            if (smallSpriteRenderer != null)
                smallSpriteRenderer.flipX = true;

            if (bigSpriteRenderer != null)
                bigSpriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        float vx = inputX * moveSpeed;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);
    }

    public void SetControlsEnabled(bool enabled)
    {
        controlsEnabled = enabled;

        if (!controlsEnabled)
        {
            inputX = 0f;

            if (rb != null)
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

    public void ForceWalkRight(float speed)
    {
        controlsEnabled = false;
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
    }
}