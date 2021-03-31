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
