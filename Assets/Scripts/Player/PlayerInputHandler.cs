using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool ChangeMaskPressed { get; private set; }

    public bool interactable { get; set; }

  

    // Movimiento
    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    // Cambio de máscara (solo estado)
    public void OnChangeMask(InputValue value)
    {
        ChangeMaskPressed = value.isPressed;
    }

    public void OnInteract(InputValue value)
    {
        interactable = value.isPressed;
    }


    // Consumir input (opcional pero recomendado)
    public void ConsumeChangeMask()
    {
        ChangeMaskPressed = false;
    }
}
