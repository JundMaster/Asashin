using System.IO;
using System.IO.Compression;
using System;

/// <summary>
/// Class responsible for writing and loading game options' files.
/// </summary>
sealed public class GameOptions : FileIO
{
    private OptionsScriptableObj options;

    public GameOptions(OptionsScriptableObj options)
    {
        this.options = options;
    }
    
    public void CreateConfigFile()
    {
        using (FileStream fs = File.Create(FilePath.CONFIG))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine();
            }
        }
    }

    /// <summary>
    /// Write current config options.
    /// </summary>
    public void SaveConfig()
    {
        using (GZipStream gzs = new GZipStream(
            File.Create(FilePath.CONFIG), CompressionMode.Compress))
        {
            using (StreamWriter sw = new StreamWriter(gzs))
            {
                sw.WriteLine(options.Difficulty);
                sw.WriteLine(options.GraphicsQuality);
                sw.WriteLine(options.ShadowQuality);
                sw.WriteLine(options.Shadows);
                sw.WriteLine(options.AfterImages);
                sw.WriteLine(options.MotionBlur);
                sw.WriteLine(options.Lightness);
                sw.WriteLine(options.Contrast);
                sw.WriteLine(options.SoundVolume);
                sw.WriteLine(options.MusicVolume);
                sw.WriteLine(options.HorizontalSensibility);
                sw.WriteLine(options.VerticalSensibility);
            }
        }
    }

    /// <summary>
    /// Read last saved config options.
    /// </summary>
    public void LoadConfig()
    {
        if (File.Exists(FilePath.CONFIG))
        {
            using (GZipStream gzs = new GZipStream(
                File.OpenRead(FilePath.CONFIG), CompressionMode.Decompress))
            {
                using (StreamReader fr = new StreamReader(gzs))
                {
                    options.Difficulty = Convert.ToByte(fr.ReadLine());
                    options.GraphicsQuality = Convert.ToByte(fr.ReadLine());
                    options.ShadowQuality = Convert.ToByte(fr.ReadLine());
                    options.Shadows = Convert.ToBoolean(fr.ReadLine());
                    options.AfterImages = Convert.ToBoolean(fr.ReadLine());
                    options.MotionBlur = Convert.ToBoolean(fr.ReadLine());
                    options.Lightness = Convert.ToSingle(fr.ReadLine());
                    options.Contrast = Convert.ToSingle(fr.ReadLine());
                    options.SoundVolume = Convert.ToSingle(fr.ReadLine());
                    options.MusicVolume = Convert.ToSingle(fr.ReadLine());
                    options.HorizontalSensibility = Convert.ToSingle(fr.ReadLine());
                    options.VerticalSensibility = Convert.ToSingle(fr.ReadLine());
                }
            }
        }
        else
        {
            LoadDefaultOptions();
        }
    }

    public void LoadDefaultOptions() => options.ResetOptions();
}
