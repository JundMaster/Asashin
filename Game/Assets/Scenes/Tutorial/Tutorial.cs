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
    [SerializeField] private bool run;
    [SerializeField] private bool sprint;
    [SerializeField] private bool roll;
    [SerializeField] private bool changeItemLeft;
    [SerializeField] private bool changeItemRight;
    [SerializeField] private bool useItem;

    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private ItemControl itemControl;
    private PlayerUseItem playerUseItem;
    private PlayerRoll playerRoll;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        itemControl = FindObjectOfType<ItemControl>();
        playerUseItem = FindObjectOfType<PlayerUseItem>();
        playerRoll = FindObjectOfType<PlayerRoll>();
        playerStats = FindObjectOfType<PlayerStats>();


        playerStats.FirebombKunais += 10000;
        playerStats.Kunais += 10000;
        playerStats.HealthFlasks += 10000;
        playerStats.SmokeGrenades += 10000;
    }

    private void OnEnable()
    {
        if (playerMovement != null)
        {
            if (walk) playerMovement.TutorialWalk += TutorialPassed;
            if (run) playerMovement.TutorialRun += TutorialPassed;
            if (sprint) playerMovement.TutorialSprint += TutorialPassed;
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
    }

    private void Start()
    {
        loadingScene = false;
        objectivesPassed = 0;

        if (walk) objectivesRequired++;
        if (run) objectivesRequired++;
        if (sprint) objectivesRequired++;
        if (changeItemLeft) objectivesRequired++;
        if (changeItemRight) objectivesRequired++;
        if (useItem) objectivesRequired++;
        if (roll) objectivesRequired++;
    }

    private void TutorialPassed(TypeOfTutorial typeOfTutorial)
    {
        switch (typeOfTutorial)
        {
            case TypeOfTutorial.Walk:
                playerMovement.TutorialWalk -= TutorialPassed;
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
