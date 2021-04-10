/// <summary>
/// Interface responsible for characters whom can receive damage.
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
