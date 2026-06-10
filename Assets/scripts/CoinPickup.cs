using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour
{
    [Header("Pop")]
    [SerializeField] private float popHeight = 0.8f;
    [SerializeField] private float popTime = 0.12f;
    [SerializeField] private float lifeAfterPop = 0.05f;

    private bool _collected;

    private void OnEnable()
    {
        StartCoroutine(PopAndDie());
    }

    private IEnumerator PopAndDie()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.up * popHeight;

        float t = 0f;
        while (t < 1f)
        {
            if (_collected) yield break;

            t += Time.deltaTime / Mathf.Max(0.0001f, popTime);
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        if (lifeAfterPop > 0f)
            yield return new WaitForSeconds(lifeAfterPop);

        if (!_collected)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_collected) return;
        if (!other.CompareTag("Player")) return;

        _collected = true;
        Destroy(gameObject);
    }
}