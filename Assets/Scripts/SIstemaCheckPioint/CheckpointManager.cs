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
    public void ClearCheckpoint()
    {
        hasCheckpoint = false;
    }

    public Vector3 GetInitialSpawnPoint()
    {
        return initialSpawnPoint;
    }

    public void SetInitialSpawn(Vector3 position)
    {
        initialSpawnPoint = position;
    }
}

