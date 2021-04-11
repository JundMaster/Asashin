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
    /// Takes an amount of damage.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="typeOfDamage">Type of this damage.</param>
    void TakeDamage(float damage, TypeOfDamage typeOfDamage);
}
