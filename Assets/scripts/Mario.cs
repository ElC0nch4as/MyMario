using UnityEngine;
using UnityEngine.VFX;

public class Mario : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float maxMoveSpeed = 1f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float deceleration = 1f;

    [Header("BetterJump")]
    [SerializeField] private float minJumpVelocity = 1f;
    [SerializeField] private float maxJumpVelocity = 1f;
    [SerializeField] private float jumpHoldAcceleration = 1f;
    [SerializeField] private float maxJumpHoldTime = 1f;
    [SerializeField] private float jumpCutVelocity = 1f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 1f;
    [SerializeField] private LayerMask groundMask;

    [Header("Animation")]
    [SerializeField] private Animator smallAnimator;
    [SerializeField] private Animator bigAnimator;

    [SerializeField] private SpriteRenderer smallSpriteRenderer;
    [SerializeField] private SpriteRenderer bigSpriteRenderer;

    private Rigidbody2D rb;

    private bool controlsEnabled = true;
    private float inputX;

    private bool isJumping;
    private float jumpHoldTimer;

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
            UpdateAnimation();
            return;
        }

        inputX = Input.GetAxisRaw("Horizontal");

        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);
        bool jumpHeld = Input.GetKey(KeyCode.Space);
        bool jumpReleased = Input.GetKeyUp(KeyCode.Space);

        if (jumpPressed && IsGrounded())
        {
            StartJump();
        }

        if (jumpHeld && isJumping)
        {
            HoldJump();
        }

        if (jumpReleased)
        {
            CutJump();
        }

        if (rb.linearVelocity.y <= 0f)
        {
            isJumping = false;
        }

        UpdateAnimation();
        FlipMario();
    }

    private void FixedUpdate()
    {
        if (!controlsEnabled) return;

        ApplyMovement();
    }

    private void ApplyMovement()
    {
        float targetSpeed = inputX * maxMoveSpeed;
        float currentSpeed = rb.linearVelocity.x;

        float accelRate;

        if (Mathf.Abs(inputX) > 0.01f)
            accelRate = acceleration;
        else
            accelRate = deceleration;

        float newSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            accelRate * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector2(newSpeed, rb.linearVelocity.y);
    }

    private void StartJump()
    {
        isJumping = true;
        jumpHoldTimer = maxJumpHoldTime;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, minJumpVelocity);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayJump();
    }

    private void HoldJump()
    {
        if (jumpHoldTimer <= 0f)
        {
            isJumping = false;
            return;
        }

        float newY = rb.linearVelocity.y + jumpHoldAcceleration * Time.deltaTime;

        if (newY > maxJumpVelocity)
            newY = maxJumpVelocity;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, newY);

        jumpHoldTimer -= Time.deltaTime;
    }

    private void CutJump()
    {
        isJumping = false;

        if (rb.linearVelocity.y > jumpCutVelocity)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpCutVelocity);
        }
    }

    private void UpdateAnimation()
    {
        bool grounded = IsGrounded();
        float speed = Mathf.Abs(rb.linearVelocity.x);

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

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;

        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundMask
        );
    }

    public void SetControlsEnabled(bool enabled)
    {
        controlsEnabled = enabled;

        if (!controlsEnabled)
        {
            inputX = 0f;
            isJumping = false;

            if (rb != null)
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

    public void StopMarioCompletely()
    {
        inputX = 0f;
        isJumping = false;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }
}