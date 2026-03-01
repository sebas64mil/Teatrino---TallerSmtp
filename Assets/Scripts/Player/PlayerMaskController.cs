using UnityEngine;
using UnityEngine.Audio;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerMaskController : MonoBehaviour, IDamageable, ICrossable
{
    [Header("Current Mask")]
    [SerializeField] private MaskType currentMask = MaskType.Happy;

    [Header("Sound")]
    [SerializeField] private AudioSource audioMask;

    [SerializeField] private Animator animator;
    private PlayerInputHandler input;

    private void Awake()
    {
        
        input = GetComponent<PlayerInputHandler>();

        UpdateMaskAnimation();
    }

    private void Update()
    {
        HandleMaskChange();
        UpdateMaskAnimation();
    }

    private void HandleMaskChange()
    {
        if (!input.ChangeMaskPressed) return;

        input.ConsumeChangeMask();

        currentMask = currentMask == MaskType.Happy ? MaskType.Sad : MaskType.Happy;

        audioMask.Play();

        Debug.Log($" Máscara cambiada a: {currentMask}");
    }

    private void UpdateMaskAnimation()
    {
        
        // 1 = Happy, 0 = Sad
        float feliz = currentMask == MaskType.Happy ? 1f : 0f;
        animator.SetFloat("Feliz", feliz);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentMask == MaskType.Happy)
        {
            if (other.CompareTag("Obstacle1"))
                Cross(other.tag);
            else if (other.CompareTag("Obstacle2"))
                TakeDamage(1);
        }
        else
        {
            if (other.CompareTag("Obstacle2"))
                Cross(other.tag);
            else if (other.CompareTag("Obstacle1"))
                TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        Debug.Log($" Daño recibido: {amount} | Máscara: {currentMask}");
        PlayerHealthManager.Instance.TakeDamage(amount);
    }


    public void Cross(string obstacleTag)
    {
        Debug.Log($" Atraviesa {obstacleTag} con máscara {currentMask}");
    }
}

public enum MaskType
{
    Happy,
    Sad
}
