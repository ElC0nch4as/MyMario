using UnityEngine;

public class HammerDelBro : MonoBehaviour
{
    [SerializeField] private float lifeTime = 4f;
    [SerializeField] private float rotationSpeed = 360f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        VIdaMario marioHealth = other.GetComponentInParent<VIdaMario>();

        if (marioHealth != null)
        {
            marioHealth.TakeDamage();
            Destroy(gameObject);
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Solid"))
        {
            Destroy(gameObject);
        }
    }
}