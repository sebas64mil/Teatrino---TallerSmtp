using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject maskPrefab;

    [Header("Mask Sprites")]
    [SerializeField] private Sprite fullMaskSprite;   
    [SerializeField] private Sprite emptyMaskSprite;  

    private List<Image> masks = new List<Image>();
    private PlayerHealthManager healthManager;
    private bool initialized;

    private void Start()
    {
        healthManager = PlayerHealthManager.Instance;

        if (healthManager == null)
        {
            return;
        }

        Init(healthManager);
    }
    private void Init(PlayerHealthManager manager)
    {
        if (initialized) return;

        initialized = true;
        healthManager = manager;

        CreateMasks(manager.MaxHealth);
        UpdateMasks(manager.CurrentHealth);
    }


    private void CreateMasks(int maxHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject maskGO = Instantiate(maskPrefab, transform);
            Image img = maskGO.GetComponent<Image>();

            img.sprite = fullMaskSprite; 
            masks.Add(img);
        }
    }


    public void UpdateMasks(int currentHealth)
    {
        for (int i = 0; i < masks.Count; i++)
        {
            masks[i].sprite = i < currentHealth
                ? fullMaskSprite
                : emptyMaskSprite;
        }
    }
}
