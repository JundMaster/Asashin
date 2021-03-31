using UnityEngine;

/// <summary>
/// Class responsible for creationg a scriptable object with game options.
/// </summary>
[CreateAssetMenu(fileName = "Game Options")]
public class OptionsScriptableObj : ScriptableObject
{
    [Header("Game Options")]
    //AutoLock Option
    [SerializeField] private bool autoLock;
    public bool AutoLock { get => autoLock; set => autoLock = value; }

    [SerializeField] private bool defaultAutoLock;
    public bool DefaultAutoLock => defaultAutoLock;

    //Difficulty Level Options
    [SerializeField] private short difficulty;
    public short Difficulty { get => difficulty; set => difficulty = value; }

    [SerializeField] private short defaultDifficulty;
    public short DefaultDifficulty => defaultDifficulty;

    [SerializeField] private short maxDifficulty;
    public short MaxDifficulty => maxDifficulty;

    [Header("Graphic Options")]
    //Screen Mode Options
    [SerializeField] private short screenMode;
    public short ScreenMode { get => screenMode; set => screenMode = value; }

    [SerializeField] private short defaultScreenMode;
    public short DefaultScreenMode => defaultScreenMode;

    [SerializeField] private short maxScreenMode;
    public short MaxScreenMode => maxScreenMode;


    [SerializeField] private short graphicsQuality;
    public short GraphicsQuality { get => graphicsQuality; set => graphicsQuality = value; }

    [SerializeField] private short defaultGraphicsQuality;
    public short DefaultGraphicsQuality => defaultGraphicsQuality;

    [SerializeField] private short maxGraphicsQuality;
    public short MaxGraphicsQuality => maxGraphicsQuality;

    [SerializeField] private short shadowQuality;
    public short ShadowQuality { get => shadowQuality; set => shadowQuality = value; }

    [SerializeField] private short defaultShadowQuality;
    public short DefaultShadowQuality => defaultShadowQuality;

    [SerializeField] private short maxShadowQuality;
    public short MaxShadowQuality => maxShadowQuality;

    [SerializeField] private bool shadows;
    public bool Shadows { get => shadows; set => shadows = value; }

    [SerializeField] private bool defaultShadows;
    public bool DefaultShadows => defaultShadows;

    [SerializeField] private bool afterImages;
    public bool AfterImages { get => afterImages; set => afterImages = value; }

    [SerializeField] private bool defaultAfterImages;
    public bool DefaultAfterImages => defaultAfterImages;

    [SerializeField] private bool motionBlur;
    public bool MotionBlur { get => motionBlur; set => motionBlur = value; }

    [SerializeField] private bool defaultMotionBlur;
    public bool DefaultMotionBlur => defaultMotionBlur;

    [SerializeField] private float lightness;
    public float Lightness { get => lightness; set => lightness = value; }

    [SerializeField] private float defaultLightness;
    public float DefaultLightness => defaultLightness;

    [SerializeField] private float minLightness;
    public float MinLightness => minLightness;

    [SerializeField] private float maxLightness;
    public float MaxLightness => maxLightness;

    [SerializeField] private float contrast;
    public float Contrast { get => contrast; set => contrast = value; }

    [SerializeField] private float defaultContrast;
    public float DefaultContrast => defaultContrast;

    [SerializeField] private float minContrast;
    public float MinContrast => minContrast;

    [SerializeField] private float maxContrast;
    public float MaxContrast => maxContrast;


    [Header("Sound Options")]
    [SerializeField] private float soundVolume;
    public float SoundVolume { get => soundVolume; set => soundVolume = value; }

    [SerializeField] private float defaultSoundVolume;
    public float DefaultSoundVolume => defaultSoundVolume;

    [SerializeField] private float minSoundVolume;
    public float MinSoundVolume => minSoundVolume;

    [SerializeField] private float maxSoundVolume;
    public float MaxSoundVolume => maxSoundVolume;

    [SerializeField] private float musicVolume;
    public float MusicVolume { get => musicVolume; set => musicVolume = value; }

    [SerializeField] private float defaultMusicVolume;
    public float DefaultMusicVolume => defaultMusicVolume;

    [SerializeField] private float minMusicVolume;
    public float MinMusicVolume => minMusicVolume;

    [SerializeField] private float maxMusicVolume;
    public float MaxMusicVolume => maxMusicVolume;

    [Header("Controls")]
    [SerializeField] private float horizontalSensibility;
    public float HorizontalSensibility { get => horizontalSensibility; set => horizontalSensibility = value; }

    [SerializeField] private float defaultHorizontalSensibility;
    public float DefaultHorizontalSensibility => defaultHorizontalSensibility;

    [SerializeField] private float minHorizontalSensibility;
    public float MinHorizontalSensibility => minHorizontalSensibility;

    [SerializeField] private float maxHorizontalSensibility;
    public float MaxHorizontalSensibility => maxHorizontalSensibility;

    [SerializeField] private float verticalSensibility;
    public float VerticalSensibility { get => verticalSensibility; set => verticalSensibility = value; }

    [SerializeField] private float defaultVerticalSensibility;
    public float DefaultVerticalSensibility => defaultVerticalSensibility;

    [SerializeField] private float minVerticalSensibility;
    public float MinVerticalSensibility => minVerticalSensibility;

    [SerializeField] private float maxVerticalSensibility;
    public float MaxVerticalSensibility => maxVerticalSensibility;

    /// <summary>
    /// Resets all options.
    /// </summary>
    public void ResetOptions()
    {
        autoLock = defaultAutoLock;
        difficulty = defaultDifficulty;
        screenMode = defaultScreenMode;
        graphicsQuality = defaultGraphicsQuality;
        shadowQuality = defaultShadowQuality;
        shadows = defaultShadows;
        afterImages = defaultAfterImages;
        motionBlur = defaultMotionBlur;
        lightness = defaultLightness;
        contrast = defaultContrast;
        soundVolume = defaultSoundVolume;
        musicVolume = defaultMusicVolume;
        horizontalSensibility = defaultHorizontalSensibility;
        verticalSensibility = defaultVerticalSensibility;
    }

    public void ResetGeneralOptions()
    {
        autoLock = defaultAutoLock;
        difficulty = defaultDifficulty;
    }

    public void ResetGraphicOptions()
    {
        screenMode = defaultScreenMode;
        graphicsQuality = defaultGraphicsQuality;
        shadowQuality = defaultShadowQuality;
        shadows = defaultShadows;
        afterImages = defaultAfterImages;
        motionBlur = defaultMotionBlur;
        lightness = defaultLightness;
        contrast = defaultContrast;
    }

    public void ResetAudioOptions()
    {
        soundVolume = defaultSoundVolume;
        musicVolume = defaultMusicVolume;
    }

    public void ResetControlsOptions()
    {
        horizontalSensibility = defaultHorizontalSensibility;
        verticalSensibility = defaultVerticalSensibility;
    }
}
