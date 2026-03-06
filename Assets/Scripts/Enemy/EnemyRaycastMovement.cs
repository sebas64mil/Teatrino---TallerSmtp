using UnityEngine;

public class EnemyRaycastMovement : MonoBehaviour
{
    public enum MovementAxis
    {
        Horizontal,
        Vertical
    }

    [Header("Movement")]

    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private MovementAxis movementAxis = MovementAxis.Horizontal;

    [SerializeField] private int direction = 1; 


    [Header("Raycast")]

    [SerializeField] private float rayDistance = 0.6f;

    [SerializeField] private LayerMask obstacleLayer;

    private Vector2 moveDirection;

    private void Start()
    {
        UpdateMoveDirection();
    }

    private void Update()
    {
        Move();
        DetectObstacle();
    }

    private void Move()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void DetectObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, rayDistance, obstacleLayer);

        if (hit.collider != null)
        {
            if (!hit.collider.CompareTag("Player"))
            {

                direction *= -1;
                UpdateMoveDirection();
            }
        }
    }

    private void UpdateMoveDirection()
    {
        if (movementAxis == MovementAxis.Horizontal)
            moveDirection = new Vector2(direction, 0);
        else
            moveDirection = new Vector2(0, direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 dir = (movementAxis == MovementAxis.Horizontal)
            ? new Vector2(direction, 0)
            : new Vector2(0, direction);

        Gizmos.DrawLine(transform.position, transform.position + (Vector3)dir * rayDistance);
    }
}
