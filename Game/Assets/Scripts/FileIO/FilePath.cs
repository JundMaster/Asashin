using UnityEngine;

/// <summary>
/// Struct with file paths.
/// </summary>
public struct FilePath
{
    public static readonly string SAVEFILESTATS = 
        Application.dataPath + "/savefileStats.savefile";

    public static readonly string SAVEFILECHECKPOINT =
        Application.dataPath + "/savefileCheckpoint.savefile";

    public static readonly string SAVEFILESCENE =
        Application.dataPath + "/savefileScene.savefile";

    public static readonly string CONFIG =
        Application.dataPath + "/config.configfile";
}
