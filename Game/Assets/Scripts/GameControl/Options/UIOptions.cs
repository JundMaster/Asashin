using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

/// <summary>
/// Class responsible for controlling UIOptions. Updates values on ui
/// and values on Options.cs.
/// </summary>
public class UIOptions : MonoBehaviour
{
    [SerializeField] private GameObject initialSelectedButton;
    [SerializeField] private OptionsScriptableObj defaultOptions;

    [Header("Screen Mode")]
    [SerializeField] private TextMeshProUGUI screenModeText;

    [Header("Auto Lock Mode")]
    [SerializeField] private TextMeshProUGUI autoLockText;


    [Header("Difficulty")]
    [SerializeField] private TextMeshProUGUI difficultyText;

    [Header("Graphics Quality")]
    [SerializeField] private TextMeshProUGUI graphicsQualityText;
    [SerializeField] private TextMeshProUGUI shadowQualityText;

    // Components
    private Options options;
    private OptionsTemporaryValues currentValues;
    private EventSystem eventSys;
    private GameObject lastSelectedGameObject;

    private void Awake()
    {
        options = FindObjectOfType<Options>();
        eventSys = FindObjectOfType<EventSystem>();
        UpdateAllUI();
    }

    /// <summary>
    /// Every time this script is enabled, the values are copied from Options
    /// OptionsTemporaryValues struct, which is a struct with the current
    /// option values.
    /// </summary>
    private void OnEnable()
    {
        currentValues = options.CurrentValues;
        eventSys.SetSelectedGameObject(initialSelectedButton);
        UpdateAllUI();
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

    /// <summary>
    /// Passes a struct with current values to Options class, updates current
    /// values and saves those values in a file.
    /// Copies the options struct again, so it knows the current values.
    /// </summary>
    public void AcceptValues()
    {
        options.UpdateValues(currentValues);
        currentValues = options.CurrentValues;
    }

    /// <summary>
    /// Revers all changed values to the current values before altering them
    /// to these new ones.
    /// </summary>
    public void RevertValues()
    {
        currentValues = options.CurrentValues;
        UpdateAllUI();
    }

    /// <summary>
    /// Decrements a certain value.
    /// </summary>
    /// <param name="option">Value to decrement.</param>
    public void LeftButton(string option)
    {
        Debug.Log("leftButton");
        switch (option)
        {
            case "AutoLock":
                if (currentValues.AutoLock)
                {
                    currentValues.AutoLock = false;
                    UpdateUI(GameOptionsEnum.AutoLock, 1);
                }
                else
                {
                    currentValues.AutoLock = true;
                    UpdateUI(GameOptionsEnum.AutoLock, 0);
                }
                break;
            case "ScreenMode":
                if (currentValues.ScreenMode - 1 >= 0) currentValues.ScreenMode--;
                else currentValues.ScreenMode = defaultOptions.MaxScreenMode;
                UpdateUI(GameOptionsEnum.ScreenMode, currentValues.ScreenMode);
                break;
            case "Difficulty":
                if (currentValues.Difficulty - 1 >= 0) currentValues.Difficulty--;
                else currentValues.Difficulty = defaultOptions.MaxDifficulty;
                UpdateUI(GameOptionsEnum.Difficulty, currentValues.Difficulty);
                break;

            case "Graphics Quality":
                if (currentValues.GraphicsQuality - 1 >= 0) currentValues.GraphicsQuality--;
                else currentValues.GraphicsQuality = defaultOptions.MaxGraphicsQuality;
                UpdateUI(GameOptionsEnum.GraphicsQuality, currentValues.GraphicsQuality);
                break;

            case "Shadow Quality":
                if (currentValues.ShadowQuality - 1 >= 0) currentValues.ShadowQuality--;
                else currentValues.ShadowQuality = defaultOptions.MaxShadowQuality;
                UpdateUI(GameOptionsEnum.ShadowQuality, currentValues.ShadowQuality);
                break;
        }
    }

    /// <summary>
    /// Increments a certain value.
    /// </summary>
    /// <param name="option">Value to increment.</param>
    public void RightButton(string option)
    {
        Debug.Log("rightButton");
        switch (option)
        {
            case "AutoLock":
                if (currentValues.AutoLock)
                {
                    currentValues.AutoLock = false;
                    UpdateUI(GameOptionsEnum.AutoLock, 1);
                }
                else
                {
                    currentValues.AutoLock = true;
                    UpdateUI(GameOptionsEnum.AutoLock, 0);
                }
                break;

            case "ScreenMode":
                if (currentValues.ScreenMode - 1 >= 0) currentValues.ScreenMode--;
                else currentValues.ScreenMode = defaultOptions.MaxScreenMode;
                UpdateUI(GameOptionsEnum.ScreenMode, currentValues.ScreenMode);
                break;

            case "Difficulty":
                if (currentValues.Difficulty + 1 <= defaultOptions.MaxDifficulty) currentValues.Difficulty++;
                else currentValues.Difficulty = 0;
                UpdateUI(GameOptionsEnum.Difficulty, currentValues.Difficulty);
                break;

            case "Graphics Quality":
                if (currentValues.GraphicsQuality + 1 <= defaultOptions.MaxGraphicsQuality) currentValues.GraphicsQuality++;
                else currentValues.GraphicsQuality = 0;
                UpdateUI(GameOptionsEnum.GraphicsQuality, currentValues.GraphicsQuality);
                break;

            case "Shadow Quality":
                if (currentValues.ShadowQuality + 1 <= defaultOptions.MaxShadowQuality) currentValues.ShadowQuality++;
                else currentValues.ShadowQuality = 0;
                UpdateUI(GameOptionsEnum.ShadowQuality, currentValues.ShadowQuality);
                break;
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
        StartCoroutine(SelectLastButtonBeforeArrow(currentButton.transform.parent.gameObject)); 
    }

    /// <summary>
    /// Selects parent button after clicking arrow.
    /// </summary>
    /// <param name="parentButton">Parent button of this arrow.</param>
    /// <returns>Wait for end of frame.</returns>
    private IEnumerator SelectLastButtonBeforeArrow(GameObject parentButton)
    {
        yield return new WaitForEndOfFrame();
        eventSys.SetSelectedGameObject(parentButton);
    }

    /// <summary>
    /// Updates UI corresponding to the current options.
    /// </summary>
    /// <param name="option">What type of option to change.</param>
    /// <param name="number">Current value of that option.</param>
    private void UpdateUI(GameOptionsEnum option, short number)
    {
        switch(option)
        {
            case GameOptionsEnum.AutoLock:
                switch (number)
                {
                    case 0:
                        autoLockText.text = "On";
                        break;
                    case 1:
                        autoLockText.text = "Off";
                        break;
                }
                break;

            case GameOptionsEnum.ScreenMode:
                switch (number)
                {
                    case 0:
                        screenModeText.text = "Windowed";
                        break;
                    case 1:
                        screenModeText.text = "Fullscreen";
                        break;
                    case 2:
                        screenModeText.text = "Borderless";
                        break;
                }
                break;

            case GameOptionsEnum.Difficulty:
                switch(number)
                {
                    case 0:
                        difficultyText.text = "Easy";
                        break;
                    case 1:
                        difficultyText.text = "Medium";
                        break;
                    case 2:
                        difficultyText.text = "Hard";
                        break;
                    case 3:
                        difficultyText.text = "Impossible";
                        break;
                }
                break;

            case GameOptionsEnum.GraphicsQuality:
                switch (number)
                {
                    case 0:
                        graphicsQualityText.text = "Low";
                        break;
                    case 1:
                        graphicsQualityText.text = "Medium";
                        break;
                    case 2:
                        graphicsQualityText.text = "High";
                        break;
                }
                break;

            case GameOptionsEnum.ShadowQuality:
                switch (number)
                {
                    case 0:
                        shadowQualityText.text = "Low";
                        break;
                    case 1:
                        shadowQualityText.text = "Medium";
                        break;
                    case 2:
                        shadowQualityText.text = "High";
                        break;
                }
                break;
        }
    }

    private void UpdateAllUI()
    {
        switch (currentValues.AutoLock)
        {
            case true:
                autoLockText.text = "On";
                break;
            case false:
                autoLockText.text = "Off";
                break;
        }

        switch (currentValues.ScreenMode)
        {
            case 0:
                screenModeText.text = "Windowed";
                break;
            case 1:
                screenModeText.text = "Fullscreen";
                break;
            case 2:
                screenModeText.text = "Borderless";
                break;
        }

        switch (currentValues.Difficulty)
        {
            case 0:
                difficultyText.text = "Easy";
                break;
            case 1:
                difficultyText.text = "Medium";
                break;
            case 2:
                difficultyText.text = "Hardcore";
                break;
            case 3:
                difficultyText.text = "Impossible";
                break;
        }

        switch (currentValues.GraphicsQuality)
        {
            case 0:
                graphicsQualityText.text = "Low";
                break;
            case 1:
                graphicsQualityText.text = "Medium";
                break;
            case 2:
                graphicsQualityText.text = "High";
                break;
        }
        switch (currentValues.ShadowQuality)
        {
            case 0:
                shadowQualityText.text = "Low";
                break;
            case 1:
                shadowQualityText.text = "Medium";
                break;
            case 2:
                shadowQualityText.text = "High";
                break;
        }
    }
}
