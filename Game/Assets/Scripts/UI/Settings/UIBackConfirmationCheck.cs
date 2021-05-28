using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class responsible for setting active Back button confirmation.
/// </summary>
public class UIBackConfirmationCheck : MonoBehaviour
{
    [Header("Confirmation Menu")]
    [SerializeField] private GameObject confirmationToSetActive;
    [SerializeField] private GameObject confirmationButtonToSelect;

    [Header("Menus to deactivate in order to go back")]
    [SerializeField] private GameObject parentOfBackButtonToDeactivate;
    [SerializeField] private GameObject noConfirmationButtonToSelect;
    [SerializeField] private GameObject backButtonFromSettingsMenu;

    // Components
    private EventSystem eventSys;
    private UIOptions uiOptions;

    private void Awake()
    {
        eventSys = FindObjectOfType<EventSystem>();
        uiOptions = FindObjectOfType<UIOptions>();
    }

    public void BackConfirmationIfValuesAreDifferent()
    {
        // Current values are equal, so it doesn't need to set confirmation active
        if (uiOptions != null)
        {
            if (uiOptions.CompareCurrentValues())
            {
                backButtonFromSettingsMenu.SetActive(true);
                eventSys.SetSelectedGameObject(noConfirmationButtonToSelect);
                parentOfBackButtonToDeactivate.SetActive(false);
            }
            // Current values are not equal, so it needs to set confirmation active.
            else
            {
                confirmationToSetActive.SetActive(true);
                eventSys.SetSelectedGameObject(confirmationButtonToSelect);
            }
        } 
    }
}
