using UnityEngine;

/// <summary>
/// Class responsible for handling enemy stats.
/// </summary>
public class EnemyStats : Stats
{
    [SerializeField] private OptionsScriptableObj options;
    [SerializeField] private CommonStatsScriptableObj normalDifficulty;
    [SerializeField] private CommonStatsScriptableObj hardDifficulty;

    /// <summary>
    /// Sets common stats depending the game difficulty.
    /// </summary>
    private void Awake()
    {
        switch (options.Difficulty)
        {
            case 0:
                commonStats = normalDifficulty;
                break;
            case 1:
                commonStats = hardDifficulty;
                break;
            default:
                commonStats = normalDifficulty;
                break;
        }
    }
}
