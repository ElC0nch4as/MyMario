using System.Collections;
using UnityEngine;

public class CoinBlock : MonoBehaviour, IHeadHittable
{
    [Header("Coin")]
    [SerializeField] private int coins = 3;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("After used")]
    [SerializeField] private Sprite usedSprite;

    [Header("Bump FX")]
    [SerializeField] private float bumpHeight = 0.12f;
    [SerializeField] private float bumpTime = 0.08f;

    private int _left;
    private bool _used;
    private SpriteRenderer _sr;
    private Coroutine _bump;

    private void Awake()
    {
        _left = coins;
        _sr = GetComponent<SpriteRenderer>();
    }

    public void OnHeadHit(HeadHit hitter)
    {
        if (_used) return;

        DoBump();

        if (_left <= 0) return;

        _left--;
        SpawnCoin();

        if (_left <= 0)
            SetUsed();
    }

    private void SpawnCoin()
    {
        if (coinPrefab == null) return;
        Vector3 p = (spawnPoint != null) ? spawnPoint.position : transform.position + Vector3.up * 0.8f;
        Instantiate(coinPrefab, p, Quaternion.identity);
    }

    private void SetUsed()
    {
        _used = true;
        if (_sr != null && usedSprite != null)
            _sr.sprite = usedSprite;
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