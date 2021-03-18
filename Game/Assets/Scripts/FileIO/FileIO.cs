using System.IO;
using System.IO.Compression;
using System;

/// <summary>
/// Class responsible for saving the game.
/// </summary>
public struct FileIO
{
    // Player Stats to save
    private PlayerStatsScriptableObj playerInventory;
    private PlayerStats playerStats;
    
    public FileIO(
        PlayerStatsScriptableObj playerInventory,
        PlayerStats playerStats)
    {
        this.playerInventory = playerInventory;
        this.playerStats = playerStats;
    }

    public bool FileExists(string file)
    {
        if (File.Exists(file)) return true;
        return false;
    }

    /// <summary>
    /// Saves game.
    /// </summary>
    public void SavePlayerStats()
    {
        using (FileStream fs = new FileStream(FilePath.SAVEFILESTATS, FileMode.Create, FileAccess.Write))
        {
            using (GZipStream gzs = new GZipStream(fs, CompressionLevel.Optimal))
            {
                using (StreamWriter fw = new StreamWriter(fs))
                {
                    fw.WriteLine(playerInventory.Kunais);
                    fw.WriteLine(playerInventory.FirebombKunais);
                    fw.WriteLine(playerInventory.HealthFlasks);
                    fw.WriteLine(playerInventory.SmokeGrenades);
                    fw.WriteLine(playerStats.Health);
                }
            }
        }
    }


    /// <summary>
    /// Loads game.
    /// </summary>
    public void LoadPlayerStats()
    {
        if (File.Exists(FilePath.SAVEFILESTATS))
        {
            using (FileStream fs = new FileStream(FilePath.SAVEFILESTATS, FileMode.Open, FileAccess.Read))
            {
                using (GZipStream gzs = new GZipStream(fs, CompressionLevel.NoCompression))
                {
                    using (StreamReader fr = new StreamReader(fs))
                    {
                        playerInventory.Kunais = Convert.ToByte(fr.ReadLine());
                        playerInventory.FirebombKunais = Convert.ToByte(fr.ReadLine());
                        playerInventory.HealthFlasks = Convert.ToByte(fr.ReadLine());
                        playerInventory.SmokeGrenades = Convert.ToByte(fr.ReadLine());
                        //playerStats.TakeDamage(100f - Convert.ToSingle(fr.ReadLine()));
                    }
                }
            }
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
                using (FileStream fs = new FileStream(FilePath.SAVEFILECHECKPOINT, FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream gzs = new GZipStream(fs, CompressionLevel.Optimal))
                    {
                        using (StreamWriter fw = new StreamWriter(fs))
                        {
                            fw.WriteLine(numberToSave);
                        }
                    }
                }
                break;
            case SaveAndLoadEnum.CheckpointScene:
                using (FileStream fs = new FileStream(FilePath.SAVEFILESCENE, FileMode.Create, FileAccess.Write))
                {
                    using (GZipStream gzs = new GZipStream(fs, CompressionLevel.Optimal))
                    {
                        using (StreamWriter fw = new StreamWriter(fs))
                        {
                            fw.WriteLine(numberToSave);
                        }
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
                using (FileStream fs = new FileStream(FilePath.SAVEFILECHECKPOINT, FileMode.Open, FileAccess.Read))
                {
                    using (GZipStream gzs = new GZipStream(fs, CompressionLevel.NoCompression))
                    {
                        using (StreamReader fr = new StreamReader(fs))
                        {
                            return Convert.ToByte(fr.ReadLine());
                        }
                    }
                }
            case SaveAndLoadEnum.CheckpointScene:
                using (FileStream fs = new FileStream(FilePath.SAVEFILESCENE, FileMode.Open, FileAccess.Read))
                {
                    using (GZipStream gzs = new GZipStream(fs, CompressionLevel.NoCompression))
                    {
                        using (StreamReader fr = new StreamReader(fs))
                        {
                            return Convert.ToByte(fr.ReadLine());
                        }
                    }
                }
            default:
                return 0;
        }
    }
}
