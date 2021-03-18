using UnityEngine;

/// <summary>
/// Struct with file paths.
/// </summary>
public struct FilePath
{
    public static readonly string SAVEFILESTATS = 
        Application.dataPath + "/Temp/savefileStats.txt";

    public static readonly string SAVEFILECHECKPOINT =
        Application.dataPath + "/Temp/savefileCheckpoint.txt";

    public static readonly string SAVEFILESCENE =
        Application.dataPath + "/Temp/savefileScene.txt";

    public static readonly string CONFIG =
        Application.dataPath + "/Temp/config.txt";
}
