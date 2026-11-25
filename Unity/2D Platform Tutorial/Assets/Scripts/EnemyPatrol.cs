using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPatrol : MonoBehaviour
{

    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;

    private Vector3 targetPosition;
    private Vector3 posA;
    private Vector3 posB;

    void Start()
    {
        posA = pointA.position;
        posB = pointB.position;
        targetPosition = posA;
    }

    void Update()
    {
        Patrol();
        FlipSprite();
    }

    void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (targetPosition == posA)
                targetPosition = posB;
            else
                targetPosition = posA;
        }
    }

    void FlipSprite()
    {
        if (targetPosition.x > transform.position.x)
            transform.localScale = new Vector3(-7, 7, 1);
        else
            transform.localScale = new Vector3(7, 7, 1);
    }
}
