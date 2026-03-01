using System;
using UnityEngine;

public class EventsEmailSmtp : MonoBehaviour
{
    public static EventsEmailSmtp Instance;

    public static event Action<string> KeyCollectedEvent;
    public static event Action<string> PlayerDiedEvent;
    public static event Action<string> PlayerWinEvent;

    private void Awake()
    {
        Instance = this;
    }

    public void CollectedKey(KeyType keyType)
    {
        string keyName = GetKeyName(keyType);
        KeyCollectedEvent?.Invoke($"You have collected Key {keyName}, congratulations!");
    }

    public void PlayerDied()
    {
        PlayerDiedEvent?.Invoke("You lost this encounter, it's okay, you have another one coming up."
);
    }

    public void PlayerWin()
    {
        PlayerWinEvent?.Invoke("You managed to escape the theater, congratulations, your skills are on another level"
);
    }

    private string GetKeyName(KeyType keyType)
    {
        return keyType switch
        {
            KeyType.Key1 => "Number  1",
            KeyType.Key2 => "Number  2",
            KeyType.Key3 => "Number  3",
            _ => "Unknown"
        };
    }
}
