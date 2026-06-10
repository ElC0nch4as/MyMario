using UnityEngine;

public class HeadHit : MonoBehaviour
{
    [Header("Head Hit")]
    [SerializeField] private Transform headPoint;
    [SerializeField] private float checkDistance = 0.25f;
    [SerializeField] private LayerMask blockMask;
    [SerializeField] private float minUpVelocity = 0.2f;

    private Rigidbody2D rb;
    private Collider2D lastBlockHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rb == null || headPoint == null) return;

        RaycastHit2D hit = Physics2D.Raycast(headPoint.position, Vector2.up, checkDistance, blockMask);

        if (!hit)
        {
            lastBlockHit = null;
            return;
        }

        if (rb.linearVelocity.y < minUpVelocity) return;

        if (hit.collider == lastBlockHit) return;

        lastBlockHit = hit.collider;

        IHeadHittable hittable = hit.collider.GetComponent<IHeadHittable>();
        if (hittable != null)
        {
            hittable.OnHeadHit(this);
        }
    }
}

public interface IHeadHittable
{
    void OnHeadHit(HeadHit hitter);
}