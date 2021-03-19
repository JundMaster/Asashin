using System.IO;
using System.IO.Compression;
using System;

/// <summary>
/// Class responsible for saving the game.
/// </summary>
public struct FileIO
{
    // Player Stats to save
    private PlayerSavedStatsScriptableObj playerSavedStats;
    private PlayerStats playerStats;
    
    public FileIO(
        PlayerSavedStatsScriptableObj playerSavedStats,
        PlayerStats playerStats)
    {
        this.playerSavedStats = playerSavedStats;
        this.playerStats = playerStats;
    }

    /// <summary>
    /// Checks if file exists.
    /// </summary>
    /// <param name="file">File path to check.</param>
    /// <returns>Returns true if file exists.</returns>
    public bool FileExists(string file)
    {
        if (File.Exists(file)) return true;
        return false;
    }

    public void DeleteFiles()
    {
        if (File.Exists(FilePath.SAVEFILECHECKPOINT)) File.Delete(FilePath.SAVEFILECHECKPOINT);
        if (File.Exists(FilePath.SAVEFILESCENE)) File.Delete(FilePath.SAVEFILESCENE);
        if (File.Exists(FilePath.SAVEFILESTATS)) File.Delete(FilePath.SAVEFILESTATS);
    }

    /// <summary>
    /// Saves player stats.
    /// </summary>
    public void SavePlayerStats()
    {
        using (GZipStream gzs = new GZipStream(File.Create(FilePath.SAVEFILESTATS), CompressionMode.Compress))
        {
            using (StreamWriter fw = new StreamWriter(gzs))
            {
                fw.WriteLine(playerSavedStats.Kunais);
                fw.WriteLine(playerSavedStats.FirebombKunais);
                fw.WriteLine(playerSavedStats.HealthFlasks);
                fw.WriteLine(playerSavedStats.SmokeGrenades);
                fw.WriteLine(playerStats.Health);
            }
        }
    }


    /// <summary>
    /// Loads player stats.
    /// </summary>
    public void LoadPlayerStats()
    {
        if (File.Exists(FilePath.SAVEFILESTATS))
        {
            using (GZipStream gzs = new GZipStream(File.OpenRead(FilePath.SAVEFILESTATS), CompressionMode.Decompress))
            {
                using (StreamReader fr = new StreamReader(gzs))
                {
                    playerSavedStats.Kunais = Convert.ToByte(fr.ReadLine());
                    playerSavedStats.FirebombKunais = Convert.ToByte(fr.ReadLine());
                    playerSavedStats.HealthFlasks = Convert.ToByte(fr.ReadLine());
                    playerSavedStats.SmokeGrenades = Convert.ToByte(fr.ReadLine());
                    playerSavedStats.SavedHealth = Convert.ToSingle(fr.ReadLine());
                }
            }
        }
        else
        {
            playerSavedStats.Kunais = playerSavedStats.DefaultKunais;
            playerSavedStats.FirebombKunais = playerSavedStats.DefaultFirebombKunais;
            playerSavedStats.HealthFlasks = playerSavedStats.DefaultHealthFlasks;
            playerSavedStats.SmokeGrenades = playerSavedStats.DefaultSmokeGrenades;
            playerSavedStats.SavedHealth = playerSavedStats.DefaultSavedHealth;
        }
    }

    /// <summary>
    /// Saves current checkpoint and current scene.
    /// </summary>
    /// <param name="condition">Type of save.</param>
    /// <param name="numberToSave">Number of checkpoint or scene.</param>
    public void SaveCheckpoint(SaveAndLoadEnum condition, byte numberToSave)
    {
        switch (condition)
        {
            case SaveAndLoadEnum.Checkpoint:
                using (GZipStream gzs = new GZipStream(File.Create(FilePath.SAVEFILECHECKPOINT), CompressionMode.Compress))
                {
                    using (StreamWriter fw = new StreamWriter(gzs))
                    {
                        fw.WriteLine(numberToSave);
                    }
                }
                break;
            case SaveAndLoadEnum.CheckpointScene:
                using (GZipStream gzs = new GZipStream(File.Create(FilePath.SAVEFILESCENE), CompressionMode.Compress))
                {
                    using (StreamWriter fw = new StreamWriter(gzs))
                    {
                        fw.WriteLine(numberToSave);
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Saves current checkpoint and current scene.
    /// </summary>
    /// <param name="condition">Type of save.</param>
    /// <param name="numberToSave">Number of checkpoint or scene.</param>
    public byte LoadCheckpoint(SaveAndLoadEnum condition)
    {
        switch (condition)
        {
            case SaveAndLoadEnum.Checkpoint:
                using (GZipStream gzs = new GZipStream(File.OpenRead(FilePath.SAVEFILECHECKPOINT), CompressionMode.Decompress))
                {
                    using (StreamReader fr = new StreamReader(gzs))
                    {
                        return Convert.ToByte(fr.ReadLine());
                    }
                }
            case SaveAndLoadEnum.CheckpointScene:
                using (GZipStream gzs = new GZipStream(File.OpenRead(FilePath.SAVEFILESCENE), CompressionMode.Decompress))
                {
                    using (StreamReader fr = new StreamReader(gzs))
                    {
                        return Convert.ToByte(fr.ReadLine());
                    }
                }
            default:
                return 0;
        }
    }
}
