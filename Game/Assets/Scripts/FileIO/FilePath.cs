using UnityEngine;

/// <summary>
/// Struct with file paths.
/// </summary>
public struct FilePath
{
    public static readonly string SAVEFILESTATS = 
        Application.dataPath + "/Temp/savefileStats.savefile";

    public static readonly string SAVEFILECHECKPOINT =
        Application.dataPath + "/Temp/savefileCheckpoint.savefile";

    public static readonly string SAVEFILESCENE =
        Application.dataPath + "/Temp/savefileScene.savefile";

    public static readonly string CONFIG =
        Application.dataPath + "/Temp/config.savefile";
}
