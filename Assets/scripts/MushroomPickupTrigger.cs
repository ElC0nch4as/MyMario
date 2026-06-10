using UnityEngine;

public class MushroomPickupTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        MarioSize size = other.GetComponentInParent<MarioSize>();
        if (size != null) size.Grow();

        Destroy(transform.root.gameObject);
    }
}