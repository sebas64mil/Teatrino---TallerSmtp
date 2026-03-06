using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Mask Animator")]
    [SerializeField] private Animator maskAnimator; 

    [Header("Mask Offset")]
    [SerializeField] private Vector3 maskCenterOffset = new Vector3(0f, 0f, 0f);
    [SerializeField] private float maskSideOffset = 0.2f;

    [Header("Sound")]
    [SerializeField] private AudioSource walkingSound;

    private Rigidbody2D rb;
    private PlayerInputHandler input;
    private Animator animator;
    private Transform maskTransform;

    public bool CanMove { get; set; } = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputHandler>();
        animator = GetComponent<Animator>();

        if (maskAnimator != null)
            maskTransform = maskAnimator.transform;
        else

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Start()
    {
        if (CheckpointManager.Instance != null)
        {
            transform.position = CheckpointManager.Instance.GetRespawnPoint();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.isPaused)
        {
            if (walkingSound != null && walkingSound.isPlaying)
                walkingSound.Pause();

            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (!CanMove)
        {
            if (walkingSound != null && walkingSound.isPlaying)
                walkingSound.Pause();

            rb.linearVelocity = Vector2.zero; 
            return; 
        }

        Move();
        UpdateAnimator();
        UpdateMaskPosition();
    }

    private void Move()
    {
        if (input.MoveInput.magnitude > 0.1f)
        {
            if (walkingSound != null && !walkingSound.isPlaying)
                walkingSound.Play();
        }
        else
        {
            if (walkingSound != null && walkingSound.isPlaying)
                walkingSound.Stop();
        }



        Vector2 direction = input.MoveInput.normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void UpdateAnimator()
    {
        float x = Mathf.Round(input.MoveInput.x);
        float y = Mathf.Round(input.MoveInput.y);

        animator.SetFloat("Xmov", Mathf.Clamp(x, -1f, 1f));
        animator.SetFloat("Ymov", Mathf.Clamp(y, -1f, 1f));

        if (maskAnimator != null)
        {
            maskAnimator.SetFloat("Xmov1", Mathf.Clamp(x, -1f, 1f));

            bool movingUpOnly = y > 0.5f && Mathf.Abs(x) < 0.1f;
            maskAnimator.gameObject.SetActive(!movingUpOnly);
        }
    }

    private void UpdateMaskPosition()
    {
        if (maskTransform == null) return;

        float xOffset = 0f;
        if (input.MoveInput.x > 0.1f) xOffset = maskSideOffset;
        else if (input.MoveInput.x < -0.1f) xOffset = -maskSideOffset;

        maskTransform.localPosition = maskCenterOffset + new Vector3(xOffset, 0f, 0f);
    }
}
