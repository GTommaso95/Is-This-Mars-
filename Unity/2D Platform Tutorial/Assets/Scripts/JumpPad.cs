using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bounceForce = 20f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Bounce(collision.GetComponent<PlayerMovement>());
        }
    }

    void Bounce(PlayerMovement playerMovement)
    {
        playerMovement.Jump(bounceForce);
        animator.Play("JumpPad_Bounce", 0, 0f);
        Invoke("StopAnimation", 1);
    }
    void StopAnimation()
    {
        animator.Play("JumpPad_Idle");
    }

}
