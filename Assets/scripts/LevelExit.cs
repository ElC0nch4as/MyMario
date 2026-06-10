using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float delay = 1.0f;

    [Header("Next Scene")]
    [SerializeField] private bool reloadScene = false;
    [SerializeField] private string nextSceneName = "SampleScene";

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;

        triggered = true;
        StartCoroutine(ExitRoutine(other.gameObject));
    }

    private IEnumerator ExitRoutine(GameObject player)
    {
        var mario = player.GetComponent<Mario>();
        if (mario != null) mario.enabled = false;

        yield return new WaitForSeconds(delay);

        if (reloadScene)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}