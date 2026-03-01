using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private string fromEmail = "ingmultimediausbbog@gmail.com";
    private string password = "fsjq ioqf zsxs jrzf";
    private string toEmail;

    public EmailService(string destinationEmail)
    {
        toEmail = destinationEmail;
    }

    public async Task SendEmailAsync(string subject, string body, Action<bool, string> onResult)
    {
        try
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Port = 587;
                    smtp.Credentials = new NetworkCredential(fromEmail, password);
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(mail);
                }
            }

            onResult?.Invoke(true, "Email sent successfully ");
        }
        catch (Exception ex)
        {
            onResult?.Invoke(false, "Error: " + ex.Message);
        }
    }
}