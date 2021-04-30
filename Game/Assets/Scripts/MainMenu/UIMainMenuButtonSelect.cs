using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIMainMenuButtonSelect : MonoBehaviour
{
    private EventSystem eventSys;

    [Header("Menu options")]
    [SerializeField] private GameObject newGame;
    [SerializeField] private GameObject continueGame;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject quit;

    /// <summary>
    /// Called on UI arrow.
    /// </summary>
    public void StartCoroutineDeselect()
    {
        // Selects the parent button of this arrow IF the player is controlling
        // it with a controller/keyboard
        StartCoroutine(DeselectLastButtonBeforeArrow());
    }

    /// <summary>
    /// Deselects current button and selects a new one.
    /// Selects a new button depending on which button is currently activated.
    /// </summary>
    /// <returns>Wait for fixed Update.</returns>
    private IEnumerator DeselectLastButtonBeforeArrow()
    {
        yield return new WaitForFixedUpdate();

        // Must find event system here, not in awake
        eventSys = FindObjectOfType<EventSystem>();

        if (newGame.activeSelf)
            eventSys.SetSelectedGameObject(newGame);

        else if (continueGame.activeSelf)
            eventSys.SetSelectedGameObject(continueGame);

        else if (options.activeSelf)
            eventSys.SetSelectedGameObject(options);

        else eventSys.SetSelectedGameObject(quit);
    }
}
