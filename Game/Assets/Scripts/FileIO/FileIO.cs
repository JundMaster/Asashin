using System.IO;

/// <summary>
/// Class responsible for controling files.
/// </summary>
public abstract class FileIO
{
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
}
