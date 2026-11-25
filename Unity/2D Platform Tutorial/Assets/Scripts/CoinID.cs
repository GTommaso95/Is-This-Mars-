using UnityEngine;

public class CoinID : MonoBehaviour
{
    public string Id { get; private set; }

    void Awake()
    {
        Id = transform.position.x.ToString("F2") + "_" + transform.position.y.ToString("F2");
    }

    void Start()
    {
        // Se questa moneta era già stata presa prima dell'ultimo checkpoint, sparisce
        if (CheckpointManager.IsCollected(Id))
            Destroy(gameObject);
    }
}
