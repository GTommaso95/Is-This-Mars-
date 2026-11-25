using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    public AudioClip flagSound;

    private bool reached;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (reached || !collision.CompareTag("Player")) return;

        PlayerCollect collect = collision.GetComponent<PlayerCollect>();

        if (collect != null && collect.Coins >= 10)
        {
            reached = true;
            AudioManager.Instance.PlaySound(flagSound);
            Invoke("LoadNextLevel", 3.5f);
        }
            
    }

    void LoadNextLevel()
    {
        CheckpointManager.Reset();

        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
