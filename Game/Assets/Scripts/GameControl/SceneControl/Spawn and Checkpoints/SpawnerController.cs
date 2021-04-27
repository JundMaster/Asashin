using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handling player spawn.
/// Used in every scenes and main menu.
/// This class, registered to events and it calls TypeOfRespawn with the
/// type of respawn desired. After this, the class will load a saved scene
/// if the player already has a save file, else it will load the first area.
/// After loading the scene, it will spawn the player in the initial position
/// or on the last checkpoint visited, and load his saved stats.
/// </summary>
public class SpawnerController : MonoBehaviour
{
    [Header("Turn this on for tests, it will set TypeOfSpawn to Load type")]
    [SerializeField] private bool ONFORTESTING;

    [SerializeField] private PlayerSavedStatsScriptableObj playerSavedStats;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform initialPosition;

    // Components
    public GameState GameState { get; private set; }
    private Checkpoint[] childrenCheckpoints;
    
    private SceneControl sceneControl;
    private LevelChanger levelChanger;

    // Components to load or respawn player
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
        levelChanger = FindObjectOfType<LevelChanger>();

        // Create a GameState to check if save file exists
        GameState = new GameState(playerSavedStats);
        sceneControl = FindObjectOfType<SceneControl>();
    }

    private void OnEnable()
    {
        if (uiRespawn != null) uiRespawn.RespawnButtonPressed += TypeOfRespawn;
        if (uiMainMenu != null) uiMainMenu.MainMenuSpawn += TypeOfRespawn;
        if (levelChanger != null) levelChanger.LevelChanged += TypeOfRespawn;
    }

    private void OnDisable()
    {
        if (uiRespawn != null) uiRespawn.RespawnButtonPressed -= TypeOfRespawn;
        if (uiMainMenu != null) uiMainMenu.MainMenuSpawn -= TypeOfRespawn;
        if (levelChanger != null) levelChanger.LevelChanged -= TypeOfRespawn;
    }

    /// <summary>
    /// Loads a scene corresponding to the last checkpoint.
    /// </summary>
    public void TypeOfRespawn(SpawnTypeEnum typeOfSpawn)
    {
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
            DeleteFiles();
            PlayerPrefs.SetString("TypeOfSpawn", SpawnTypeEnum.Newgame.ToString());
            sceneControl.LoadScene(SceneEnum.Area1);
            return;
        }

        // If it's respawn or load game
        if (PlayerPrefs.GetString("TypeOfSpawn") != SpawnTypeEnum.Newgame.ToString())
        {
            // If a save file already exists, it loads the scene saved
            if (GameState.FileExists(FilePath.SAVEFILESCENE))
            {
                // Loads the scene connected to the last saved file
                sceneControl.LoadScene(GameState.LoadCheckpointScene());
            }
        }
    }

    /// <summary>
    /// Happens when the player goes to next area.
    /// Loads the next scene.
    /// </summary>
    /// <param name="sceneToLoad">Scene to load.</param>
    private void TypeOfRespawn(SceneEnum sceneToLoad)
    {
        PlayerPrefs.SetString("TypeOfSpawn", SpawnTypeEnum.Loadgame.ToString());

        // Loads the scene connected to the last saved file
        sceneControl.LoadScene(sceneToLoad);
    }

    private void Start()
    {
        // ONLY FOR TESTS
        if (ONFORTESTING)
            PlayerPrefs.SetString("TypeOfSpawn", SpawnTypeEnum.Loadgame.ToString());
        ////////////////////////////////////////////////////////////////////////
        
        // Main menu is always visited by default.
        PlayerPrefs.SetInt(SceneEnum.MainMenu.ToString(), 1);

        // NOT in main menu
        if (sceneControl.CurrentSceneEnum() != SceneEnum.MainMenu)
        {
            // If the current scene was already visited
            if (PlayerPrefs.GetInt(sceneControl.CurrentSceneEnum().ToString()) == 1)
            {
                if (PlayerPrefs.GetString("TypeOfSpawn") == SpawnTypeEnum.Respawn.ToString())
                {
                    StartCoroutine(SpawnPlayer(SpawnTypeEnum.Respawn));
                    return;
                }
                else if (PlayerPrefs.GetString("TypeOfSpawn") == SpawnTypeEnum.Loadgame.ToString())
                {
                    StartCoroutine(SpawnPlayer(SpawnTypeEnum.Loadgame));
                    return;
                }
            }
            // Else if this scene was NOT visited yet
            else
            {
                // Sets this current scene as visited.
                PlayerPrefs.SetInt(sceneControl.CurrentSceneEnum().ToString(), 1);

                // If the player just started a new game, else it will ignore this
                if (PlayerPrefs.GetString("TypeOfSpawn") == SpawnTypeEnum.Newgame.ToString())
                {
                    StartCoroutine(SpawnPlayer(SpawnTypeEnum.Newgame));
                    return;
                }
                
                else if (PlayerPrefs.GetString("TypeOfSpawn") == SpawnTypeEnum.Loadgame.ToString())
                {
                    StartCoroutine(SpawnPlayer(SpawnTypeEnum.Loadgame));
                }
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
        if (GameState.FileExists(FilePath.SAVEFILECHECKPOINT) &&
            typeOfSpawn != SpawnTypeEnum.Newgame)
        {
            // If player didn't visit this map yet, spawns on initial position of that map
            if ((byte)GameState.LoadCheckpointScene() < (byte)sceneControl.CurrentSceneEnum())
            {
                if (FindObjectOfType<Player>() == null)
                    Instantiate(
                        playerPrefab,
                        transform.position + initialPosition.transform.position,
                        initialPosition.transform.rotation);
            }
            // Else if player already visited this map, spawns on a checkpoint
            else
            {
                foreach (Checkpoint checkpoint in childrenCheckpoints)
                {
                    // If checkpoint number is  the same as the saved one
                    if (checkpoint.CheckpointNumber ==
                        GameState.LoadCheckpoint())
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
        }
        // else if the player is playing for the first time ( new game )
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
        GameState.AddPlayerStats(playerStats);

        // Loads saved stats OR default stats, if there's no saved stats yet
        GameState.LoadPlayerStats();

        // Refreshes UI
        FindObjectOfType<ItemUIParent>().UpdateAllItemUI();

        // Updates player's hp to the current hp on the last checkpoint
        yield return waitForFixedUpdate;
        if (typeOfSpawn == SpawnTypeEnum.Loadgame)
            playerStats.TakeDamage(100 - playerSavedStats.SavedHealth, TypeOfDamage.None);
    }

    /// <summary>
    /// If the player passes through a higher checkpoint, the script saves player stats,
    /// saves number of checkpoint, saves current scene.
    /// </summary>
    /// <param name="numberOfCheckpoint">Current checkpoint.</param>
    /// <param name="nameOfScene">Current scene.</param>
    public void SaveCheckpoint(byte numberOfCheckpoint, SceneEnum nameOfScene)
    {
        if (GameState.FileExists(FilePath.SAVEFILECHECKPOINT))
        {
            // Only saves if the current scene is higher than the current saved one
            if ((byte)GameState.LoadCheckpointScene() < (byte)sceneControl.CurrentSceneEnum())
            {
                GameState.SaveCheckpoint(numberOfCheckpoint);
                GameState.SaveCheckpointScene(nameOfScene);
                GameState.SavePlayerStats();
                OnCheckPointReached();
            }
            // Else if this scene is the same
            else
            {
                // Only saves if the current checkpoint is higher than the current saved one
                if (numberOfCheckpoint > GameState.LoadCheckpoint())
                {
                    GameState.SaveCheckpoint(numberOfCheckpoint);
                    GameState.SaveCheckpointScene(nameOfScene);
                    GameState.SavePlayerStats();
                    OnCheckPointReached();
                }
            }
        }
        // Else if save file doesn't exist yet
        else
        {
            GameState.SaveCheckpoint(numberOfCheckpoint);
            GameState.SaveCheckpointScene(nameOfScene);
            GameState.SavePlayerStats();
            OnCheckPointReached();
        }
    }

    /// <summary>
    /// Deletes all save files. Happens when the player presses new game on main menu.
    /// </summary>
    public void DeleteFiles()
    {
        PlayerPrefs.DeleteKey(SceneEnum.Area1.ToString());
        PlayerPrefs.DeleteKey(SceneEnum.Area2.ToString());
        PlayerPrefs.DeleteKey(SceneEnum.Area3.ToString());
        PlayerPrefs.DeleteKey(SceneEnum.Area4.ToString());
        PlayerPrefs.DeleteKey(SceneEnum.Area5.ToString());
        PlayerPrefs.DeleteKey(SceneEnum.Area6.ToString());
        PlayerPrefs.DeleteKey(SceneEnum.ProgrammingTests.ToString());
        PlayerPrefs.DeleteKey(SceneEnum.TESTAREA.ToString());
        GameState.DeleteFiles();
    }

    /// <summary>
    /// Invokes checkpoint reached method. This method is called everytime the
    /// player reaches a new checkpoint.
    /// </summary>
    protected virtual void OnCheckPointReached() => CheckpointReached?.Invoke();

    /// <summary>
    /// Event registered on UISaveIcon.
    /// </summary>
    public event Action CheckpointReached;
}
