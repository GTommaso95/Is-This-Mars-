using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public float invincibleTime = 1f;
    public float knockBackForce = 10f;

    public int currentHealth;

    public AudioClip damageSfx;

    private float invincibleTimer;
    private PlayerMovement playerMovement;
    private bool isDead = false;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentHealth = maxHealth;

        if (CheckpointManager.HasCheckpoint)
            transform.position = CheckpointManager.Position;
    }

    void Update()
    {
        invincibleTimer = invincibleTimer - Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckForDamage(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckForDamage(collision.gameObject);
    }

    void CheckForDamage(GameObject obj)
    {
        if (obj.layer == LayerMask.NameToLayer("Water") || obj.layer == LayerMask.NameToLayer("Void"))
        {
            Die();
            return;
        }

        if (obj.layer == LayerMask.NameToLayer("Damage"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        if (invincibleTimer > 0) return;

        AudioManager.Instance.PlaySound(damageSfx);
        playerMovement.Jump(knockBackForce);
        currentHealth = currentHealth - amount;
        invincibleTimer = invincibleTime;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Heal(int amount)
    {
        currentHealth = currentHealth + amount;

        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}
