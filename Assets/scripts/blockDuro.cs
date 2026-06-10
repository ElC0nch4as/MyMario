using System.Collections;
using UnityEngine;

public class SolidBlock : MonoBehaviour, IHeadHittable
{
    [Header("Block State")]
    [SerializeField] private Sprite blockedSprite;
    [SerializeField] private bool lockAfterFirstHit = true;

    [Header("Bump FX")]
    [SerializeField] private float bumpHeight = 0.12f;
    [SerializeField] private float bumpTime = 0.08f;

    private bool _used;
    private SpriteRenderer _sr;
    private Coroutine _bump;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void OnHeadHit(HeadHit hitter)
    {
        if (_used) return;

        if (_bump != null) StopCoroutine(_bump);
        _bump = StartCoroutine(Bump());

        if (lockAfterFirstHit)
        {
            _used = true;

            if (_sr != null && blockedSprite != null)
                _sr.sprite = blockedSprite;
        }
    }

    private IEnumerator Bump()
    {
        Vector3 start = transform.localPosition;
        Vector3 up = start + Vector3.up * bumpHeight;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, bumpTime);
            transform.localPosition = Vector3.Lerp(start, up, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, bumpTime);
            transform.localPosition = Vector3.Lerp(up, start, t);
            yield return null;
        }

        transform.localPosition = start;
        _bump = null;
    }
}