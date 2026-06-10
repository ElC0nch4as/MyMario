using UnityEngine;

public class DeathPIT : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        VIdaMario vida = other.GetComponentInParent<VIdaMario>();

        if (vida != null)
        {
            vida.DieInstant();
        }
    }
}