using UnityEngine;
using UnityEngine.UI; // Para Image de UI
using UnityEngine.Audio;

public class MapTrigger : MonoBehaviour
{
    [Header("Mapa UI")]
    [SerializeField] private GameObject mapUI;    // Panel del mapa que se activa/desactiva
    [SerializeField] private Image mapUIImage;    // Componente Image dentro del panel de UI
    [SerializeField] private Sprite mapSprite;    // Sprite específico de este mapa

    private PlayerInputHandler playerInput;
    private PlayerMovement2D playerMovement;

    [Header("Sound")]
    [SerializeField] private AudioSource mapSound;


    private bool mapOpen = false;
    private bool mapBlockedByPause = false; // Para evitar que el mapa se abra solo al despausar

    private void Update()
    {
        if (playerInput != null)
        {
            // Abrir/cerrar mapa con la acción Interact solo si no está bloqueado por pausa
            if (playerInput.interactable && !mapBlockedByPause)
            {
                ToggleMap();
                playerInput.interactable = false; // Consumir el input
            }
        }

        // Si el mapa está abierto y el juego se pausa, cerrarlo y bloquearlo
        if (mapOpen && GameManager.isPaused)
        {
            mapBlockedByPause = true;
            CloseMap();
        }

        // Si el juego se despausa, desbloquear la posibilidad de abrir el mapa
        if (!GameManager.isPaused)
        {
            mapBlockedByPause = false;
        }
    }

    private void ToggleMap()
    {
        mapSound.Play();


        if (mapOpen)
            CloseMap();
        else
            OpenMap();
    }

    private void OpenMap()
    {
        if (mapUI != null)
        {
            mapUI.SetActive(true);

            // Cambiar la imagen del mapa al sprite específico
            if (mapUIImage != null && mapSprite != null)
                mapUIImage.sprite = mapSprite;
        }

        mapOpen = true;

        // Bloquear movimiento
        if (playerMovement != null)
            playerMovement.CanMove = false;

    }

    private void CloseMap()
    {
        if (mapUI != null)
            mapUI.SetActive(false);

        mapOpen = false;

        // Desbloquear movimiento solo si el juego no está pausado
        if (!GameManager.isPaused && playerMovement != null)
            playerMovement.CanMove = true;

        // Ocultar cursor solo si el juego no está pausado
        if (!GameManager.isPaused)
            GameManager.CursorVisible(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareTag("Mapa") && collision.CompareTag("Player"))
        {
            playerInput = collision.GetComponent<PlayerInputHandler>();
            playerMovement = collision.GetComponent<PlayerMovement2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CompareTag("Mapa") && collision.CompareTag("Player"))
        {
            playerInput = null;
            playerMovement = null;

            // Cerrar mapa al salir del trigger
            if (mapOpen)
                CloseMap();
        }
    }
}
