using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Class responsible for saving the game.
/// </summary>
public class SaveAndLoadSystem : MonoBehaviour
{
    // Player Stats to save
    [SerializeField] private PlayerStatsScriptableObj playerStats;

    /// <summary>
    /// Saves game.
    /// </summary>
    public void SaveGame()
    {
        using (FileStream fs = new FileStream(FilePath.SAVEFILE, FileMode.Create, FileAccess.Write))
        {
            //using (GZipStream gzs = new GZipStream(fs, System.IO.Compression.CompressionLevel.Optimal))
            //{
                using (StreamWriter fw = new StreamWriter(fs))
                {
                    fw.WriteLine(playerStats.Lives);
                    fw.WriteLine(playerStats.Kunais);
                    fw.WriteLine(playerStats.FirebombKunais);
                    fw.WriteLine(playerStats.SmokeGrenades);
                }
            //}
        }
    }

    /// <summary>
    /// Loads game.
    /// </summary>
    public void LoadGame()
    {
        using (FileStream fs = new FileStream(FilePath.SAVEFILE, FileMode.Open, FileAccess.Read))
        {
            //using (GZipStream gzs = new GZipStream(fs, System.IO.Compression.CompressionLevel.NoCompression))
            //{
                using (StreamReader fr = new StreamReader(fs))
                {
                    playerStats.Lives = Convert.ToByte(fr.ReadLine());
                    playerStats.Kunais = Convert.ToByte(fr.ReadLine());
                    playerStats.FirebombKunais = Convert.ToByte(fr.ReadLine());
                    playerStats.SmokeGrenades = Convert.ToByte(fr.ReadLine());
                }
            //}
        }
    }
}
