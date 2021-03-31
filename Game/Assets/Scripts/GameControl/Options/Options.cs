using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Class responsible for updating options.
/// This class works together with GameOptions.cs and UIOptions.cs.
/// Uses a kind of sandbox pattern.
/// </summary>
public class Options : MonoBehaviour
{
    // Components
    private GameOptions gameOptions;
    public OptionsTemporaryValues SavedValues { get; private set; }

    [SerializeField] private OptionsScriptableObj options;

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
            gameOptions = new GameOptions(options);

            // When the game begins for the first time on a computer
            if (gameOptions.FileExists(FilePath.CONFIG))
            {
                // Updates options values to player last config with options
                gameOptions.LoadConfig();

            }
            // Else if file doesn't exist yet
            else
            {
                // Creates new file
                gameOptions.CreateConfigFile();

                // Resets options to default
                options.ResetOptions();

                // Saves current options
                gameOptions.SaveConfig();
            }
            
            // Initializes a struct with all current values from options
            SavedValues = new OptionsTemporaryValues(options);
            UpdateWindowMode();
            UpdateScreenResolution();
            UpdateQualitySettings();
            UpdateValuesForInterfaces();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////
    
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
        options.Difficulty = value.Difficulty;
        options.GraphicsQuality = value.GraphicsQuality;
        options.ShadowQuality = value.ShadowQuality;
        options.Shadows = value.Shadows;
        options.AfterImages = value.AfterImages;
        options.MotionBlur = value.MotionBlur;
        options.Lightness = value.Lightness;
        options.Contrast = value.Contrast;
        options.SoundVolume = value.SoundVolume;
        options.MusicVolume = value.MusicVolume;
        options.HorizontalSensibility = value.HorizontalSensibility;
        options.VerticalSensibility = value.VerticalSensibility;

        // Copies the script received
        SavedValues = value;
        gameOptions.SaveConfig();
        UpdateWindowMode();
        UpdateScreenResolution();
        UpdateQualitySettings();
        UpdateValuesForInterfaces();
    }

    /// <summary>
    /// Updates values for every class that implements IUpdateOptions.
    /// </summary>
    public void UpdateValuesForInterfaces()
    {
        SceneControl sceneController = FindObjectOfType<SceneControl>();
        GameObject[] rootGameObjects = sceneController.CurrentScene().GetRootGameObjects();
        foreach (GameObject rootGameObject in rootGameObjects)
        {
            IUpdateOptions[] childrenInterfaces =
                rootGameObject.GetComponentsInChildren<IUpdateOptions>();

            foreach (IUpdateOptions childInterface in childrenInterfaces)
            {
                childInterface.UpdateValues();
            }
        }
    }

    /// <summary>
    /// Updates quality settings.
    /// </summary>
    public void UpdateQualitySettings()
    {
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
    public void UpdateScreenResolution()
    {

        switch (options.ScreenResolution)
        {
            case 0:
                if (options.ScreenResolution == 1 || options.ScreenResolution == 2)
                    Screen.SetResolution(1280, 720, true);
                else
                    Screen.SetResolution(1280, 720, false);
                break;
            case 1:
                if (options.ScreenResolution == 1 || options.ScreenResolution == 2)
                    Screen.SetResolution(1600, 900, true);
                else
                    Screen.SetResolution(1600, 900, false);
                break;
            case 2:
                if (options.ScreenResolution == 1 || options.ScreenMode == 2)
                    Screen.SetResolution(1920, 1080, true);
                else
                    Screen.SetResolution(1920, 1080, false);
                break;
        }

    }

    /// <summary>
    /// Updates window mode
    /// </summary>
    public void UpdateWindowMode()
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
            case 2:
                windowMode = FullScreenMode.ExclusiveFullScreen;
                break;
        }
        Screen.fullScreenMode = windowMode;
    }

}
