using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager Instance;

    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ResetHealth();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int amount)
    {

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);


        EventsEmailSmtp.Instance.PlayerDied();


        if (currentHealth <= 0)
        {
            GameManager.Instance.RestartLevelFromStart();
            GameManager.Instance.Restart();
        }
        else
        {
            OnDeath();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    private void OnDeath()
    {

        GameManager.Instance.Restart();
    }
}
