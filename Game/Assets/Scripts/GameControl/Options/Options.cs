using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Class responsible for updating options.
/// This class works with GameOptions.cs and UIOptions.cs.
/// Uses a kind of sandbox pattern.
/// </summary>
public class Options : MonoBehaviour
{
    // Initialized on singleton
    private GameOptions gameOptions;
    public OptionsTemporaryValues CurrentValues { get; private set; }

    [SerializeField] private OptionsScriptableObj options;

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
            CurrentValues = new OptionsTemporaryValues(options);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    ////////////////////////////////////////////////////////////////////////////
    
    /// <summary>
    /// Updates options scriptable obj to the same values as a temp values struct.
    /// </summary>
    /// <param name="value">Struct with values.</param>
    public void UpdateValues(OptionsTemporaryValues value)
    {
        options.AutoLock = value.AutoLock;
        options.ScreenMode = value.ScreenMode;
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

        gameOptions.SaveConfig();
    }
}
