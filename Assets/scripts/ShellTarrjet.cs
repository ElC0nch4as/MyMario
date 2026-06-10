using UnityEngine;

public class ShellTarrjet : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private EnemyWalker walker;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private Sprite deadSprite;

    [Header("Death Object Optional")]
    [SerializeField] private GameObject deadObjectPrefab;
    [SerializeField] private float deadObjectLifeTime = 0.6f;

    [Header("Death")]
    [SerializeField] private float destroyDelay = 0.4f;

    private bool dead;

    private void Awake()
    {
        if (walker == null)
            walker = GetComponent<EnemyWalker>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (visual == null)
            visual = GetComponentInChildren<SpriteRenderer>();
    }

    public int GetMoveDirection(float shellX)
    {
        if (walker != null)
            return walker.Direction;

        return transform.position.x < shellX ? 1 : -1;
    }

    public void KillByShell()
    {
        if (dead) return;
        dead = true;

        if (walker != null)
            walker.Die();

        if (animator != null)
            animator.enabled = false;

        Collider2D[] cols = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in cols)
            col.enabled = false;

        if (deadObjectPrefab != null)
        {
            GameObject deadObj = Instantiate(deadObjectPrefab, transform.position, transform.rotation);

            SpriteRenderer deadSr = deadObj.GetComponentInChildren<SpriteRenderer>();

            if (deadSr != null && visual != null)
                deadSr.flipX = visual.flipX;

            if (visual != null)
                visual.gameObject.SetActive(false);

            Destroy(deadObj, deadObjectLifeTime);
        }

        else if (visual != null && deadSprite != null)
        {
            visual.sprite = deadSprite;
        }

        Destroy(gameObject, destroyDelay);
    }
}