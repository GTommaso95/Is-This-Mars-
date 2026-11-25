using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Item Settings")]
    public float interactRadius = 1.1f;
    public KeyCode interactKey = KeyCode.E;
    public AudioClip wrongKeySound;
    public AudioClip rightKeySound;
    public Vector2 floatOffset = new Vector2(-0.5f, 0.8f); // posizione rispetto al player
    public float followSmoothTime = 0.15f;                 // più alto = più "in ritardo"
    public float bobAmplitude = 0.05f;                     // altezza dell'ondeggiamento
    public float bobSpeed = 3f;                            // velocità dell'ondeggiamento

    private Vector3 keyVelocity; // serve a SmoothDamp
    private GameObject heldItem;

    [Header("Grab Settings")]
    public float grabRange = 1.5f;
    public float moveSpeed = 1.5f;

    private Transform grabbedObject;
    private bool isGrabbing = false;
    private Rigidbody2D grabbedRb;

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

    void FixedUpdate()
    {
        if (!isGrabbing || grabbedRb == null)
            return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        grabbedRb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            grabbedRb.linearVelocity.y
        );
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (heldItem == null)
                TryPickUp();
            else
                UseOrDrop();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Grab();
        }

        MoveGrabbedObject();
    }

    void TryPickUp()
    {
        GameObject nearest = FindNearest("RightKey", "WrongKey");
        if (nearest == null) return;

        heldItem = nearest;
        keyVelocity = Vector3.zero;

        Rigidbody2D rb = heldItem.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        Collider2D col = heldItem.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    void LateUpdate()
    {
        if (heldItem == null) return;

        Vector3 target = transform.position + (Vector3)floatOffset;
        target.y = target.y + Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        target.z = heldItem.transform.position.z;

        heldItem.transform.position = Vector3.SmoothDamp(
            heldItem.transform.position,
            target,
            ref keyVelocity,
            followSmoothTime
        );
    }

    GameObject FindNearest(params string[] tags)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius);
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == heldItem) continue;

            bool match = false;
            foreach (string t in tags)
            {
                if (hit.CompareTag(t)) { match = true; break; }
            }
            if (!match) continue;

            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = hit.gameObject;
            }
        }

        return nearest;
    }

    void Drop()
    {
        heldItem.transform.position = transform.position;

        Collider2D col = heldItem.GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        Rigidbody2D rb = heldItem.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = true;

        heldItem = null;
    }

    void UseOrDrop()
    {
        GameObject door = FindNearest("Door");

        if (door != null)
        {
            if (heldItem.CompareTag("RightKey"))
            {
                if (rightKeySound != null)
                    AudioManager.Instance.PlaySound(rightKeySound);

                Destroy(door);
                Destroy(heldItem);
                heldItem = null;
            }
            else if (wrongKeySound != null)
            {
                AudioManager.Instance.PlaySound(wrongKeySound);
            }
            return;
        }

        Drop();
    }

    void Grab()
    {
        // Se stiamo già afferrando un oggetto, lo rilasciamo
        if (isGrabbing)
        {
            grabbedRb.linearVelocity = Vector2.zero;

            grabbedRb.bodyType = RigidbodyType2D.Static;

            grabbedObject = null;
            grabbedRb = null;
            isGrabbing = false;
            return;
        }

        // Cerca gli oggetti vicini
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, grabRange);

        float closestDistance = Mathf.Infinity;
        Rigidbody2D closestBox = null;

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Box"))
            {
                float distance = Vector2.Distance(
                    transform.position,
                    col.transform.position
                );

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBox = col.GetComponent<Rigidbody2D>();
                }
            }
        }

        // Se abbiamo trovato una Box, la afferriamo
        if (closestBox != null)
        {
            grabbedRb = closestBox;
            grabbedObject = closestBox.transform;

            grabbedRb.bodyType = RigidbodyType2D.Dynamic;

            isGrabbing = true;
        }
    }

    void MoveGrabbedObject()
    {
        if (!isGrabbing || grabbedObject == null)
            return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector3 movement = new Vector3(
            horizontalInput * moveSpeed * Time.deltaTime,
            0f,
            0f
        );

        grabbedObject.position += movement;
    }
}
