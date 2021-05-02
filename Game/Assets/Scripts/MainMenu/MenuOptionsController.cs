using UnityEngine;

/// <summary>
/// Class responsible for handling menu actions.
/// </summary>
public class MenuOptionsController : MonoBehaviour
{
    [Header("Main menu buttons")]
    [SerializeField] private GameObject newGameOption;
    [SerializeField] private GameObject continueOption;
    [SerializeField] private GameObject continueOptionNonSelectable;
    [SerializeField] private GameObject optionsOption;
    [SerializeField] private GameObject quitOption;

    private SpawnerController options;
    private CameraController vmController;

    private void Awake()
    {
        vmController = FindObjectOfType<CameraController>();
        options = FindObjectOfType<SpawnerController>();
    }

    public void SwitchMenuOption()
    {
        if (vmController.IsNewGameCamActive)
        {
            newGameOption.SetActive(true);
            continueOption.SetActive(false);
            continueOptionNonSelectable.SetActive(false);
            optionsOption.SetActive(false);
            quitOption.SetActive(false);
        }

        else if (vmController.IsContinueCamActive)
        {
            newGameOption.SetActive(false);
            optionsOption.SetActive(false);
            quitOption.SetActive(false);
            if (options.GameState.FileExists(FilePath.SAVEFILECHECKPOINT))
            {
                continueOption.SetActive(true);
                continueOptionNonSelectable.SetActive(false);
            }
            else
            {
                continueOptionNonSelectable.SetActive(true);
                continueOption.SetActive(false);
            }
        }

        else if (vmController.IsOptionCamActive)
        {
            newGameOption.SetActive(false);
            continueOption.SetActive(false);
            continueOptionNonSelectable.SetActive(false);
            optionsOption.SetActive(true);
            quitOption.SetActive(false);
        }

        else
        {
            newGameOption.SetActive(false);
            continueOption.SetActive(false);
            continueOptionNonSelectable.SetActive(false);
            optionsOption.SetActive(false);
            quitOption.SetActive(true);
        }
    }
}
