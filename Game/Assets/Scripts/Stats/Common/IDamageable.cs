/// <summary>
/// Interface responsible for player and enemy health stats.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Getter for health.
    /// </summary>
    float Health { get; }

    /// <summary>
    /// Method responsible for doing damage.
    /// </summary>
    /// <param name="damage"></param>
    void TakeDamage(float damage);
}
