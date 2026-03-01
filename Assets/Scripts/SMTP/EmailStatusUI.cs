using UnityEngine;
using TMPro;

public class EmailStatusUI : MonoBehaviour
{
    public TextMeshProUGUI statusText;

    private void OnEnable()
    {
        GameEmailNotifier.OnEmailStatusChanged += UpdateStatus;
    }

    private void OnDisable()
    {
        GameEmailNotifier.OnEmailStatusChanged -= UpdateStatus;
    }

    private void UpdateStatus(int code, string message)
    {
        statusText.text = $"[{code}] {message}";

        switch (code)
        {
            case 100:
                statusText.color = Color.yellow;
                break;

            case 200:
                statusText.color = Color.green;
                break;

            case 400:
                statusText.color = Color.red;
                break;

            case 500:
                statusText.color = new Color(0.8f, 0.2f, 0.2f);
                break;

            default:
                statusText.color = Color.white;
                break;
        }

        Debug.Log($"Email status updated: [{code}] {message}");
    }
}