using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollect : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public int healAmount = 1;

    public AudioClip takeCoin;
    public AudioClip takeLife;

    private int coins;
    public int Coins => coins;

    private HashSet<string> collectedIds = new HashSet<string>();
    public IReadOnlyCollection<string> CollectedIds => collectedIds;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        if (CheckpointManager.HasCheckpoint)
        {
            coins = CheckpointManager.Coins;
            collectedIds = new HashSet<string>(CheckpointManager.CollectedIds);
        }

        UpdateCoinText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            CollectCoins(collision.gameObject);
        }
        else if (collision.CompareTag("Health"))
        {
            CollectHealth(collision.gameObject);
        }
    }

    void CollectCoins(GameObject item)
    {
        AudioManager.Instance.PlaySound(takeCoin);
        coins++;

        CoinID coinId = item.GetComponent<CoinID>();
        if (coinId != null)
            collectedIds.Add(coinId.Id);

        UpdateCoinText();
        Destroy(item);
    }

    void CollectHealth(GameObject item)
    {
        AudioManager.Instance.PlaySound(takeLife);
        playerHealth.Heal(healAmount);
        Destroy(item);
    }

    void UpdateCoinText()
    {
        coinText.text = coins.ToString() + "/10";
    }
}
