using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class VIdaMario : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Mario mario;
    [SerializeField] private MarioSize marioSize;

    [Header("Visuals")]
    [SerializeField] private GameObject smallVisual;
    [SerializeField] private GameObject bigVisual;

    [SerializeField] private SpriteRenderer smallRenderer;
    [SerializeField] private SpriteRenderer bigRenderer;

    [SerializeField] private Animator smallAnimator;
    [SerializeField] private Animator bigAnimator;

    [Header("Death")]
    [SerializeField] private Sprite deathSprite;
    [SerializeField] private float deathJumpHeight = 2.5f;
    [SerializeField] private float deathJumpTime = 0.35f;
    [SerializeField] private float deathFallDistance = 6f;
    [SerializeField] private float deathFallTime = 1.2f;
    [SerializeField] private float restartDelay = 0.5f;

    [Header("Damage")]
    [SerializeField] private float damageCooldown = 2f;
    [SerializeField] private float blinkSpeed = 0.12f;

    [Header("Audio")]
    [SerializeField] private AudioSource mainCameraMusic;

    private bool invincible;
    private bool dead;

    private void Awake()
    {
        if (mario == null)
            mario = GetComponent<Mario>();

        if (marioSize == null)
            marioSize = GetComponent<MarioSize>();
    }

    public void TakeDamage()
    {
        if (dead) return;
        if (invincible) return;

        if (marioSize != null && marioSize.IsBig)
        {
            marioSize.Shrink();
            StartCoroutine(DamageBlink());
        }
        else
        {
            StartCoroutine(DeathRoutine());
        }
    }

    public void DieInstant()
    {
        if (dead) return;
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DamageBlink()
    {
        invincible = true;

        float timer = 0f;

        while (timer < damageCooldown)
        {
            SetAlpha(0.35f);
            yield return new WaitForSeconds(blinkSpeed);

            SetAlpha(1f);
            yield return new WaitForSeconds(blinkSpeed);

            timer += blinkSpeed * 2f;
        }

        SetAlpha(1f);
        invincible = false;
    }

    private void SetAlpha(float alpha)
    {
        if (smallRenderer != null)
        {
            Color c = smallRenderer.color;
            c.a = alpha;
            smallRenderer.color = c;
        }

        if (bigRenderer != null)
        {
            Color c = bigRenderer.color;
            c.a = alpha;
            bigRenderer.color = c;
        }
    }

    private IEnumerator DeathRoutine()
    {
        dead = true;
        invincible = true;

        Time.timeScale = 0f;

        if (mainCameraMusic != null)
            mainCameraMusic.Stop();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayLose();

        if (mario != null)
            mario.SetControlsEnabled(false);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        if (smallAnimator != null)
            smallAnimator.enabled = false;

        if (bigAnimator != null)
            bigAnimator.enabled = false;

        if (smallVisual != null)
            smallVisual.SetActive(true);

        if (bigVisual != null)
            bigVisual.SetActive(false);

        if (smallRenderer != null && deathSprite != null)
        {
            smallRenderer.sprite = deathSprite;
            smallRenderer.color = Color.white;
        }

        Vector3 start = transform.position;
        Vector3 up = start + Vector3.up * deathJumpHeight;
        Vector3 down = start + Vector3.down * deathFallDistance;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / deathJumpTime;
            transform.position = Vector3.Lerp(start, up, t);
            yield return null;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / deathFallTime;
            transform.position = Vector3.Lerp(up, down, t);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(restartDelay);

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}