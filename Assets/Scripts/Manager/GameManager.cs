using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool isPaused = false;

    [Header("Scene Transition")]
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private float transitionTime = 1f;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Start()
    {
        LoadPlayerPrefs();
    }


    public static void CursorVisible(bool state)
    {
        Cursor.visible = state;
        if (state)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithFade(sceneName));
    }

    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        // pedir fade
        Time.timeScale = 1f;
        transitionAnimator.SetBool("IsFade", true);

        // esperar animación
        yield return new WaitForSeconds(transitionTime);

        isPaused = false;

        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GamePause(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0f : 1f;
    }

    public void Restart()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        ChangeScene(currentScene);
    }

    public void RestartLevelFromStart()
    {
        PlayerHealthManager.Instance.ResetHealth();
        CheckpointManager.Instance.ClearCheckpoint();
        KeySystem.Instance.ClearKeys();
        ChangeScene(SceneManager.GetActiveScene().name);
    }

    public void LoadPlayerPrefs()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float ambient = PlayerPrefs.GetFloat("AmbientVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetMixerVolume("MasterVolume", master);
        SetMixerVolume("AmbientVolume", ambient);
        SetMixerVolume("SFXVolume", sfx);
    }
    private void SetMixerVolume(string parameter, float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameter, dB);
    }
}
