using UnityEngine;
using System;

public class GameEmailNotifier : MonoBehaviour
{
    private EmailService emailService;

    public static event Action<int, string> OnEmailStatusChanged;

    private void Start()
    {
        emailService = EmailServiceLocator.Service;
    }

    private void OnEnable()
    {
        EventsEmailSmtp.KeyCollectedEvent += OnKeyCollected;
        EventsEmailSmtp.PlayerDiedEvent += OnPlayerDied;
        EventsEmailSmtp.PlayerWinEvent += OnPlayerWin;
    }

    private void OnDisable()
    {
        EventsEmailSmtp.KeyCollectedEvent -= OnKeyCollected;
        EventsEmailSmtp.PlayerDiedEvent -= OnPlayerDied;
        EventsEmailSmtp.PlayerWinEvent -= OnPlayerWin;
    }

    private async void Send(string subject, string body)
    {
        if (emailService == null)
        {
            OnEmailStatusChanged?.Invoke(500, "Email service not initialized");
            return;
        }

        OnEmailStatusChanged?.Invoke(100, "Sending email...");

        await emailService.SendEmailAsync(subject, body, (success, message) =>
        {
            if (success)
            {
                OnEmailStatusChanged?.Invoke(200, message);
            }
            else
            {
                OnEmailStatusChanged?.Invoke(400, message);
            }
        });
    }

    private void OnKeyCollected(string msg)
    {
        Send("Key collected - Teatrino", msg);
    }

    private void OnPlayerDied(string msg)
    {
        Send("Game Over - Teatrino", msg);
    }

    private void OnPlayerWin(string msg)
    {
        Send("Victory - Teatrino", msg);
    }
}