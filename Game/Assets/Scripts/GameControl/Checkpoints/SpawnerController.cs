using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling player spawn.
/// Used in every scenes and main menu.
/// </summary>
public class SpawnerController : MonoBehaviour
{
    [SerializeField] private PlayerSavedStatsScriptableObj playerSavedStats;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform initialPosition;

    // Components
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
        // PLAYER FAZ RESPAWN SEMPRE EM MODO LOADING
        // SO PARA TESTES <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        PlayerPrefs.SetString("TypeOfSpawn", "Load");
        ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////

        if (PlayerPrefs.GetString("TypeOfSpawn") == "Respawn")
        {
            StartCoroutine(SpawnPlayer(SpawnTypeEnum.Respawn));
        }
        else if (PlayerPrefs.GetString("TypeOfSpawn") == "Load")
        {
            StartCoroutine(SpawnPlayer(SpawnTypeEnum.Loadgame));
        }
        else
        {
            // Left brank on purpose
            // Does nothing, meaning it's on main menu
        }
    }

    /// <summary>
    /// Happens after first fixed update.
    /// </summary>
    /// <param name="typeOfSpawn">Respawn or loadgame.</param>
    /// <returns>Null.</returns>
    private IEnumerator SpawnPlayer(SpawnTypeEnum typeOfSpawn)
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        yield return waitForFixedUpdate;

        // After fixed update loads variables saved on last checkpoint
        // If the player already played through a checkpoint
        if (saveAndLoad.FileExists(FilePath.SAVEFILECHECKPOINT))
        {
            foreach (Checkpoint checkpoint in childrenCheckpoints)
            {
                // If checkpoint number is  the same as the saved one
                if (checkpoint.CheckpointNumber == 
                    saveAndLoad.LoadCheckpoint(SaveAndLoadEnum.Checkpoint))
                {
                    // Instantiates the player on that checkpoint's position
                    Instantiate(
                        playerPrefab, 
                        transform.position + checkpoint.transform.position, 
                        checkpoint.transform.rotation);
                }
            }
        }
        // else if the player is playing for the first time
        else
        {
            Instantiate(
                        playerPrefab,
                        transform.position + initialPosition.transform.position,
                        initialPosition.transform.rotation);
        }

        // Finds player stats
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        // Creates FileIO
        saveAndLoad = new FileIO(playerSavedStats, playerStats);

        // Loads saved stats OR default stats, if there's no saved stats yet
        saveAndLoad.LoadPlayerStats();

        // Refreshes UI
        FindObjectOfType<ItemUIParent>().UpdateAllItemUI();

        // Updates player's hp to the current hp on the last checkpoint
        yield return waitForFixedUpdate;
        if (typeOfSpawn == SpawnTypeEnum.Loadgame)
            playerStats.TakeDamage(100 - playerSavedStats.SavedHealth);
    }

    private void OnEnable()
    {
        if (uiRespawn) uiRespawn.RespawnButtonPressed += RespawnPlayer;
    }

    private void OnDisable()
    {
        if (uiRespawn) uiRespawn.RespawnButtonPressed -= RespawnPlayer;
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
    /// If there is no checkpoint yet, loads default initial scene.
    /// </summary>
    private void RespawnPlayer(SpawnTypeEnum typeOfSpawn)
    {
        if (typeOfSpawn == SpawnTypeEnum.Respawn)
        {
            PlayerPrefs.SetString("TypeOfSpawn", "Respawn");
        }
        else if (typeOfSpawn == SpawnTypeEnum.Loadgame)
        {
            PlayerPrefs.SetString("TypeOfSpawn", "Load");
        }
        else if (typeOfSpawn == SpawnTypeEnum.Newgame)
        {
            // left brank on purpose
        }

        SceneControl sceneControl = FindObjectOfType<SceneControl>();

        if (saveAndLoad.FileExists(FilePath.SAVEFILESCENE))
        {
            // Loads the scene connected to the last saved checkpoint
            sceneControl.LoadScene(
                saveAndLoad.LoadCheckpoint(SaveAndLoadEnum.CheckpointScene));
        }
        else
        {
            // Loads first scene after main menu
            sceneControl.LoadScene(1);
        }  
    }

    /// <summary>
    /// Deletes all save files. Happens when the player presses new game on main menu.
    /// </summary>
    private void DeleteFiles() => saveAndLoad.DeleteFiles();

    /// <summary>
    /// Resets playerprefs TypeOfSpawn when the game closes.
    /// </summary>
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("TypeOfSpawn", "RandomStringMeaningTheGameClosed");
    }
}
