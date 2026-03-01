using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmailInstaller : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField emailInput;
    public Button playButton;

    private void Start()
    {
        playButton.interactable = false;
    }

    public void SaveEmail()
    {
        string email = emailInput.text;

        if (!IsValidEmail(email))
        {
            Debug.Log("Invalid email address");
            return;
        }

        EmailServiceLocator.Service = new EmailService(email);

        playButton.interactable = true;

        Debug.Log("Email successfully saved: " + email);
    }

    private bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }
}