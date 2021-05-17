using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
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
    [SerializeField] private bool changeItemLeft;
    [SerializeField] private bool changeItemRight;
    [SerializeField] private bool useItem;
    [SerializeField] private bool wallHug;
    [SerializeField] private bool wallHugLeft;
    [SerializeField] private bool wallHugRight;

    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private ItemControl itemControl;
    private PlayerUseItem playerUseItem;
    private PlayerRoll playerRoll;
    private PlayerWallHug playerWallHug;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        itemControl = FindObjectOfType<ItemControl>();
        playerUseItem = FindObjectOfType<PlayerUseItem>();
        playerRoll = FindObjectOfType<PlayerRoll>();
        playerStats = FindObjectOfType<PlayerStats>();

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
    }

    private void OnDisable()
    {
        
    }

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
        if (wallHug) objectivesRequired++;
        if (wallHugLeft) objectivesRequired++;
        if (wallHugRight) objectivesRequired++;
    }

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
            case TypeOfTutorial.WallHug:
                playerWallHug.TutorialWallHug -= TutorialPassed;
                break;
            case TypeOfTutorial.WallHugLeft:
                playerWallHug.TutorialWallHugLeft -= TutorialPassed;
                break;
            case TypeOfTutorial.WallHugRight:
                playerWallHug.TutorialWallHugRight -= TutorialPassed;
                break;
        }

        StartCoroutine(TutorialPassedCoroutine());
    }

    private IEnumerator TutorialPassedCoroutine()
    {
        yield return new WaitForSeconds(timeToWaitForObjectiveDone);
        objectivesPassed++;  
    }

    private void Update()
    {
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
