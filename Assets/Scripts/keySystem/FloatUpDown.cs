using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float floatHeight = 0.25f; 
    [SerializeField] private float floatSpeed = 2f;    

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = startPosition + Vector3.up * offset;
    }
}
