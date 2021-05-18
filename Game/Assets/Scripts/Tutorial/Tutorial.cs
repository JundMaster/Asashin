using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for controling tutorial levels.
/// </summary>
public class Tutorial : MonoBehaviour
{
    [Header("Last door animator")]
    [SerializeField] private Animator tutorialDoor;
    [SerializeField] private float timeToWaitForObjectiveDone;

    private bool loadingScene;
    private byte objectivesRequired;
    private byte objectivesPassed;

    [SerializeField] private bool walk;
    [SerializeField] private bool hidden;
    [SerializeField] private bool run;
    [SerializeField] private bool sprint;
    [SerializeField] private bool roll;
    [SerializeField] private bool slowMotion;
    [SerializeField] private bool changeItemLeft;
    [SerializeField] private bool changeItemRight;
    [SerializeField] private bool useItem;
    [SerializeField] private bool wallHug;
    [SerializeField] private bool wallHugLeft;
    [SerializeField] private bool wallHugRight;
    [SerializeField] private bool alert;
    [SerializeField] private bool block;
    [SerializeField] private bool woodenBox;
    [SerializeField] private bool treasure;
    [SerializeField] private bool instantKill;

    // Components
    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private ItemControl itemControl;
    private PlayerUseItem playerUseItem;
    private PlayerRoll playerRoll;
    private PlayerWallHug playerWallHug;
    private PlayerMeleeAttack playerAttack;
    private EnemyTutorial enemyTutorial;
    private BreakableBox breakableBox;
    private TreasureBox treasureBox;
    private SlowMotionBehaviour slowMotionBehaviour;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        itemControl = FindObjectOfType<ItemControl>();
        playerUseItem = FindObjectOfType<PlayerUseItem>();
        playerRoll = FindObjectOfType<PlayerRoll>();
        playerStats = FindObjectOfType<PlayerStats>();
        playerWallHug = FindObjectOfType<PlayerWallHug>();
        playerAttack = FindObjectOfType<PlayerMeleeAttack>();
        enemyTutorial = FindObjectOfType<EnemyTutorial>();
        breakableBox = FindObjectOfType<BreakableBox>();
        treasureBox = FindObjectOfType<TreasureBox>();
        slowMotionBehaviour = FindObjectOfType<SlowMotionBehaviour>();
    }

    private void OnEnable()
    {
        if (playerMovement != null)
        {
            if (walk) playerMovement.TutorialWalk += TutorialPassed;
            if (run) playerMovement.TutorialRun += TutorialPassed;
            if (sprint) playerMovement.TutorialSprint += TutorialPassed;
            if (hidden) playerMovement.TutorialHidden += TutorialPassed;
        }

        if (playerRoll != null) 
            if (roll) playerRoll.TutorialRoll += TutorialPassed;

        if (itemControl != null)
        {
            if (changeItemLeft) itemControl.TutorialItemLeft += TutorialPassed;
            if (changeItemRight) itemControl.TutorialItemRight += TutorialPassed;
        }

        if (playerUseItem != null)
            if (useItem) playerUseItem.TutorialItemUse += TutorialPassed;

        if (playerWallHug != null)
        {
            if (wallHug) playerWallHug.TutorialWallHug += TutorialPassed;
            if (wallHugLeft) playerWallHug.TutorialWallHugLeft += TutorialPassed;
            if (wallHugRight) playerWallHug.TutorialWallHugRight += TutorialPassed;
        }

        if (playerAttack != null)
            if (instantKill) playerAttack.TutorialInstantKill += TutorialPassed;

        if (enemyTutorial != null)
        {
            if (block) enemyTutorial.TutorialBlock += TutorialPassed;
            if (alert) enemyTutorial.TutorialAlert += TutorialFailed;
        }

        if (breakableBox != null)
            if (woodenBox) breakableBox.TutorialBrokenBox += TutorialPassed;

        if (treasureBox != null)
            if (treasure) treasureBox.TutorialTreasure += TutorialPassed;

        if (slowMotionBehaviour != null)
            if (slowMotion) slowMotionBehaviour.TutorialSlowMotion += TutorialPassed;
    }

    private void OnDisable()
    {
        
    }

    /// <summary>
    /// Gives infinite items. Increments objectives required to pass this level.
    /// </summary>
    private void Start()
    {
        loadingScene = false;
        objectivesPassed = 0;

        playerStats.FirebombKunais += 10000;
        playerStats.Kunais += 10000;
        playerStats.HealthFlasks += 10000;
        playerStats.SmokeGrenades += 10000;

        if (walk) objectivesRequired++;
        if (hidden) objectivesRequired++;
        if (run) objectivesRequired++;
        if (sprint) objectivesRequired++;
        if (changeItemLeft) objectivesRequired++;
        if (changeItemRight) objectivesRequired++;
        if (useItem) objectivesRequired++;
        if (roll) objectivesRequired++;
        if (slowMotion) objectivesRequired++;
        if (wallHug) objectivesRequired++;
        if (instantKill) objectivesRequired++;
        if (wallHugLeft) objectivesRequired++;
        if (wallHugRight) objectivesRequired++;
        if (woodenBox) objectivesRequired++;
        if (treasure) objectivesRequired++;
        if (block) objectivesRequired++;
    }

    /// <summary>
    /// Loads current tutorial scene.
    /// </summary>
    private void TutorialFailed()
    {
        if (loadingScene == false)
        {
            SceneControl sceneControl = FindObjectOfType<SceneControl>();
            sceneControl.LoadScene(sceneControl.CurrentSceneEnum());
            loadingScene = true;
        }
    }

    /// <summary>
    /// Increments a value and unregisters from current event.
    /// </summary>
    /// <param name="typeOfTutorial">Type of tutorial passed.</param>
    private void TutorialPassed(TypeOfTutorial typeOfTutorial)
    {
        switch (typeOfTutorial)
        {
            case TypeOfTutorial.Walk:
                playerMovement.TutorialWalk -= TutorialPassed;
                break;
            case TypeOfTutorial.Hidden:
                playerMovement.TutorialHidden -= TutorialPassed;
                break;
            case TypeOfTutorial.Run:
                playerMovement.TutorialRun -= TutorialPassed;
                break;
            case TypeOfTutorial.Sprint:
                playerMovement.TutorialSprint -= TutorialPassed;
                break;
            case TypeOfTutorial.ItemChangeLeft:
                itemControl.TutorialItemLeft -= TutorialPassed;
                break;
            case TypeOfTutorial.ItemChangeRight:
                itemControl.TutorialItemRight -= TutorialPassed;
                break;
            case TypeOfTutorial.ItemUse:
                playerUseItem.TutorialItemUse -= TutorialPassed;
                break;
            case TypeOfTutorial.Roll:
                playerRoll.TutorialRoll -= TutorialPassed;
                break;
            case TypeOfTutorial.SlowMotion:
                slowMotionBehaviour.TutorialSlowMotion -= TutorialPassed;
                break;
            case TypeOfTutorial.WallHug:
                playerWallHug.TutorialWallHug -= TutorialPassed;
                break;
            case TypeOfTutorial.WallHugLeft:
                playerWallHug.TutorialWallHugLeft -= TutorialPassed;
                break;
            case TypeOfTutorial.WallHugRight:
                playerWallHug.TutorialWallHugRight -= TutorialPassed;
                break;
            case TypeOfTutorial.InstantKill:
                playerAttack.TutorialInstantKill -= TutorialPassed;
                break;
            case TypeOfTutorial.LootWoodenBox:
                breakableBox.TutorialBrokenBox -= TutorialPassed;
                break;
            case TypeOfTutorial.LootTreasure:
                treasureBox.TutorialTreasure -= TutorialPassed;
                break;
            case TypeOfTutorial.Block:
                enemyTutorial.TutorialBlock -= TutorialPassed;
                break;
        }

        // After x seconds, increments objectives passed to open the last door
        StartCoroutine(TutorialPassedCoroutine());
    }

    /// <summary>
    /// After x seconds, increments objectives passed to open the last door.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator TutorialPassedCoroutine()
    {
        yield return new WaitForSeconds(timeToWaitForObjectiveDone);
        objectivesPassed++;

        if (loadingScene == false)
        {
            if (objectivesPassed == objectivesRequired)
            {
                tutorialDoor.SetTrigger("OpenDoor");
                loadingScene = true;
            }
        }
    }
}
