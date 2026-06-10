using UnityEngine;

public class MarioSize : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject smallVisual;
    [SerializeField] private GameObject bigVisual;

    [Header("Colliders")]
    [SerializeField] private CapsuleCollider2D smallCollider;
    [SerializeField] private CapsuleCollider2D bigCollider;

    [Header("Head Hit")]
    [SerializeField] private Transform headHit;
    [SerializeField] private float smallHeadY = 0.5f;
    [SerializeField] private float bigHeadY = 2.5f;

    public bool IsBig { get; private set; }

    private void Awake()
    {
        ApplyState(false);
    }

    public void Grow()
    {
        if (IsBig) return;

        ApplyState(true);
    }

    public void Shrink()
    {
        ApplyState(false);
    }

    private void ApplyState(bool big)
    {
        IsBig = big;

        if (smallVisual != null)
            smallVisual.SetActive(!big);

        if (bigVisual != null)
            bigVisual.SetActive(big);

        if (smallCollider != null)
            smallCollider.enabled = !big;

        if (bigCollider != null)
            bigCollider.enabled = big;

        if (headHit != null)
        {
            Vector3 pos = headHit.localPosition;
            pos.y = big ? bigHeadY : smallHeadY;
            headHit.localPosition = pos;
        }
    }
}