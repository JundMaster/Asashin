using UnityEngine;

/// <summary>
/// TEMPORARY
/// Class responsible for activating and deactivating options menu.
/// </summary>
public class ShowOptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private PauseSystem pause;

    private void Awake()
    {
        pause = FindObjectOfType<PauseSystem>();
    }

    private void OnEnable()
    {
        pause.GamePaused += ControlPause;
    }

    private void OnDisable()
    {
        pause.GamePaused -= ControlPause;
    }

    private void ControlPause(PauseSystemEnum pause)
    {
        if (pause == PauseSystemEnum.Paused)
            pauseMenu.SetActive(true);
        else
            pauseMenu.SetActive(false);
    }
}
