using UnityEngine;
using UnityEngine.UI; 

public class MapTrigger : MonoBehaviour
{
    [Header("Map UI")]
    [SerializeField] private GameObject mapUI;   
    [SerializeField] private Image mapUIImage;    
    [SerializeField] private Sprite mapSprite;  

    private PlayerInputHandler playerInput;
    private PlayerMovement2D playerMovement;

    [Header("Sound")]
    [SerializeField] private AudioSource mapSound;


    private bool mapOpen = false;
    private bool mapBlockedByPause = false;

    private void Update()
    {
        if (playerInput != null)
        {
            if (playerInput.interactable && !mapBlockedByPause)
            {
                ToggleMap();
                playerInput.interactable = false;
            }
        }

        if (mapOpen && GameManager.isPaused)
        {
            mapBlockedByPause = true;
            CloseMap();
        }

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

            if (mapUIImage != null && mapSprite != null)
                mapUIImage.sprite = mapSprite;
        }

        mapOpen = true;

        if (playerMovement != null)
            playerMovement.CanMove = false;

    }

    private void CloseMap()
    {
        if (mapUI != null)
            mapUI.SetActive(false);

        mapOpen = false;

        if (!GameManager.isPaused && playerMovement != null)
            playerMovement.CanMove = true;

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

            if (mapOpen)
                CloseMap();
        }
    }
}
