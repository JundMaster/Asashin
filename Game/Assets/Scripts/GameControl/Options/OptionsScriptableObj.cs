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

    [SerializeField] private int maxDifficulty;
    public int MaxDifficulty => maxDifficulty;


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


    public float SoundVolume { get; set; }

    [Header("Sound Options")]
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
        PlayerPrefs.SetInt(GameOptionsEnum.ScreenMode.ToString(), ScreenMode);
        PlayerPrefs.SetInt(GameOptionsEnum.ScreenResolution.ToString(), ScreenResolution);
        PlayerPrefs.SetInt(GameOptionsEnum.Difficulty.ToString(), Difficulty);
        PlayerPrefs.SetInt(GameOptionsEnum.GraphicsQuality.ToString(), GraphicsQuality);
        PlayerPrefs.SetInt(GameOptionsEnum.ShadowQuality.ToString(), ShadowQuality);
        PlayerPrefs.SetString(GameOptionsEnum.Shadows.ToString(), Shadows.ToString());
        PlayerPrefs.SetString(GameOptionsEnum.AfterImages.ToString(), AfterImages.ToString());
        PlayerPrefs.SetString(GameOptionsEnum.MotionBlur.ToString(), MotionBlur.ToString());
        PlayerPrefs.SetFloat(GameOptionsEnum.Lightness.ToString(), Lightness);
        PlayerPrefs.SetFloat(GameOptionsEnum.Contrast.ToString(), Contrast);
        PlayerPrefs.SetFloat(GameOptionsEnum.SoundVolume.ToString(), SoundVolume);
        PlayerPrefs.SetFloat(GameOptionsEnum.MusicVolume.ToString(), MusicVolume);
        PlayerPrefs.SetFloat(GameOptionsEnum.HorizontalSensibility.ToString(), HorizontalSensibility);
        PlayerPrefs.SetFloat(GameOptionsEnum.VerticalSensibility.ToString(), VerticalSensibility);
    }

    public void LoadConfig()
    {
        AutoLock = bool.Parse(PlayerPrefs.GetString(GameOptionsEnum.AutoLock.ToString(), defaultAutoLock.ToString()));
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
        SoundVolume = PlayerPrefs.GetFloat(GameOptionsEnum.SoundVolume.ToString(), defaultSoundVolume);
        MusicVolume = PlayerPrefs.GetFloat(GameOptionsEnum.MusicVolume.ToString(), defaultMusicVolume);
        HorizontalSensibility = PlayerPrefs.GetFloat(GameOptionsEnum.HorizontalSensibility.ToString(), defaultHorizontalSensibility);
        VerticalSensibility = PlayerPrefs.GetFloat(GameOptionsEnum.VerticalSensibility.ToString(), defaultVerticalSensibility);
    }

    /// <summary>
    /// Resets all options.
    /// </summary>
    public void ResetOptions()
    {
        AutoLock = defaultAutoLock;
        ScreenMode = defaultScreenMode;
        ScreenResolution = defaultScreenResolution;
        Difficulty = defaultDifficulty;
        GraphicsQuality = defaultGraphicsQuality;
        ShadowQuality = defaultShadowQuality;
        Shadows = defaultShadows;
        AfterImages = defaultAfterImages;
        MotionBlur = defaultMotionBlur;
        Lightness = defaultLightness;
        Contrast = defaultContrast;
        SoundVolume = defaultSoundVolume;
        MusicVolume = defaultMusicVolume;
        HorizontalSensibility = defaultHorizontalSensibility;
        VerticalSensibility = defaultVerticalSensibility;
    }

    public void ResetGeneralOptions()
    {
        AutoLock = defaultAutoLock;
        ScreenMode = defaultScreenMode;
        ScreenResolution = defaultScreenResolution;
        Difficulty = defaultDifficulty;
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
        SoundVolume = defaultSoundVolume;
        MusicVolume = defaultMusicVolume;
    }

    public void ResetControlsOptions()
    {
        HorizontalSensibility = defaultHorizontalSensibility;
        VerticalSensibility = defaultVerticalSensibility;
    }
}
