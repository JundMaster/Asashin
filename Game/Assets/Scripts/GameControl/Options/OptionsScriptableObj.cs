using UnityEngine;

/// <summary>
/// Class responsible for creationg a scriptable object with game options.
/// </summary>
[CreateAssetMenu(fileName = "Game Options")]
public class OptionsScriptableObj : ScriptableObject
{
    public bool AutoLock { get; set; }

    [Header("Game Options")]
    [SerializeField] private bool defaultAutoLock;

    public bool EnemyVisionCones { get; set; }

    [SerializeField] private bool defaultEnemyVisionCones;

    public int ScreenMode { get; set; }

    [SerializeField] private int defaultScreenMode;

    [SerializeField] private int maxScreenMode;
    public int MaxScreenMode => maxScreenMode;


    public int ScreenResolution { get; set; }

    [SerializeField] private int defaultScreenResolution;

    [SerializeField] private int maxScreenResolution;
    public int MaxScreenResolution => maxScreenResolution;


    public int Difficulty { get; set; }

    [SerializeField] private int defaultDifficulty;


    public int GraphicsQuality { get; set; }

    [Header("Graphic Options")]
    [SerializeField] private int defaultGraphicsQuality;

    [SerializeField] private int maxGraphicsQuality;
    public int MaxGraphicsQuality => maxGraphicsQuality;


    public int ShadowQuality { get; set; }

    [SerializeField] private int defaultShadowQuality;

    [SerializeField] private int maxShadowQuality;
    public int MaxShadowQuality => maxShadowQuality;


    public bool Shadows { get; set; }

    [SerializeField] private bool defaultShadows;


    public bool AfterImages { get; set; }

    [SerializeField] private bool defaultAfterImages;


    public bool MotionBlur { get; set; }

    [SerializeField] private bool defaultMotionBlur;


    public float Lightness { get; set; }

    [SerializeField] private float defaultLightness;

    [SerializeField] private float minLightness;
    public float MinLightness => minLightness;

    [SerializeField] private float maxLightness;
    public float MaxLightness => maxLightness;


    public float Contrast { get; set; }

    [SerializeField] private float defaultContrast;

    [SerializeField] private float minContrast;
    public float MinContrast => minContrast;

    [SerializeField] private float maxContrast;
    public float MaxContrast => maxContrast;


    public float MasterVolume { get; set; }

    [Header("Sound Options")]
    [SerializeField] private float defaultMasterVolume;

    [SerializeField] private float minMasterVolume;
    public float MinMasterVolume => minMasterVolume;

    [SerializeField] private float maxMasterVolume;
    public float MaxMasterVolume => maxMasterVolume;


    public float SoundVolume { get; set; }

    [SerializeField] private float defaultSoundVolume;

    [SerializeField] private float minSoundVolume;
    public float MinSoundVolume => minSoundVolume;

    [SerializeField] private float maxSoundVolume;
    public float MaxSoundVolume => maxSoundVolume;


    public float MusicVolume { get; set; }

    [SerializeField] private float defaultMusicVolume;

    [SerializeField] private float minMusicVolume;
    public float MinMusicVolume => minMusicVolume;

    [SerializeField] private float maxMusicVolume;
    public float MaxMusicVolume => maxMusicVolume;


    public float HorizontalSensibility { get; set; }

    [Header("Controls")]
    [SerializeField] private float defaultHorizontalSensibility;

    [SerializeField] private float minHorizontalSensibility;
    public float MinHorizontalSensibility => minHorizontalSensibility;

    [SerializeField] private float maxHorizontalSensibility;
    public float MaxHorizontalSensibility => maxHorizontalSensibility;


    public float VerticalSensibility { get; set; }

    [SerializeField] private float defaultVerticalSensibility;

    [SerializeField] private float minVerticalSensibility;
    public float MinVerticalSensibility => minVerticalSensibility;

    [SerializeField] private float maxVerticalSensibility;
    public float MaxVerticalSensibility => maxVerticalSensibility;

    public void SaveConfig()
    {
        PlayerPrefs.SetString(GameOptionsEnum.AutoLock.ToString(), AutoLock.ToString());
        PlayerPrefs.SetString(GameOptionsEnum.EnemyVisionCones.ToString(), EnemyVisionCones.ToString());
        PlayerPrefs.SetInt(GameOptionsEnum.ScreenMode.ToString(), ScreenMode);
        PlayerPrefs.SetInt(GameOptionsEnum.ScreenResolution.ToString(), ScreenResolution);
        PlayerPrefs.SetInt(GameOptionsEnum.GraphicsQuality.ToString(), GraphicsQuality);
        PlayerPrefs.SetInt(GameOptionsEnum.ShadowQuality.ToString(), ShadowQuality);
        PlayerPrefs.SetString(GameOptionsEnum.Shadows.ToString(), Shadows.ToString());
        PlayerPrefs.SetString(GameOptionsEnum.AfterImages.ToString(), AfterImages.ToString());
        PlayerPrefs.SetString(GameOptionsEnum.MotionBlur.ToString(), MotionBlur.ToString());
        PlayerPrefs.SetFloat(GameOptionsEnum.Lightness.ToString(), Lightness);
        PlayerPrefs.SetFloat(GameOptionsEnum.Contrast.ToString(), Contrast);
        PlayerPrefs.SetFloat(GameOptionsEnum.MasterVolume.ToString(), MasterVolume);
        PlayerPrefs.SetFloat(GameOptionsEnum.SoundVolume.ToString(), SoundVolume);
        PlayerPrefs.SetFloat(GameOptionsEnum.MusicVolume.ToString(), MusicVolume);
        PlayerPrefs.SetFloat(GameOptionsEnum.HorizontalSensibility.ToString(), HorizontalSensibility);
        PlayerPrefs.SetFloat(GameOptionsEnum.VerticalSensibility.ToString(), VerticalSensibility);
    }

    public void LoadConfig()
    {
        AutoLock = bool.Parse(PlayerPrefs.GetString(GameOptionsEnum.AutoLock.ToString(), defaultAutoLock.ToString()));
        EnemyVisionCones = bool.Parse(PlayerPrefs.GetString(GameOptionsEnum.EnemyVisionCones.ToString(), defaultEnemyVisionCones.ToString()));
        ScreenMode = PlayerPrefs.GetInt(GameOptionsEnum.ScreenMode.ToString(), defaultScreenMode);
        ScreenResolution = PlayerPrefs.GetInt(GameOptionsEnum.ScreenResolution.ToString(), defaultScreenResolution);
        Difficulty = PlayerPrefs.GetInt(GameOptionsEnum.Difficulty.ToString(), defaultDifficulty);
        GraphicsQuality = PlayerPrefs.GetInt(GameOptionsEnum.GraphicsQuality.ToString(), defaultGraphicsQuality);
        ShadowQuality = PlayerPrefs.GetInt(GameOptionsEnum.ShadowQuality.ToString(), defaultShadowQuality);
        Shadows = bool.Parse(PlayerPrefs.GetString(GameOptionsEnum.Shadows.ToString(), defaultShadows.ToString()));
        AfterImages = bool.Parse(PlayerPrefs.GetString(GameOptionsEnum.AfterImages.ToString(), defaultAfterImages.ToString()));
        MotionBlur = bool.Parse(PlayerPrefs.GetString(GameOptionsEnum.MotionBlur.ToString(), defaultMotionBlur.ToString()));
        Lightness = PlayerPrefs.GetFloat(GameOptionsEnum.Lightness.ToString(), defaultLightness);
        Contrast = PlayerPrefs.GetFloat(GameOptionsEnum.Contrast.ToString(), defaultContrast);
        MasterVolume = PlayerPrefs.GetFloat(GameOptionsEnum.MasterVolume.ToString(), defaultMasterVolume);
        SoundVolume = PlayerPrefs.GetFloat(GameOptionsEnum.SoundVolume.ToString(), defaultSoundVolume);
        MusicVolume = PlayerPrefs.GetFloat(GameOptionsEnum.MusicVolume.ToString(), defaultMusicVolume);
        HorizontalSensibility = PlayerPrefs.GetFloat(GameOptionsEnum.HorizontalSensibility.ToString(), defaultHorizontalSensibility);
        VerticalSensibility = PlayerPrefs.GetFloat(GameOptionsEnum.VerticalSensibility.ToString(), defaultVerticalSensibility);
    }

    /// <summary>
    /// Saves difficulty. Only happens on start new game.
    /// </summary>
    /// <param name="dif">0 for normal, 1 for hard.</param>
    public void SaveDifficulty(int dif)
    {
        if (dif == 0)
        {
            Difficulty = 0;
            PlayerPrefs.SetInt(GameOptionsEnum.Difficulty.ToString(), 0);
        }
        else if (dif == 1)
        {
            Difficulty = 1;
            PlayerPrefs.SetInt(GameOptionsEnum.Difficulty.ToString(), 1);
        }
    }

    public void ResetGeneralOptions()
    {
        AutoLock = defaultAutoLock;
        EnemyVisionCones = defaultEnemyVisionCones;
        ScreenMode = defaultScreenMode;
        ScreenResolution = defaultScreenResolution;
    }

    public void ResetGraphicOptions()
    {
        GraphicsQuality = defaultGraphicsQuality;
        ShadowQuality = defaultShadowQuality;
        Shadows = defaultShadows;
        AfterImages = defaultAfterImages;
        MotionBlur = defaultMotionBlur;
        Lightness = defaultLightness;
        Contrast = defaultContrast;
    }

    public void ResetAudioOptions()
    {
        MasterVolume = defaultMasterVolume;
        SoundVolume = defaultSoundVolume;
        MusicVolume = defaultMusicVolume;
    }

    public void ResetControlsOptions()
    {
        HorizontalSensibility = defaultHorizontalSensibility;
        VerticalSensibility = defaultVerticalSensibility;
    }
}
