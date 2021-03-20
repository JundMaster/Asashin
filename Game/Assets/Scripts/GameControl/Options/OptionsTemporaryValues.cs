/// <summary>
/// Struct responsible for keeping current config options.
/// </summary>
public struct OptionsTemporaryValues
{
    public short Difficulty { get; set; }
    public short GraphicsQuality { get; set; }
    public short ShadowQuality { get; set; }
    public bool Shadows { get; set; }
    public bool AfterImages { get; set; }
    public bool MotionBlur { get; set; }
    public float Lightness { get; set; }
    public float Contrast { get; set; }
    public float SoundVolume { get; set; }
    public float MusicVolume { get; set; }
    public float HorizontalSensibility { get; set; }
    public float VerticalSensibility { get; set; }

    public OptionsTemporaryValues(OptionsScriptableObj options)
    {
        Difficulty = options.Difficulty;
        GraphicsQuality = options.GraphicsQuality;
        ShadowQuality = options.ShadowQuality;
        Shadows = options.Shadows;
        AfterImages = options.AfterImages;
        MotionBlur = options.MotionBlur;
        Lightness = options.Lightness;
        Contrast = options.Contrast;
        SoundVolume = options.SoundVolume;
        MusicVolume = options.MusicVolume;
        HorizontalSensibility = options.HorizontalSensibility;
        VerticalSensibility = options.VerticalSensibility;
    }
}
