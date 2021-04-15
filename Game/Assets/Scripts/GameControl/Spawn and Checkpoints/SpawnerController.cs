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
    private GameState gameState;
    private Checkpoint[] childrenCheckpoints;
    private UIRespawn uiRespawn;
    private UIMainMenu uiMainMenu;

    private void Awake()
    {
        // Checkpoint variables
        childrenCheckpoints = new Checkpoint[GetComponentsInChildren<Checkpoint>().Length];
        childrenCheckpoints = GetComponentsInChildren<Checkpoint>();

        // Spawner variables
        uiRespawn = FindObjectOfType<UIRespawn>();
        uiMainMenu = FindObjectOfType<UIMainMenu>();

        // Create a GameState to check if save file exists
        gameState = new GameState(playerSavedStats);
    }

    private void Start()
    {
        PlayerPrefs.SetString("TypeOfSpawn", SpawnTypeEnum.Respawn.ToString());

        if (PlayerPrefs.GetString("TypeOfSpawn") == SpawnTypeEnum.Respawn.ToString())
        {
            StartCoroutine(SpawnPlayer(SpawnTypeEnum.Respawn));
        }
        else if (PlayerPrefs.GetString("TypeOfSpawn") == SpawnTypeEnum.Loadgame.ToString())
        {
            StartCoroutine(SpawnPlayer(SpawnTypeEnum.Loadgame));
        }
        else if (PlayerPrefs.GetString("TypeOfSpawn") == SpawnTypeEnum.Newgame.ToString())
        {
            StartCoroutine(SpawnPlayer(SpawnTypeEnum.Newgame));
        }
        else
        {
            // Left brank on purpose
            // Does nothing, meaning it's on main menu
        }
    }

    private void OnEnable()
    {
        if (uiRespawn != null) uiRespawn.RespawnButtonPressed += RespawnPlayer;
        if (uiMainMenu != null) uiMainMenu.MainMenuSpawn += RespawnPlayer;
    }

    private void OnDisable()
    {
        if (uiRespawn != null) uiRespawn.RespawnButtonPressed -= RespawnPlayer;
        if (uiMainMenu != null) uiMainMenu.MainMenuSpawn -= RespawnPlayer;
    }

    /// <summary>
    /// Loads a scene corresponding to the last checkpoint.
    /// If there is no checkpoint yet, loads default initial scene.
    /// </summary>
    private void RespawnPlayer(SpawnTypeEnum typeOfSpawn)
    {
        SceneControl sceneControl = FindObjectOfType<SceneControl>();

        if (typeOfSpawn == SpawnTypeEnum.Respawn)
        {
            PlayerPrefs.SetString("TypeOfSpawn", SpawnTypeEnum.Respawn.ToString());
        }
        else if (typeOfSpawn == SpawnTypeEnum.Loadgame)
        {
            PlayerPrefs.SetString("TypeOfSpawn", SpawnTypeEnum.Loadgame.ToString());
        }
        else if (typeOfSpawn == SpawnTypeEnum.Newgame)
        {
            // left brank on purpose
            PlayerPrefs.SetString("TypeOfSpawn", SpawnTypeEnum.Newgame.ToString());
            sceneControl.LoadScene(SceneEnum.Area1);
        }

        if (PlayerPrefs.GetString("TypeOfSpawn") != SpawnTypeEnum.Newgame.ToString())
        {
            if (gameState.FileExists(FilePath.SAVEFILESCENE))
            {
                // Loads the scene connected to the last saved checkpoint
                sceneControl.LoadScene(
                    gameState.LoadCheckpoint<SceneEnum>(SaveAndLoadEnum.CheckpointScene));
            }
            else
            {
                // Loads first scene after main menu.
                // Happens when the player is playing and didn't reach
                // the first checkpoint yet.
                sceneControl.LoadScene(SceneEnum.Area1);
            }
        }
    }

    /// <summary>
    /// Happens after first fixed update.
    /// </summary>
    /// <param name="typeOfSpawn">Type of player spawn.</param>
    /// <returns>Null.</returns>
    private IEnumerator SpawnPlayer(SpawnTypeEnum typeOfSpawn)
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        yield return waitForFixedUpdate;

        // After fixed update loads variables saved on last checkpoint
        // If the player already played through a checkpoint
        if (gameState.FileExists(FilePath.SAVEFILECHECKPOINT) &&
            typeOfSpawn != SpawnTypeEnum.Newgame)
        {
            foreach (Checkpoint checkpoint in childrenCheckpoints)
            {
                // If checkpoint number is  the same as the saved one
                if (checkpoint.CheckpointNumber == 
                    gameState.LoadCheckpoint<byte>(SaveAndLoadEnum.Checkpoint))
                {
                    // Instantiates the player on that checkpoint's position
                    if (FindObjectOfType<Player>() == null)
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
            if (FindObjectOfType<Player>() == null)
                Instantiate(
                    playerPrefab,
                    transform.position + initialPosition.transform.position,
                    initialPosition.transform.rotation);
        }

        // Finds player stats
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        // Creates FileIO
        gameState.AddPlayerStats(playerStats);

        // Loads saved stats OR default stats, if there's no saved stats yet
        gameState.LoadPlayerStats();

        // Refreshes UI
        FindObjectOfType<ItemUIParent>().UpdateAllItemUI();

        // Updates player's hp to the current hp on the last checkpoint
        yield return waitForFixedUpdate;
        if (typeOfSpawn == SpawnTypeEnum.Loadgame)
            playerStats.TakeDamage(100 - playerSavedStats.SavedHealth, TypeOfDamage.EnemyMelee);
    }

    /// <summary>
    /// If the player passes through a checkpoint, the script saves player stats.
    /// </summary>
    /// <param name="numberOfCheckpoint">Current checkpoint.</param>
    /// <param name="nameOfScene">Current scene.</param>
    public void SaveCheckpoint(byte numberOfCheckpoint, SceneEnum nameOfScene)
    {
        if (gameState.FileExists(FilePath.SAVEFILECHECKPOINT))
        {
            if (numberOfCheckpoint > gameState.LoadCheckpoint<byte>(SaveAndLoadEnum.Checkpoint))
            {
                gameState.SaveCheckpoint(numberOfCheckpoint);
                gameState.SaveCheckpointScene(nameOfScene);
                gameState.SavePlayerStats();
            }
        }
        else
        {
            gameState.SaveCheckpoint(numberOfCheckpoint);
            gameState.SaveCheckpointScene(nameOfScene);
            gameState.SavePlayerStats();
        }
    }

    /// <summary>
    /// Deletes all save files. Happens when the player presses new game on main menu.
    /// </summary>
    public void DeleteFiles() => gameState.DeleteFiles();
}
