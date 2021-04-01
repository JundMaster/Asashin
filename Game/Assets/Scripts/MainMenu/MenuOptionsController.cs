using UnityEngine;

/// <summary>
/// Class responsible for handling menu actions.
/// </summary>
public class MenuOptionsController : MonoBehaviour
{
    [SerializeField]
    private GameObject newGameOption;

    [SerializeField]
    private GameObject continueOption;

    [SerializeField]
    private GameObject optionsOption;

    [SerializeField]
    private GameObject quitOption;

    private CameraController vmController;

    private void Awake()
    {
        vmController = FindObjectOfType<CameraController>();
    }

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SwitchMenuOption()
    {
        if (vmController.IsNewGameCamActive)
        {
            newGameOption.SetActive(true);
            continueOption.SetActive(false);
            optionsOption.SetActive(false);
            quitOption.SetActive(false);
        }

        else if (vmController.IsContinueCamActive)
        {
            newGameOption.SetActive(false);
            continueOption.SetActive(true);
            optionsOption.SetActive(false);
            quitOption.SetActive(false);
        }

        else if (vmController.IsOptionCamActive)
        {
            newGameOption.SetActive(false);
            continueOption.SetActive(false);
            optionsOption.SetActive(true);
            quitOption.SetActive(false);
        }

        else
        {
            newGameOption.SetActive(false);
            continueOption.SetActive(false);
            optionsOption.SetActive(false);
            quitOption.SetActive(true);
        }
    }
}
