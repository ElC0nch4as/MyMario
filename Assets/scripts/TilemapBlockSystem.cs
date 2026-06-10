using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBlockSystem : MonoBehaviour
{
    [Header("Tilemap")]
    [SerializeField] private Tilemap interactTilemap;

    [Header("Tiles (asigna tus Tile assets)")]
    [SerializeField] private TileBase coinBlockTile;      // bloque que da N monedas
    [SerializeField] private TileBase breakBlockTile;     // bloque rompible
    [SerializeField] private TileBase questionBlockTile;  // bloque ? (hongo o monedas)
    [SerializeField] private TileBase solidBlockTile;     // bloque que solo se bloquea (no rompe)
    [SerializeField] private TileBase usedBlockTile;      // bloque “apagado” / usado

    [Header("Spawns (prefabs opcionales)")]
    [SerializeField] private GameObject coinPrefab;       // opcional (solo visual)
    [SerializeField] private GameObject mushroomPrefab;   // hongo

    [Header("Rules")]
    [SerializeField] private int coinsFromCoinBlock = 3;
    [SerializeField] private int coinsFromQuestionBlock = 1;
    [SerializeField] private bool questionGivesMushroom = true;

    [Header("Popup (subida del item)")]
    [SerializeField] private float popUpDistance = 0.8f;
    [SerializeField] private float popUpTime = 0.12f;

    private void Reset()
    {
        interactTilemap = GetComponent<Tilemap>();
    }

    private void Awake()
    {
        if (interactTilemap == null)
            interactTilemap = GetComponent<Tilemap>();
    }

    /// <summary>
    /// Llamar cuando Mario golpea "con la cabeza". worldHitPoint = punto del raycast.
    /// </summary>
    public bool TryHitAtWorldPoint(Vector2 worldHitPoint)
    {
        if (interactTilemap == null) return false;

        Vector3Int cell = interactTilemap.WorldToCell(worldHitPoint);
        TileBase hitTile = interactTilemap.GetTile(cell);
        if (hitTile == null) return false;

        // Centro del tile para spawns
        Vector3 tileCenter = interactTilemap.GetCellCenterWorld(cell);

        if (hitTile == breakBlockTile)
        {
            // Rompible: desaparece
            interactTilemap.SetTile(cell, null);
            return true;
        }

        if (hitTile == coinBlockTile)
        {
            SpawnCoins(tileCenter, coinsFromCoinBlock);
            SetUsed(cell);
            return true;
        }

        if (hitTile == questionBlockTile)
        {
            if (questionGivesMushroom && mushroomPrefab != null)
            {
                SpawnPopup(mushroomPrefab, tileCenter);
            }
            else
            {
                SpawnCoins(tileCenter, coinsFromQuestionBlock);
            }

            SetUsed(cell);
            return true;
        }

        if (hitTile == solidBlockTile)
        {
            // No da nada, pero se “bloquea”/se cambia a usado (si quieres)
            SetUsed(cell);
            return true;
        }

        // Si es otro tile que no reconocemos, no hacemos nada
        return false;
    }

    private void SetUsed(Vector3Int cell)
    {
        if (usedBlockTile != null)
            interactTilemap.SetTile(cell, usedBlockTile);
    }

    private void SpawnCoins(Vector3 tileCenter, int amount)
    {
        // Si no quieres prefabs, aquí podrías solo sumar contador.
        if (coinPrefab == null) return;

        for (int i = 0; i < amount; i++)
        {
            SpawnPopup(coinPrefab, tileCenter);
        }
    }

    private void SpawnPopup(GameObject prefab, Vector3 tileCenter)
    {
        GameObject go = Instantiate(prefab, tileCenter, Quaternion.identity);
        StartCoroutine(PopupRoutine(go.transform, tileCenter));
    }

    private System.Collections.IEnumerator PopupRoutine(Transform t, Vector3 startPos)
    {
        Vector3 endPos = startPos + Vector3.up * popUpDistance;
        float elapsed = 0f;

        while (elapsed < popUpTime)
        {
            elapsed += Time.deltaTime;
            float k = Mathf.Clamp01(elapsed / popUpTime);
            t.position = Vector3.Lerp(startPos, endPos, k);
            yield return null;
        }

        t.position = endPos;
    }
}