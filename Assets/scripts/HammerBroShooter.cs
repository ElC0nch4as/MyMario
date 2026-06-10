using System.Collections;
using UnityEngine;

public class HammerBroShooter : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject hammerPrefab;
    [SerializeField] private Transform mario;

    [Header("Throw")]
    [SerializeField] private float throwInterval = 1.2f;
    [SerializeField] private float throwXForce = 5f;
    [SerializeField] private float throwYForce = 8f;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Death")]
    [SerializeField] private GameObject deathPrefab;
    [SerializeField] private float deathPrefabLifeTime = 0.6f;

    private bool active;
    private bool dead;
    private Coroutine shootRoutine;

    private void Awake()
    {
        if (mario == null)
        {
            Mario marioScript = FindFirstObjectByType<Mario>();
            if (marioScript != null)
                mario = marioScript.transform;
        }

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (active && !dead)
            LookAtMario();
    }

    public void Activate()
    {
        if (active || dead) return;

        active = true;
        LookAtMario();
        shootRoutine = StartCoroutine(ShootLoop());
    }

    public void Deactivate()
    {
        active = false;

        if (shootRoutine != null)
        {
            StopCoroutine(shootRoutine);
            shootRoutine = null;
        }
    }

    private IEnumerator ShootLoop()
    {
        while (active && !dead)
        {
            ThrowHammer();
            yield return new WaitForSeconds(throwInterval);
        }
    }

    private void LookAtMario()
    {
        if (mario == null || spriteRenderer == null) return;

        bool marioIsRight = mario.position.x > transform.position.x;

        spriteRenderer.flipX = marioIsRight;
    }

    private void ThrowHammer()
    {
        if (hammerPrefab == null || throwPoint == null || mario == null) return;

        float dir = mario.position.x > transform.position.x ? 1f : -1f;

        LookAtMario();

        GameObject hammer = Instantiate(hammerPrefab, throwPoint.position, Quaternion.identity);

        Rigidbody2D rb = hammer.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        rb.linearVelocity = new Vector2(dir * throwXForce, throwYForce);
    }

    public void Die()
    {
        if (dead) return;

        dead = true;
        Deactivate();

        Collider2D[] cols = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in cols)
            col.enabled = false;

        if (deathPrefab != null)
        {
            GameObject flat = Instantiate(deathPrefab, transform.position, transform.rotation);

            SpriteRenderer flatSr = flat.GetComponentInChildren<SpriteRenderer>();
            if (flatSr != null && spriteRenderer != null)
                flatSr.flipX = spriteRenderer.flipX;

            Destroy(flat, deathPrefabLifeTime);
        }

        Destroy(gameObject);
    }
}