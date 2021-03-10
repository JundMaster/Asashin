/// <summary>
/// Interface responsible for player and enemy stats.
/// </summary>
public interface IStats
{
    /// <summary>
    /// Getter for health.
    /// </summary>
    float Health { get; }

    /// <summary>
    /// Getter for damage.
    /// </summary>
    float Damage { get; }
}
