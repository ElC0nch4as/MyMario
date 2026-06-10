using System.Collections;
using UnityEngine;

public class BreakableBlock : MonoBehaviour, IHeadHittable
{
    [Header("Rules")]
    [SerializeField] private bool requiresBigMario = true;

    [Header("Optional FX")]
    [SerializeField] private float bumpHeight = 0.10f;
    [SerializeField] private float bumpTime = 0.06f;

    private Coroutine _bump;
    private bool _broken;

    public void OnHeadHit(HeadHit hitter)
    {
        if (_broken) return;

        if (requiresBigMario)
        {
            var size = hitter.GetComponent<MarioSize>();
            if (size == null || !size.IsBig)
            {
                DoBump();
                return;
            }
        }

        _broken = true;
        Destroy(gameObject);
    }

    private void DoBump()
    {
        if (_bump != null) StopCoroutine(_bump);
        _bump = StartCoroutine(Bump());
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