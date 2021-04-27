using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIMainMenuButtonSelect : MonoBehaviour
{

    private EventSystem eventSys;
    private GameObject lastSelectedGameObject;

    [SerializeField] private GameObject newGame;
    [SerializeField] private GameObject continueGame;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject quit;

    private void Start()
    {
        eventSys = FindObjectOfType<EventSystem>();
       
    }

    public void SelectMenuOption()
    {
        if (newGame.activeSelf) eventSys.SetSelectedGameObject(newGame);

        else if (continueGame.activeSelf) eventSys.SetSelectedGameObject(continueGame);

        else if (options.activeSelf) eventSys.SetSelectedGameObject(options);

        else eventSys.SetSelectedGameObject(quit);

    }

    private void Update()
    {
        Debug.Log(eventSys.currentSelectedGameObject, gameObject);
        // Keeps last selected gameobject
        if (eventSys.currentSelectedGameObject != null &&
            eventSys.currentSelectedGameObject != lastSelectedGameObject)
        {
            lastSelectedGameObject = eventSys.currentSelectedGameObject;
        }
        // If the button is null, it selects the last selected button
        if (eventSys.currentSelectedGameObject == null)
        {
            eventSys.SetSelectedGameObject(lastSelectedGameObject);
        }

    }

    /// <summary>
    /// Starts coroutine to select parent button after clicking an arrow with a controller.
    /// </summary>
    public void StartCoroutineSelectLastButtonBeforeArrow()
    {
        // Gets current button selected (ARROW)
        GameObject currentButton = eventSys.currentSelectedGameObject;
        // Selects the parent button of this arrow IF the player is controlling it with a controller/keyboard
        StartCoroutine(SelectLastButtonBeforeArrow(currentButton.transform.gameObject));
    }
    private IEnumerator SelectLastButtonBeforeArrow(GameObject previousButton)
    {
        yield return new WaitForEndOfFrame();
        eventSys.SetSelectedGameObject(previousButton);
    }


    public void StartCoroutineDeselect()
    {
        // Gets current button selected (ARROW)
        GameObject currentButton = eventSys.currentSelectedGameObject;
        // Selects the parent button of this arrow IF the player is controlling it with a controller/keyboard
        StartCoroutine(DeselectLastButtonBeforeArrow(currentButton.transform.gameObject));
    }
    private IEnumerator DeselectLastButtonBeforeArrow(GameObject previousButton)
    {
        yield return new WaitForFixedUpdate();
        eventSys.SetSelectedGameObject(null);
        SelectMenuOption();
    }
}
