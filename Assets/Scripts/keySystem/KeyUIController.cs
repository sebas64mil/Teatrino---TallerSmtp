using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyUIController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject keyPrefab;

    [Header("Key Sprites")]
    [SerializeField] private Sprite keyOffSprite;
    [SerializeField] private Sprite keyOnSprite;

    [Header("Complete Message")]
    [SerializeField] private TMP_Text completeText;
    [SerializeField] private string completeMessage = "Ve hacia la cortina";

    private List<Image> keysUI = new List<Image>();
    private bool initialized;
    private int totalKeys;

    private void Start()
    {
        if (KeySystem.Instance == null)
        {
            return;
        }

        Init(KeySystem.Instance);
    }

    private void Init(KeySystem keySystem)
    {
        if (initialized) return;
        initialized = true;

        totalKeys = keySystem.TotalKeysRequired;

        CreateKeys(totalKeys);
        UpdateKeys(keySystem.CollectedCount);

        if (completeText != null)
            completeText.text = ""; 
    }

    private void CreateKeys(int totalKeys)
    {
        for (int i = 0; i < totalKeys; i++)
        {
            GameObject keyGO = Instantiate(keyPrefab, transform);
            Image img = keyGO.GetComponent<Image>();

            img.sprite = keyOffSprite;
            keysUI.Add(img);
        }
    }


    public void UpdateKeys(int collectedKeys)
    {
        for (int i = 0; i < keysUI.Count; i++)
        {
            keysUI[i].sprite = i < collectedKeys
                ? keyOnSprite
                : keyOffSprite;
        }

        if (completeText == null) return;

        if (collectedKeys >= totalKeys)
            completeText.text = completeMessage;
        else
            completeText.text = "";
    }
}
