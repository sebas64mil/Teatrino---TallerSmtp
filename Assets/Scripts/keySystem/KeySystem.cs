using System.Collections.Generic;
using UnityEngine;

public class KeySystem : MonoBehaviour
{
    public static KeySystem Instance { get; private set; }

    [Header("Keys Required")]
    [SerializeField] private int totalKeysRequired = 3;

    private HashSet<KeyType> collectedKeys = new HashSet<KeyType>();
    private List<Key> allKeys = new List<Key>();

    public int CollectedCount => collectedKeys.Count;
    public int TotalKeysRequired => totalKeysRequired;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterKey(Key key)
    {
        if (!allKeys.Contains(key))
            allKeys.Add(key);
    }


    public void CollectKey(KeyType key)
    {
        if (collectedKeys.Contains(key))
            return;

        collectedKeys.Add(key);

        // avisar UI
        Object.FindFirstObjectByType<KeyUIController>()?.UpdateKeys(collectedKeys.Count);
    }


    public bool HasAllKeys()
    {
        return collectedKeys.Count >= totalKeysRequired;
    }


    public void ClearKeys()
    {
        collectedKeys.Clear();

        foreach (Key key in allKeys)
        {
            if (key != null)
                key.ResetKey(); 
        }

    }

    public bool IsKeyCollected(KeyType key)
    {
        return collectedKeys.Contains(key);
    }

}
