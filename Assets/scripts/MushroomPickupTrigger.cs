using UnityEngine;
using UnityEngine.VFX;

public class MushroomPickupTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        MarioSize size = other.GetComponentInParent<MarioSize>();
        if (size != null)
        {
            size.Grow();

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayPowerUp();
        }

        Destroy(transform.root.gameObject);
    }
}