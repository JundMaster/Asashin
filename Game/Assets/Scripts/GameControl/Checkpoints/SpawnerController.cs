using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform initialPosition;

    // Components
    [SerializeField] private PlayerStatsScriptableObj playerInventory;
    private PlayerStats playerStats;
    private FileIO saveAndLoad;
    private Checkpoint[] childrenCheckpoints;
    private UIRespawn uiRespawn;

    private void Awake()
    {
        childrenCheckpoints = new Checkpoint[GetComponentsInChildren<Checkpoint>().Length];
        childrenCheckpoints = GetComponentsInChildren<Checkpoint>();
        uiRespawn = FindObjectOfType<UIRespawn>();
    }

    private void Start()
    {
        StartCoroutine(SpawnPlayer());
    }

    /// <summary>
    /// Happens after first fixed update.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(1f);

        // After fixed update loads variables saved on last checkpoint
        if (saveAndLoad.FileExists(FilePath.SAVEFILECHECKPOINT))
        {
            foreach (Checkpoint checkpoint in childrenCheckpoints)
            {
                if (checkpoint.CheckpointNumber == saveAndLoad.LoadCheckpoint(SaveAndLoadEnum.Checkpoint))
                {
                    Instantiate(
                        playerPrefab, 
                        transform.position + checkpoint.transform.position, 
                        checkpoint.transform.rotation);
                }
            }
        }
        else
        {
            Instantiate(
                        playerPrefab,
                        transform.position + initialPosition.transform.position,
                        initialPosition.transform.rotation);
        }

        // Finds player stats
        playerStats = FindObjectOfType<PlayerStats>();

        // Creates FileIO
        saveAndLoad = new FileIO(playerInventory, playerStats);

        // Loads saved stats
        saveAndLoad.LoadPlayerStats();
        Debug.Log(playerStats.Health);
        // Refreshes UI
        FindObjectOfType<ItemUIParent>().UpdateAllItemUI();
    }

    private void OnEnable()
    {
        uiRespawn.RespawnButtonPressed += LoadCheckpoint;
    }

    private void OnDisable()
    {
        uiRespawn.RespawnButtonPressed -= LoadCheckpoint;
    }

    /// <summary>
    /// If the player passes through a checkpoint, the script saves player stats.
    /// </summary>
    /// <param name="numberOfCheckpoint">Current checkpoint.</param>
    /// <param name="numberOfScene">Current scene.</param>
    public void SaveCheckpoint(byte numberOfCheckpoint, byte numberOfScene)
    {
        if (saveAndLoad.FileExists(FilePath.SAVEFILECHECKPOINT))
        {
            if (numberOfCheckpoint > saveAndLoad.LoadCheckpoint(SaveAndLoadEnum.Checkpoint))
            {
                saveAndLoad.SaveCheckpoint(SaveAndLoadEnum.Checkpoint, numberOfCheckpoint);
                saveAndLoad.SaveCheckpoint(SaveAndLoadEnum.CheckpointScene, numberOfScene);
                saveAndLoad.SavePlayerStats();
            }
        }
        else
        {
            saveAndLoad.SaveCheckpoint(SaveAndLoadEnum.Checkpoint, numberOfCheckpoint);
            saveAndLoad.SaveCheckpoint(SaveAndLoadEnum.CheckpointScene, numberOfScene);
            saveAndLoad.SavePlayerStats();
        }
    }

    /// <summary>
    /// Loads a scene corresponding to the last checkpoint.
    /// </summary>
    private void LoadCheckpoint()
    {
        SceneControl sceneControl = FindObjectOfType<SceneControl>();
        if (saveAndLoad.FileExists(FilePath.SAVEFILESCENE))
        {
            sceneControl.LoadScene(saveAndLoad.LoadCheckpoint(SaveAndLoadEnum.CheckpointScene));
        }
        else
        {
            sceneControl.LoadScene(saveAndLoad.LoadCheckpoint(0));
        }
    }

    public void FindPlayer()
    {
        
    }

    public void PlayerLost()
    {
        //
    }
}
