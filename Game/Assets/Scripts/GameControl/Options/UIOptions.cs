using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System;

/// <summary>
/// Class responsible for controlling UIOptions. Updates values on ui
/// and values on Options.cs.
/// </summary>
public class UIOptions : MonoBehaviour
{
    [SerializeField] private GameObject initialSelectedButton;
    [SerializeField] private OptionsScriptableObj configScriptableObj;

    [Header("General Options")]
    [SerializeField] private TextMeshProUGUI screenModeText;
    [SerializeField] private TextMeshProUGUI screenResText;
    [SerializeField] private TextMeshProUGUI autoLockText;
    [SerializeField] private TextMeshProUGUI difficultyText;

    [Header("Graphic Options")]
    [SerializeField] private TextMeshProUGUI graphicsQualityText;
    [SerializeField] private TextMeshProUGUI shadowQualityText;
    [SerializeField] private TextMeshProUGUI shadowsText;
    [SerializeField] private TextMeshProUGUI afterImagesText;
    [SerializeField] private TextMeshProUGUI motionBlurText;
    [SerializeField] private Slider lightness;
    [SerializeField] private Slider contrast;

    [Header("Sound Options")]
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider soundVolume;

    [Header("Control Options")]
    [SerializeField] private Slider verticalSensiblity;
    [SerializeField] private Slider horizontalSensiblity;

    // Components
    private Options optionsScript;
    private OptionsTemporaryValues currentValues;
    private EventSystem eventSys;
    private GameObject lastSelectedGameObject;

    private void Awake()
    {
        optionsScript = FindObjectOfType<Options>();
        eventSys = FindObjectOfType<EventSystem>();
    }

    /// <summary>
    /// Updates all UI
    /// </summary>
    /// <returns></returns>
    private void OnEnable()
    {
        eventSys.SetSelectedGameObject(initialSelectedButton);

        currentValues = optionsScript.SavedValues;

        // Updates current values
        UpdateAllUI();

        // Sets min and max values on sliders
        SetMinAndMaxSliderValues();

        // Updates current values again
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
    /// values and saves those values in a file. Also updates UI.
    /// </summary>
    public void AcceptValues()
    {
        optionsScript.UpdateValues(currentValues);
        UpdateAllUI();
    }

    /// <summary>
    /// Reverts all changed values to the current values before altering them
    /// to these new ones.
    /// </summary>
    public void RevertValues()
    {
        currentValues = optionsScript.SavedValues;
        UpdateAllUI();
    }

    /// <summary>
    /// Compares if the current values are the same as the saved values.
    /// </summary>
    public bool CompareCurrentValues()
    {
        if (currentValues.Equals(optionsScript.SavedValues))
        {
            return true;
        }
        return false;
    }

    #region Reset values
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Resets options, creates new copies of the new values, updates all options and UI.
    /// </summary>
    public void ResetGeneralValues()
    {
        configScriptableObj.ResetGeneralOptions();  // Resets options
        optionsScript.CreateNewStructWithSavedValues(); // Creates a new struct with the new values
        currentValues = optionsScript.SavedValues;  // Creates a copy of the new values
        AcceptValues(); // Updates values and updates UI
    }
    /// <summary>
    /// Resets options, creates new copies of the new values, updates all options and UI.
    /// </summary>
    public void ResetGraphicValues()
    {
        configScriptableObj.ResetGraphicOptions();  // Resets options
        optionsScript.CreateNewStructWithSavedValues(); // Creates a new struct with the new values
        currentValues = optionsScript.SavedValues;  // Creates a copy of the new values
        AcceptValues(); // Updates values and updates UI
    }
    /// <summary>
    /// Resets options, creates new copies of the new values, updates all options and UI.
    /// </summary>
    public void ResetAudioValues() 
    {
        configScriptableObj.ResetAudioOptions();  // Resets options
        optionsScript.CreateNewStructWithSavedValues(); // Creates a new struct with the new values
        currentValues = optionsScript.SavedValues;  // Creates a copy of the new values
        AcceptValues(); // Updates values and updates UI
    }
    /// <summary>
    /// Resets options, creates new copies of the new values, updates all options and UI.
    /// </summary>
    public void ResetControlValues()
    {
        configScriptableObj.ResetControlsOptions();  // Resets options
        optionsScript.CreateNewStructWithSavedValues(); // Creates a new struct with the new values
        currentValues = optionsScript.SavedValues;  // Creates a copy of the new values
        AcceptValues(); // Updates values and updates UI
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////

    #region Updates strings with arrows
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Decrements a certain value.
    /// </summary>
    /// <param name="option">Value to decrement.</param>
    public void LeftButton(string option)
    {
        switch (option)
        {
            case "AutoLock":
                if (currentValues.AutoLock)
                {
                    currentValues.AutoLock = false;
                }
                else
                {
                    currentValues.AutoLock = true;
                }
                break;

            case "ScreenMode":
                if (currentValues.ScreenMode - 1 >= 0) currentValues.ScreenMode--;
                else currentValues.ScreenMode = configScriptableObj.MaxScreenMode;
                break;

            case "ScreenResolution":
                if (currentValues.ScreenResolution - 1 >= 0) currentValues.ScreenResolution--;
                else currentValues.ScreenResolution = configScriptableObj.MaxScreenRes;
                break;

            case "Difficulty":
                if (currentValues.Difficulty - 1 >= 0) currentValues.Difficulty--;
                else currentValues.Difficulty = configScriptableObj.MaxDifficulty;
                break;

            case "GraphicsQuality":
                if (currentValues.GraphicsQuality - 1 >= 0) currentValues.GraphicsQuality--;
                else currentValues.GraphicsQuality = configScriptableObj.MaxGraphicsQuality;
                break;

            case "ShadowQuality":
                if (currentValues.ShadowQuality - 1 >= 0) currentValues.ShadowQuality--;
                else currentValues.ShadowQuality = configScriptableObj.MaxShadowQuality;
                break;

            case "Shadows":
                if (currentValues.Shadows)
                {
                    currentValues.Shadows = false;
                }
                else
                {
                    currentValues.Shadows = true;
                }
                break;

            case "AfterImages":
                if (currentValues.AfterImages)
                {
                    currentValues.AfterImages = false;
                }
                else
                {
                    currentValues.AfterImages = true;
                }
                break;

            case "MotionBlur":
                if (currentValues.MotionBlur)
                {
                    currentValues.MotionBlur = false;
                }
                else
                {
                    currentValues.MotionBlur = true;
                }
                break;
        }
        UpdateAllUI();
    }

    /// <summary>
    /// Increments a certain value.
    /// </summary>
    /// <param name="option">Value to increment.</param>
    public void RightButton(string option)
    {
        switch (option)
        {
            case "AutoLock":
                if (currentValues.AutoLock)
                {
                    currentValues.AutoLock = false;
                }
                else
                {
                    currentValues.AutoLock = true;
                }
                break;

            case "ScreenMode":
                if (currentValues.ScreenMode + 1 <= configScriptableObj.MaxScreenMode) currentValues.ScreenMode++;
                else currentValues.ScreenMode = 0;
                break;

            case "ScreenResolution":
                if (currentValues.ScreenResolution + 1 <= configScriptableObj.MaxScreenRes) currentValues.ScreenResolution++;
                else currentValues.ScreenResolution = 0;
                break;

            case "Difficulty":
                if (currentValues.Difficulty + 1 <= configScriptableObj.MaxDifficulty) currentValues.Difficulty++;
                else currentValues.Difficulty = 0;
                break;

            case "GraphicsQuality":
                if (currentValues.GraphicsQuality + 1 <= configScriptableObj.MaxGraphicsQuality) currentValues.GraphicsQuality++;
                else currentValues.GraphicsQuality = 0;
                break;

            case "ShadowQuality":
                if (currentValues.ShadowQuality + 1 <= configScriptableObj.MaxShadowQuality) currentValues.ShadowQuality++;
                else currentValues.ShadowQuality = 0;
                break;

            case "Shadows":
                if (currentValues.Shadows)
                {
                    currentValues.Shadows = false;
                }
                else
                {
                    currentValues.Shadows = true;
                }
                break;

            case "AfterImages":
                if (currentValues.AfterImages)
                {
                    currentValues.AfterImages = false;
                }
                else
                {
                    currentValues.AfterImages = true;
                }
                break;

            case "MotionBlur":
                if (currentValues.MotionBlur)
                {
                    currentValues.MotionBlur = false;
                }
                else
                {
                    currentValues.MotionBlur = true;
                }
                break;
        }
        UpdateAllUI();
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////

    #region Methods called from sliders
    ////////////////////////////////////////////////////////////////////////////
    public void UpdateLightnessValue(float value) => currentValues.Lightness = value;
    public void UpdateContrastValue(float value) => currentValues.Contrast = value;
    public void UpdateMusicVolume(float value) => currentValues.MusicVolume = value;
    public void UpdateSoundVolume(float value) => currentValues.SoundVolume = value;
    public void VerticalSensiblity(float value) => currentValues.VerticalSensibility = value;
    public void HorizontalSensiblity(float value) => currentValues.HorizontalSensibility = value;

    #endregion
    ////////////////////////////////////////////////////////////////////////////

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
    /// Updates all UI.
    /// </summary>
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
                screenModeText.text = "Borderless";
                break;
            case 2:
                screenModeText.text = "Fullscreen";
                break;
        }

        switch (currentValues.ScreenResolution)
        {
            case 0:
                screenResText.text = "1280x720";
                break;
            case 1:
                screenResText.text = "1600x900";
                break;
            case 2:
                screenResText.text = "1920x1080";
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
        switch (currentValues.Shadows)
        {
            case true:
                shadowsText.text = "On";
                break;
            case false:
                shadowsText.text = "Off";
                break;
        }
        switch (currentValues.AfterImages)
        {
            case true:
                afterImagesText.text = "On";
                break;
            case false:
                afterImagesText.text = "Off";
                break;
        }
        switch (currentValues.MotionBlur)
        {
            case true:
                motionBlurText.text = "On";
                break;
            case false:
                motionBlurText.text = "Off";
                break;
        }

        lightness.value = currentValues.Lightness;
        contrast.value = currentValues.Contrast;
        musicVolume.value = currentValues.MusicVolume;
        soundVolume.value = currentValues.SoundVolume;
        verticalSensiblity.value = currentValues.VerticalSensibility;
        horizontalSensiblity.value = currentValues.HorizontalSensibility;
    }

    /// <summary>
    /// Sets slider values to min and max from options scriptable object.
    /// </summary>
    private void SetMinAndMaxSliderValues()
    {
        lightness.minValue = configScriptableObj.MinLightness;
        lightness.maxValue = configScriptableObj.MaxLightness;
        contrast.minValue = configScriptableObj.MinContrast;
        contrast.maxValue = configScriptableObj.MaxContrast;
        musicVolume.minValue = configScriptableObj.MinMusicVolume;
        musicVolume.maxValue = configScriptableObj.MaxMusicVolume;
        soundVolume.minValue = configScriptableObj.MinSoundVolume;
        soundVolume.maxValue = configScriptableObj.MaxSoundVolume;
        verticalSensiblity.minValue = configScriptableObj.MinVerticalSensibility;
        verticalSensiblity.maxValue = configScriptableObj.MaxVerticalSensibility;
        horizontalSensiblity.minValue = configScriptableObj.MinHorizontalSensibility;
        horizontalSensiblity.maxValue = configScriptableObj.MaxHorizontalSensibility;
    }
}
