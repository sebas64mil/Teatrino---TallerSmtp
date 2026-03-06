using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrapController : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float openTime = 1.5f;
    [SerializeField] private float closedTime = 1.5f;

    [Header("References")]
    [SerializeField] private Animator animator;

    private Collider2D trapCollider;
    private bool isOpen;

    private void Awake()
    {
        trapCollider = GetComponent<Collider2D>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(TrapLoop());
    }

    private IEnumerator TrapLoop()
    {
        while (true)
        {
            SetTrapState(true);
            yield return new WaitForSeconds(openTime);

            SetTrapState(false);
            yield return new WaitForSeconds(closedTime);
        }
    }

    private void SetTrapState(bool open)
    {
        isOpen = open;

        animator.SetBool("IsOpen", open);
        trapCollider.enabled = open;
    }
}
