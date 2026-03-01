using System.Collections;
using UnityEngine;
using UnityEngine.Audio;


[RequireComponent(typeof(SpriteRenderer))]
public class PlayerLowHealthBlink : MonoBehaviour
{
    [Header("Blink Settings")]
    [SerializeField] private Color blinkColor = Color.red;
    [SerializeField] private float blinkSpeed = 0.15f;
    [SerializeField] private float blinkDuration = 1.5f;

    private SpriteRenderer sprite;
    private Color originalColor;
    private Coroutine blinkRoutine;

    [SerializeField] private AudioSource lowHealthAudio;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
    }

    private void Start()
    {
        CheckHealthAndBlink();
    }

    // =========================
    // CHECK LIFE
    // =========================
    public void CheckHealthAndBlink()
    {
        if (PlayerHealthManager.Instance.CurrentHealth <
            PlayerHealthManager.Instance.MaxHealth)
        {
            StartBlink();
        }
    }

    // =========================
    // BLINK LOGIC
    // =========================
    private void StartBlink()
    {
        ForceResetColor();

        if (lowHealthAudio != null)
            lowHealthAudio.Play();

        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkTimed());
    }

    private IEnumerator BlinkTimed()
    {
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            sprite.color = blinkColor;
            yield return new WaitForSeconds(blinkSpeed);

            sprite.color = originalColor;
            yield return new WaitForSeconds(blinkSpeed);

            elapsed += blinkSpeed * 2f;
        }

        // 🔒 aseguramos color final
        ForceResetColor();
        blinkRoutine = null;
    }

    // =========================
    // SAFETY
    // =========================
    private void ForceResetColor()
    {
        sprite.color = originalColor;
    }

    private void OnDisable()
    {
        ForceResetColor();
    }

    private void OnDestroy()
    {
        ForceResetColor();
    }
}
