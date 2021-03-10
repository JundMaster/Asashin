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

    private readonly string SAVEFILE = Application.dataPath + "/saveFile.txt";

    /// <summary>
    /// Saves game.
    /// </summary>
    public void SaveGame()
    {
        using (StreamWriter fw = File.CreateText(SAVEFILE))
        {
            fw.WriteLine(playerStats.Lives);
            fw.WriteLine(playerStats.Kunais);
            fw.WriteLine(playerStats.FirebombKunais);
            fw.WriteLine(playerStats.SmokeGrenades);
        }
    }

    /// <summary>
    /// Loads game.
    /// </summary>
    public void LoadGame()
    {
        using (StreamReader fr = File.OpenText(SAVEFILE))
        {
            playerStats.Lives = Convert.ToByte(fr.ReadLine());
            playerStats.Kunais = Convert.ToByte(fr.ReadLine());
            playerStats.FirebombKunais = Convert.ToByte(fr.ReadLine());
            playerStats.SmokeGrenades = Convert.ToByte(fr.ReadLine());
        }
    }
}
