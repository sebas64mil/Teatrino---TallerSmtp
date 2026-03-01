using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    [Header("Victory")]
    [SerializeField] private string victorySceneName = "VictoryScene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (KeySystem.Instance == null)
        {
            Debug.LogWarning("VictoryTrigger: KeySystem no encontrado");
            return;
        }

        if (KeySystem.Instance.HasAllKeys())
        {
            EventsEmailSmtp.Instance.PlayerWin();
            GameManager.Instance.RestartLevelFromStart();
            GameManager.Instance.ChangeScene(victorySceneName);
        }
        else
        {
            Debug.Log(" Aún faltan llaves");
        }
    }
}
