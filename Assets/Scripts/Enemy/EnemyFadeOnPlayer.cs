using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyFadeOnPlayer : MonoBehaviour
{
    [Header("Fade Settings")]
    [Tooltip("Alpha cuando el jugador está dentro del enemigo")]
    [Range(0f, 1f)]
    [SerializeField] private float fadedAlpha = 0.4f;

    [Tooltip("Velocidad de transición del fade")]
    [SerializeField] private float fadeSpeed = 8f;

    private SpriteRenderer spriteRenderer;
    private float targetAlpha;
    private float originalAlpha;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalAlpha = spriteRenderer.color.a;
        targetAlpha = originalAlpha;
    }

    private void Update()
    {
        Color color = spriteRenderer.color;
        color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime * fadeSpeed);
        spriteRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        targetAlpha = fadedAlpha;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        targetAlpha = originalAlpha;
    }
}
