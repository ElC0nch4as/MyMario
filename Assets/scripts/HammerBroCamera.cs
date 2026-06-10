using UnityEngine;

public class HammerBroCamera : MonoBehaviour
{
    [SerializeField] private HammerBroShooter shooter;

    private void Awake()
    {
        if (shooter == null)
            shooter = GetComponent<HammerBroShooter>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("MainCamera")) return;

        if (shooter != null)
            shooter.Activate();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("MainCamera")) return;

        if (shooter != null)
            shooter.Deactivate();
    }
}