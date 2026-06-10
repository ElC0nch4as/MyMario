using UnityEngine;

public class HammerBroDetection : MonoBehaviour
{
    [SerializeField] private HammerBroShooter shooter;

    private void Awake()
    {
        if (shooter == null)
            shooter = GetComponentInParent<HammerBroShooter>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Mario mario = other.GetComponentInParent<Mario>();
        if (mario == null) return;

        shooter.Activate();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Mario mario = other.GetComponentInParent<Mario>();
        if (mario == null) return;

        shooter.Deactivate();
    }
}