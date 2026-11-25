using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioClip checkpointSound;

    private bool activated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated || !collision.CompareTag("Player")) return;

        activated = true;

        PlayerCollect collect = collision.GetComponent<PlayerCollect>();
        if (collect != null)
            CheckpointManager.SetCheckpoint(transform.position, collect.Coins, collect.CollectedIds);
        else
            CheckpointManager.SetCheckpoint(transform.position, 0, new List<string>());

        if (checkpointSound != null)
            AudioManager.Instance.PlaySound(checkpointSound);
    }
}