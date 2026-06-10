using UnityEngine;

public class HeadHit : MonoBehaviour
{
    [Header("Head Hit")]
    [SerializeField] private Transform headPoint;
    [SerializeField] private float checkDistance = 0.15f;
    [SerializeField] private LayerMask blockMask;
    [SerializeField] private float minUpVelocity = 0.2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rb == null || headPoint == null) return;
        if (rb.linearVelocityY < minUpVelocity) return;

        RaycastHit2D hit = Physics2D.Raycast(headPoint.position, Vector2.up, checkDistance, blockMask);
        if (!hit) return;

        var tileSystem = hit.collider.GetComponentInParent<TilemapBlockSystem>();
        if (tileSystem != null)
        {
            tileSystem.TryHitAtWorldPoint(hit.point);
            return;
        }

        var hittable = hit.collider.GetComponent<IHeadHittable>();
        if (hittable != null)
        {
            hittable.OnHeadHit(this);
        }

        if (hit)
        {
            Debug.Log("Pegó a: " + hit.collider.name);
        }
    }
}

public interface IHeadHittable
{
    void OnHeadHit(HeadHit hitter);
}