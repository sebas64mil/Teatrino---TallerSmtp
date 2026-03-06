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

    public void OnChangeMask(InputValue value)
    {
        ChangeMaskPressed = value.isPressed;
    }

    public void OnInteract(InputValue value)
    {
        interactable = value.isPressed;
    }


    public void ConsumeChangeMask()
    {
        ChangeMaskPressed = false;
    }
}
