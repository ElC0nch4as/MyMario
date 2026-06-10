using UnityEngine;
using UnityEngine.Windows;

public class MarioyFlag : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private MarioSize marioSize;

    [Header("Small Mario")]
    [SerializeField] private SpriteRenderer smallRenderer;
    [SerializeField] private Animator smallAnimator;
    [SerializeField] private Sprite smallFlagSprite;

    [Header("Big Mario")]
    [SerializeField] private SpriteRenderer bigRenderer;
    [SerializeField] private Animator bigAnimator;
    [SerializeField] private Sprite bigFlagSprite;

    public void SetFlagPose()
    {
        bool isBig = marioSize != null && marioSize.IsBig;

        if (smallAnimator != null) smallAnimator.enabled = false;
        if (bigAnimator != null) bigAnimator.enabled = false;

        if (!isBig)
        {
            if (smallRenderer != null && smallFlagSprite != null)
                smallRenderer.sprite = smallFlagSprite;
        }
        else
        {
            if (bigRenderer != null && bigFlagSprite != null)
                bigRenderer.sprite = bigFlagSprite;
        }
    }

    public void RestoreAnimators()
    {
        if (smallAnimator != null) smallAnimator.enabled = true;
        if (bigAnimator != null) bigAnimator.enabled = true;
    }

    public void FaceRight()
    {
        if (smallRenderer != null) smallRenderer.flipX = false;
        if (bigRenderer != null) bigRenderer.flipX = false;
    }
}