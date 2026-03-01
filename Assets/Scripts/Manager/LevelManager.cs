using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private InputActionReference pauseAction;

    private bool isPaused = false;

    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPausePressed;
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= OnPausePressed;
        pauseAction.action.Disable();
    }

    private void Start()
    {
        GameManager.CursorVisible(false);
        pauseMenu.SetActive(false);
        GameManager.Instance.GamePause(false);
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;
        PauseMenuVisible(isPaused);
    }

    public void PauseMenuVisible(bool state)
    {
        pauseMenu.SetActive(state);
        GameManager.Instance.GamePause(state);
    }
}
