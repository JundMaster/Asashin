using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

/// <summary>
/// Class responsible for updating options.
/// This class works together with GameOptions.cs and UIOptions.cs.
/// Uses a kind of sandbox pattern.
/// </summary>
public class Options : MonoBehaviour
{
    public OptionsTemporaryValues SavedValues { get; private set; }

    [Header("Game Configuration")]
    [SerializeField] private OptionsScriptableObj options;

    [Header("Post Process")]
    [SerializeField] private Volume postProcess;

    [Header("Quality Settings")]
    [SerializeField] private UniversalRenderPipelineAsset[] qualitySettingsNoShadows;
    [SerializeField] private UniversalRenderPipelineAsset[] qualitySettingsLowShadows;
    [SerializeField] private UniversalRenderPipelineAsset[] qualitySettingsMediumShadows;
    [SerializeField] private UniversalRenderPipelineAsset[] qualitySettingsHighShadows;

    /// <summary>
    /// Creates or loads a config file.
    /// Initialized on singleton
    /// </summary>
    #region Singleton
    public static Options instance = null;
    private void Awake()
    {
        // When the game begins a new session
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

            // Updates options values to player last config with options
            // If the values don't exist yet, it gives them default values
            options.LoadConfig();

            // Initializes a struct with all current values from options
            CreateNewStructWithSavedValues();
            UpdateConfiguration();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    /// <summary>
    /// Every time a scene loads, this script updates the current post process volume.
    /// </summary>
    /// <param name="scene">Null.</param>
    /// <param name="mode">Null.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CurrentLevelDefinitions levelDefinitions = FindObjectOfType<CurrentLevelDefinitions>();
        postProcess.profile = levelDefinitions.ThisArea.PostProcess;

        UpdateConfiguration();
    }

    /// <summary>
    /// Creates a new copy of the saved values.
    /// </summary>
    public void CreateNewStructWithSavedValues() =>
        SavedValues = new OptionsTemporaryValues(options);

    /// <summary>
    /// Updates post process values.
    /// </summary>
    private void UpdatePostProcess()
    {
        if (postProcess.profile.TryGet(out ColorAdjustments colorAdjustement))
        {
            colorAdjustement.postExposure.value = options.Lightness;
            colorAdjustement.contrast.value = options.Contrast;
        }

        if (postProcess.profile.TryGet(out MotionBlur motionBlur))
        {
            motionBlur.active = options.MotionBlur;
        }
    }

    /// <summary>
    /// Updates options scriptable object to the same values as the struct receives values struct.
    /// Saves current options.
    /// </summary>
    /// <param name="value">Struct with values.</param>
    public void UpdateValues(OptionsTemporaryValues value)
    {
        options.AutoLock = value.AutoLock;
        options.ScreenMode = value.ScreenMode;
        options.ScreenResolution = value.ScreenResolution;
        options.GraphicsQuality = value.GraphicsQuality;
        options.ShadowQuality = value.ShadowQuality;
        options.Shadows = value.Shadows;
        options.AfterImages = value.AfterImages;
        options.MotionBlur = value.MotionBlur;
        options.Lightness = value.Lightness;
        options.Contrast = value.Contrast;
        options.MasterVolume = value.MasterVolume;
        options.SoundVolume = value.SoundVolume;
        options.MusicVolume = value.MusicVolume;
        options.HorizontalSensibility = value.HorizontalSensibility;
        options.VerticalSensibility = value.VerticalSensibility;

        // Copies the script received
        SavedValues = value;
        options.SaveConfig();
        UpdateConfiguration();
    }

    /// <summary>
    /// Only updates sound options in real time.
    /// </summary>
    /// <param name="value"></param>
    public void UpdateSoundInstantlyWithSliders(TypeOfAudio type, float value) =>
        OnUpdateAudioRealTime(type, value);

    /// <summary>
    /// Updates all configs.
    /// </summary>
    private void UpdateConfiguration()
    {
        UpdateWindowMode();
        UpdateScreenResolution();
        UpdateQualitySettings();
        UpdatePostProcess();
        OnUpdatedValues();
    }

    /// <summary>
    /// Updates quality settings.
    /// </summary>
    private void UpdateQualitySettings()
    {
        // Uses assets with shadows on
        if (options.Shadows == true)
        {
            QualitySettings.SetQualityLevel(options.GraphicsQuality);
            switch (options.ShadowQuality)
            {
                case 0:
                    QualitySettings.renderPipeline =
                        qualitySettingsLowShadows[options.GraphicsQuality];
                    break;
                case 1:
                    QualitySettings.renderPipeline =
                        qualitySettingsMediumShadows[options.GraphicsQuality];
                    break;
                case 2:
                    QualitySettings.renderPipeline =
                        qualitySettingsHighShadows[options.GraphicsQuality];
                    break;
            }

        }
        // Uses assets with shadows off
        else if (options.Shadows == false)
        {
            QualitySettings.SetQualityLevel(options.GraphicsQuality);
            QualitySettings.renderPipeline =
                qualitySettingsNoShadows[options.GraphicsQuality];
        }
    }

    /// <summary>
    /// Updates screen resolution.
    /// </summary>
    private void UpdateScreenResolution()
    {
        switch (options.ScreenResolution)
        {
            case 0:
                if (options.ScreenMode == 1 || options.ScreenMode == 2)
                    Screen.SetResolution(1280, 720, true);
                else
                    Screen.SetResolution(1280, 720, false);
                break;
            case 1:
                if (options.ScreenMode == 1 || options.ScreenMode == 2)
                    Screen.SetResolution(1600, 900, true);
                else
                    Screen.SetResolution(1600, 900, false);
                break;
        }
    }

    /// <summary>
    /// Updates window mode
    /// </summary>
    private void UpdateWindowMode()
    {
        FullScreenMode windowMode = default;
        switch (options.ScreenMode)
        {
            case 0:
                windowMode = FullScreenMode.Windowed;
                break;
            case 1:
                windowMode = FullScreenMode.FullScreenWindow;
                break;
        }
        Screen.fullScreenMode = windowMode;
    }


    protected virtual void OnUpdatedValues() => UpdatedValues?.Invoke();

    /// <summary>
    /// Registered on CinemachineTarget.
    /// Event registered on AudioController. 
    /// </summary>
    public event Action UpdatedValues;

    protected virtual void OnUpdateAudioRealTime(TypeOfAudio type, float value) => 
        UpdateAudioRealTime?.Invoke(type, value);

    /// <summary>
    /// Event registered on AudioController.
    /// </summary>
    public event Action<TypeOfAudio, float> UpdateAudioRealTime;
}
