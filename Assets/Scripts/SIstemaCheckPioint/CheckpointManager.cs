using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [Header("Initial Spawn")]
    [SerializeField] private Vector3 initialSpawnPoint;

    private Vector3 currentCheckpoint;
    private bool hasCheckpoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================
    // CHECKPOINT LOGIC
    // =========================
    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        currentCheckpoint = checkpointPosition;
        hasCheckpoint = true;
    }

    public Vector3 GetRespawnPoint()
    {
        if (hasCheckpoint)
            return currentCheckpoint;

        return initialSpawnPoint;
    }

    // =========================
    // RESET / CONTROL
    // =========================

    //  Para que al reiniciar NO vaya al último checkpoint
    public void ClearCheckpoint()
    {
        hasCheckpoint = false;
    }

    //  Forzar volver al inicio del nivel
    public Vector3 GetInitialSpawnPoint()
    {
        return initialSpawnPoint;
    }

    //  Permite cambiar el spawn inicial si lo necesitas
    public void SetInitialSpawn(Vector3 position)
    {
        initialSpawnPoint = position;
    }
}

