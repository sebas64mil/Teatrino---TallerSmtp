# 🎭 Teatrino - Taller SMTP

---

# 📑 Índice

1. [¿Qué es el proyecto?](#1️⃣-qué-es-el-proyecto)
2. [Organización del Proyecto](#2️⃣-organización-del-proyecto)
   - 2.1 [Estructura Principal](#21-estructura-principal)
   - 2.2 [Carpeta SMTP](#22-carpeta-smtp)
3. [Flujo Básico de Envío SMTP](#3️⃣-flujo-básico-de-envío-smtp)
   - 3.1 [Registro del Email](#31-registro-del-email)
   - 3.2 [Disparo de Evento del Juego](#32-disparo-de-evento-del-juego)
   - 3.3 [Notificación y Envío](#33-notificación-y-envío)
   - 3.4 [Respuesta del Servidor](#34-respuesta-del-servidor)
4. [Flujo General del Sistema](#4️⃣-flujo-general-del-sistema)
5. [Pantallazos del Juego](#5️⃣-pantallazos-del-juego)

---

## 1️⃣ ¿Qué es el proyecto?

**Teatrino - Taller SMTP** es un mini-videojuego desarrollado en **Unity** cuyo objetivo principal es demostrar la integración funcional de un sistema de notificaciones por correo electrónico utilizando el protocolo SMTP.

El proyecto fue realizado como parte de un taller académico enfocado en evidenciar la correcta conexión entre:

- 🎮 Lógica del videojuego  
- 🖥 Interfaz de usuario básica en Unity  
- 🧠 Generación dinámica de mensajes  
- 📧 Envío real de correos electrónicos mediante SMTP  

Más allá de la complejidad del gameplay, el propósito central del proyecto es validar que un evento interno del juego pueda generar una notificación automática por correo, mostrando claramente el flujo completo desde la acción del jugador hasta la respuesta del servidor SMTP.

---

### 🎭 Contexto del Juego

**Teatrino** es un juego de aventura en solitario en el que el jugador encarna a un actor que intenta escapar de un pequeño teatro abandonado.

Durante la partida, debe:

- Evitar fantasmas y trampas  
- Utilizar máscaras para protegerse  
- Recolectar tickets distribuidos en el escenario  

Estos eventos del juego (recoger una llave, ganar la partida o perderla) activan el sistema de notificaciones por correo.

---

### 📧 Enfoque del Taller

El proyecto cumple con los siguientes requisitos académicos:

- ✅ Uso obligatorio del código SMTP proporcionado  
- ✅ Envío real de correos electrónicos  
- ✅ Construcción dinámica del asunto y cuerpo del mensaje  
- ✅ Visualización del estado del envío (éxito o error)  
- ✅ Interfaz clara que permite:
  - Ingresar el correo destino  
  - Activar eventos del juego  
  - Visualizar el estado del envío en tiempo real  

- La arquitectura implementada permite mantener desacoplado el sistema SMTP de la lógica principal del juego, utilizando eventos y programación asíncrona (`async/await`) para garantizar una integración limpia y funcional.
---

# 2️⃣ Organización del Proyecto

La estructura del proyecto sigue una organización modular dentro de la carpeta **Scripts**, separando responsabilidades por sistemas.

---

## 2.1 Estructura Principal
```
Assets/
│
├── Imagenes/
├── Materials/
├── prefabs/
├── Scenes/
├── Scripts/
│   ├── Enemy/
│   ├── HealthSystem/
│   ├── keySystem/
│   ├── Manager/
│   ├── Player/
│   ├── SistemaCheckPoint/
│   ├── SMTP/
│   │   ├── EmailInstaller.cs
│   │   ├── EmailService.cs
│   │   ├── EmailServiceLocator.cs
│   │   ├── EmailStatusUI.cs
│   │   ├── EventsEmailSmtp.cs
│   │   └── GameEmailNotifier.cs
│   ├── transicion/
│
├── Settings/
├── Sonido/
├── TextMesh Pro/
├── TileMap/
├── UI/
└── Packages/
```


---

## 2.2 Carpeta SMTP

Contiene toda la lógica relacionada con el sistema de envío de correos electrónicos:

- `EmailInstaller.cs`
- `EmailService.cs`
- `EmailServiceLocator.cs`
- `EventsEmailSmtp.cs`
- `GameEmailNotifier.cs`
- `EmailStatusUI.cs`

📌 Esta separación permite mantener desacoplado el sistema SMTP del resto de la lógica del juego.

---

# 3️⃣ Flujo Básico de Envío SMTP

El flujo general del sistema funciona de la siguiente manera:

---

## 3.1 Registro del Email

El jugador ingresa su correo en la interfaz.

- Script: `EmailInstaller.cs`
- Se valida el email.
- Se inicializa `EmailService` mediante `EmailServiceLocator`.

```csharp
public class EmailInstaller : MonoBehaviour
{
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
```
---

## 3.2 Disparo de Evento del Juego

Un evento del juego ocurre:

- Se recoge una llave
- El jugador muere
- El jugador gana

Estos eventos se disparan desde:

- `EventsEmailSmtp.cs`
```csharp
public static event Action<string> KeyCollectedEvent;
public static event Action<string> PlayerDiedEvent;
public static event Action<string> PlayerWinEvent;

public void CollectedKey(KeyType keyType)
{
    string keyName = GetKeyName(keyType);
    KeyCollectedEvent?.Invoke($"You have collected Key {keyName}, congratulations!");
}

public void PlayerDied()
{
    PlayerDiedEvent?.Invoke("You lost this encounter, it's okay, you have another one coming up.");
}

public void PlayerWin()
{
    PlayerWinEvent?.Invoke("You managed to escape the theater, congratulations, your skills are on another level");
}
```
---

## 3.3 – Notificación y Envío

- `GameEmailNotifier.cs` escucha los eventos.
- Se llama al método `Send()`.
- Se ejecuta `EmailService.SendEmailAsync()`.
- Se envía el correo usando `SmtpClient` (`smtp.gmail.com`, puerto 587, SSL).

### Suscripción a eventos
```csharp
private void OnEnable()
{
    EventsEmailSmtp.KeyCollectedEvent += OnKeyCollected;
    EventsEmailSmtp.PlayerDiedEvent += OnPlayerDied;
    EventsEmailSmtp.PlayerWinEvent += OnPlayerWin;
}
```
### Método Send()
```csharp
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
```
### Envío SMTP

```csharp
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
```
--- 

## 3.4 – Respuesta del Servidor

- El resultado del envío devuelve éxito o error.
- Se dispara `OnEmailStatusChanged`.
- `EmailStatusUI.cs` actualiza la interfaz con código y color correspondiente.

### Evento de estado
```csharp
public static event Action<int, string> OnEmailStatusChanged;
```
### UI de estado
```csharp
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
}
```
### Flujo General del Sistema
```
Jugador realiza acción (ej: recoger llave)
        ↓
EventsEmailSmtp dispara evento
        ↓
GameEmailNotifier escucha el evento
        ↓
GameEmailNotifier llama a EmailService.SendEmailAsync()
        ↓
SMTP (smtp.gmail.com:587) envía el correo
        ↓
Callback devuelve resultado (success/error)
        ↓
GameEmailNotifier dispara OnEmailStatusChanged
        ↓
EmailStatusUI actualiza el texto y color en pantalla
```
---
# 5️⃣ Pantallazos del Juego

📸 Agregar aquí las capturas del proyecto.

---

## 5.1 Pantalla de Registro de Email

![Pantalla Registro Email](Assets/Imagenes/ImagenesReadme/Registro.png)

---

## 5.2 Juego en Ejecución

![Gameplay](Assets/Imagenes/ImagenesReadme/InicioJuego.png)

---

## 5.3 Notificación de Envío Exitoso

![Email Success](Assets/Imagenes/ImagenesReadme/CorreoExitoso.png)

---

## 5.4 Notificación de esperarando

![Email Esperando](Assets/Imagenes/ImagenesReadme/Enviando.png)

---

## Requisitos

- Unity 6000.2.10f1
- .NET Framework (no .NET Standard) en Player Settings > Api Compatibility Level
- Cuenta de Gmail con contrasena de aplicacion habilitada
