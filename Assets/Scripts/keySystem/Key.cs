using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Key : MonoBehaviour
{
    [SerializeField] private KeyType keyType;
    [SerializeField] private AudioSource SoundKey;

    private void Awake()
    {
        if (KeySystem.Instance != null)
        {
            KeySystem.Instance.RegisterKey(this);

            if (KeySystem.Instance.IsKeyCollected(keyType))
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (KeySystem.Instance == null)
        {
            return;
        }

        SoundKey.Play();

        EventsEmailSmtp.Instance.CollectedKey(keyType);
        KeySystem.Instance.CollectKey(keyType);
        gameObject.SetActive(false);
    }

    public void ResetKey()
    {
        gameObject.SetActive(true);
    }
}
