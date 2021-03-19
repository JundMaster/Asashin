using UnityEngine;

/// <summary>
/// Class responsible for creationg a scriptable object with game options.
/// </summary>
[CreateAssetMenu(fileName = "Game Options")]
public class OptionsScriptableObj : ScriptableObject
{
    [Header("Game Options")]
    [SerializeField] private byte difficulty;
    public byte Difficulty { get => difficulty; set => difficulty = value; }

    [SerializeField] private byte defaultDifficulty;
    public byte DefaultDifficulty => defaultDifficulty;

    [Header("Graphic Options")]
    [SerializeField] private byte graphicsQuality;
    public byte GraphicsQuality { get => graphicsQuality; set => graphicsQuality = value; }

    [SerializeField] private byte defaultGraphicsQuality;
    public byte DefaultGraphicsQuality => defaultGraphicsQuality;

    [SerializeField] private byte shadowQuality;
    public byte ShadowQuality { get => shadowQuality; set => shadowQuality = value; }

    [SerializeField] private byte defaultShadowQuality;
    public byte DefaultShadowQuality => defaultShadowQuality;

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

    [SerializeField] private float contrast;
    public float Contrast { get => contrast; set => contrast = value; }

    [SerializeField] private float defaultContrast;
    public float DefaultContrast => defaultContrast;

    [Header("Sound Options")]
    [SerializeField] private float soundVolume;
    public float SoundVolume { get => soundVolume; set => soundVolume = value; }

    [SerializeField] private float defaultSoundVolume;
    public float DefaultSoundVolume => defaultSoundVolume;

    [SerializeField] private float musicVolume;
    public float MusicVolume { get => musicVolume; set => musicVolume = value; }

    [SerializeField] private float defaultMusicVolume;
    public float DefaultMusicVolume => defaultMusicVolume;

    [Header("Controls")]
    [SerializeField] private float horizontalSensibility;
    public float HorizontalSensibility { get => horizontalSensibility; set => horizontalSensibility = value; }

    [SerializeField] private float defaultHorizontalSensibility;
    public float DefaultHorizontalSensibility => defaultHorizontalSensibility;

    [SerializeField] private float verticalSensibility;
    public float VerticalSensibility { get => verticalSensibility; set => verticalSensibility = value; }

    [SerializeField] private float defaultVerticalSensibility;
    public float DefaultVerticalSensibility => defaultVerticalSensibility;

    public void ResetOptions()
    {
        difficulty = defaultDifficulty;
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
}
