using UnityEngine;

public class EnemyRaycastMovement : MonoBehaviour
{
    public enum MovementAxis
    {
        Horizontal,
        Vertical
    }

    [Header("Movement")]

    [Tooltip("Velocidad a la que se mueve el enemigo")]
    [SerializeField] private float moveSpeed = 2f;

    [Tooltip("Eje en el que se moverá el enemigo (Horizontal o Vertical)")]
    [SerializeField] private MovementAxis movementAxis = MovementAxis.Horizontal;

    [Tooltip("Dirección inicial del movimiento: 1 = derecha/arriba, -1 = izquierda/abajo")]
    [SerializeField] private int direction = 1; // 1 o -1


    [Header("Raycast")]

    [Tooltip("Distancia del raycast para detectar obstáculos frente al enemigo")]
    [SerializeField] private float rayDistance = 0.6f;

    [Tooltip("Layers que harán que el enemigo cambie de dirección (NO incluir Player)")]
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
            // Si NO es el player cambia dirección
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
