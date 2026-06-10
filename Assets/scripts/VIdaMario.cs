using UnityEngine;
using UnityEngine.SceneManagement;

public class VIdaMario : MonoBehaviour
{
    [SerializeField] private MarioSize marioSize;
    [SerializeField] private float damageCooldown = 1f;

    private float nextDamageTime;

    private void Awake()
    {
        if (marioSize == null)
            marioSize = GetComponent<MarioSize>();
    }

    public void TakeDamage()
    {
        if (Time.time < nextDamageTime) return;
        nextDamageTime = Time.time + damageCooldown;

        if (marioSize != null && marioSize.IsBig)
        {
            marioSize.Shrink();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}