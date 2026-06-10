using UnityEngine;
using UnityEngine.VFX;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip flagClip;
    [SerializeField] private AudioClip fungusBlockClip;
    [SerializeField] private AudioClip goombaFlatClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip powerUpClip;
    [SerializeField] private AudioClip winClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Play(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip);
    }

    public void PlayCoin()
    {
        Play(coinClip);
    }

    public void PlayFlag()
    {
        Play(flagClip);
    }

    public void PlayFungusBlock()
    {
        Play(fungusBlockClip);
    }

    public void PlayGoombaFlat()
    {
        Play(goombaFlatClip);
    }

    public void PlayJump()
    {
        Play(jumpClip);
    }

    public void PlayLose()
    {
        Play(loseClip);
    }

    public void PlayPowerUp()
    {
        Play(powerUpClip);
    }

    public void PlayWin()
    {
        Play(winClip);
    }
}