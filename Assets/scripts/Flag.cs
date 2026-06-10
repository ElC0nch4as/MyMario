using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class Flag : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform slideBottom;
    [SerializeField] private Transform castleTarget;

    [Header("Movement")]
    [SerializeField] private float slideSpeed = 3f;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float leavePoleDistance = 0.8f;

    [Header("End")]
    [SerializeField] private float disappearDelay = 0.2f;

    private bool activated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        Mario mario = other.GetComponentInParent<Mario>();
        if (mario == null) return;

        if (slideBottom == null || castleTarget == null)
        {
            Debug.LogError("Falta SlideBottom o CastleTarget en la bandera.");
            return;
        }

        activated = true;

        Collider2D myCol = GetComponent<Collider2D>();
        if (myCol != null)
            myCol.enabled = false;

        StartCoroutine(FlagSequence(mario));
    }

    private IEnumerator FlagSequence(Mario mario)
    {
        Rigidbody2D rb = mario.GetComponent<Rigidbody2D>();
        MarioyFlag flagVisual = mario.GetComponent<MarioyFlag>();

        float originalGravity = 3f;

        mario.SetControlsEnabled(false);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayFlag();

        mario.StopMarioCompletely();

        if (rb != null)
        {
            originalGravity = rb.gravityScale;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.simulated = false;
        }

        if (flagVisual != null)
            flagVisual.SetFlagPose();

        Vector3 pos = mario.transform.position;
        pos.x = transform.position.x;
        mario.transform.position = pos;

        while (mario.transform.position.y > slideBottom.position.y)
        {
            pos = mario.transform.position;
            pos.y = Mathf.MoveTowards(pos.y, slideBottom.position.y, slideSpeed * Time.deltaTime);
            mario.transform.position = pos;

            yield return null;
        }

        pos = mario.transform.position;
        pos.y = slideBottom.position.y;
        mario.transform.position = pos;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayWin();

        yield return new WaitForSeconds(0.15f);

        if (flagVisual != null)
        {
            flagVisual.RestoreAnimators();
            flagVisual.FaceRight();
        }

        float leaveX = mario.transform.position.x + leavePoleDistance;

        while (mario.transform.position.x < leaveX)
        {
            pos = mario.transform.position;
            pos.x = Mathf.MoveTowards(pos.x, leaveX, walkSpeed * Time.deltaTime);
            mario.transform.position = pos;

            yield return null;
        }

        if (rb != null)
        {
            rb.simulated = true;
            rb.gravityScale = originalGravity;
            rb.linearVelocity = new Vector2(walkSpeed, rb.linearVelocity.y);
        }

        while (mario.transform.position.x < castleTarget.position.x)
        {
            if (rb != null)
                rb.linearVelocity = new Vector2(walkSpeed, rb.linearVelocity.y);

            yield return null;
        }

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        mario.gameObject.SetActive(false);

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}