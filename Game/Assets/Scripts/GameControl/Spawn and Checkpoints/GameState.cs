using System.IO;
using System.IO.Compression;
using System;

/// <summary>
/// Class responsible for writing and reading game files.
/// </summary>
sealed public class GameState : FileIO
{
    // Player Stats to save
    private PlayerSavedStatsScriptableObj playerSavedStats;
    private PlayerStats playerStats;

    public GameState(PlayerSavedStatsScriptableObj playerSavedStats)
    {
        this.playerSavedStats = playerSavedStats;
    }

    public void AddPlayerStats(PlayerStats playerStats) =>
        this.playerStats = playerStats;

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
        using (GZipStream gzs = new GZipStream(
            File.Create(FilePath.SAVEFILESTATS), CompressionMode.Compress))
        {
            using (StreamWriter fw = new StreamWriter(gzs))
            {
                fw.WriteLine(playerSavedStats.Kunais);
                fw.WriteLine(playerSavedStats.FirebombKunais);
                fw.WriteLine(playerSavedStats.HealthFlasks);
                fw.WriteLine(playerSavedStats.SmokeGrenades);
                if (playerStats != null)
                    fw.WriteLine(playerStats.Health);
                else
                    fw.WriteLine(100f);
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
            using (GZipStream gzs = new GZipStream(
                File.OpenRead(FilePath.SAVEFILESTATS), CompressionMode.Decompress))
            {
                using (StreamReader fr = new StreamReader(gzs))
                {
                    playerSavedStats.Kunais = Convert.ToInt32(fr.ReadLine());
                    playerSavedStats.FirebombKunais = Convert.ToInt32(fr.ReadLine());
                    playerSavedStats.HealthFlasks = Convert.ToInt32(fr.ReadLine());
                    playerSavedStats.SmokeGrenades = Convert.ToInt32(fr.ReadLine());
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
    /// Saves current checkpoint.
    /// </summary>
    /// <param name="numberToSave">Number of checkpoint.</param>
    public void SaveCheckpoint(byte numberToSave)
    {
        using (GZipStream gzs = new GZipStream(
            File.Create(FilePath.SAVEFILECHECKPOINT), CompressionMode.Compress))
        {
            using (StreamWriter fw = new StreamWriter(gzs))
            {
                fw.WriteLine(numberToSave);
            }
        }
    }

    /// <summary>
    /// Saves current scene.
    /// </summary>
    /// <param name="scene">Scene to save.</param>
    public void SaveCheckpointScene(SceneEnum scene)
    {
        using (GZipStream gzs = new GZipStream(
            File.Create(FilePath.SAVEFILESCENE), CompressionMode.Compress))
        {
            using (StreamWriter fw = new StreamWriter(gzs))
            {
                fw.WriteLine(scene);
            }
        }
    }

    /// <summary>
    /// Loads a checkpoint number.
    /// </summary>
    /// <returns>Number of checkpoint.</returns>
    public byte LoadCheckpoint()
    {
        using (GZipStream gzs = new GZipStream(
            File.OpenRead(FilePath.SAVEFILECHECKPOINT), CompressionMode.Decompress))
        {
            using (StreamReader fr = new StreamReader(gzs))
            {
                return (byte)Convert.ChangeType(fr.ReadLine(), typeof(byte));
            }
        }
    }

    /// <summary>
    /// Loads a scene enum.
    /// </summary>
    /// <returns>Scene to load.</returns>
    public SceneEnum LoadCheckpointScene()
    {
        using (GZipStream gzs = new GZipStream(
            File.OpenRead(FilePath.SAVEFILESCENE), CompressionMode.Decompress))
        {
            using (StreamReader fr = new StreamReader(gzs))
            {
                if (Enum.TryParse(fr.ReadLine(), out SceneEnum savedVal))
                    return (SceneEnum)Convert.ChangeType(savedVal, typeof(SceneEnum));

                return SceneEnum.MainMenu;
            }
        }
    }
}
