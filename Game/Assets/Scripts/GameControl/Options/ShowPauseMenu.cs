using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// TEMPORARY
/// Class responsible for activating and deactivating options menu.
/// </summary>
public class ShowPauseMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenuParent;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    [Header("Buttons to Select")]
    [SerializeField] private GameObject resumeButton;

    private PauseSystem pause;
    private EventSystem eventSys;
    private GameObject lastSelectedGameObject;

    private void Awake()
    {
        pause = FindObjectOfType<PauseSystem>();
        eventSys = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        if (pause != null)
            pause.GamePaused += ControlPause;
    }

    private void OnDisable()
    {
        if (pause != null)
            pause.GamePaused -= ControlPause;
    }

    /// <summary>
    /// Checks if current selected game object is null.
    /// If it's null it selects the last game object selected.
    /// </summary>
    private void Update()
    {
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

    private void ControlPause(PauseSystemEnum pause)
    {
        if (pause == PauseSystemEnum.Paused)
        {
            pauseMenuParent.SetActive(true);
            eventSys.SetSelectedGameObject(resumeButton);
        }
    }

    /// <summary>
    /// Called on Cancel event from event trigger on button.
    /// </summary>
    public void UnpauseGame()
    {
        FindObjectOfType<PauseSystem>().HandlePause(PauseSystemEnum.Unpaused);
        FindObjectOfType<PlayerInputCustom>().SwitchActionMapToGameplay();

        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        pauseMenuParent.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
