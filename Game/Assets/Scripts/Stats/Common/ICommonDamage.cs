/// <summary>
/// Interface responsible for player and enemy damage.
/// </summary>
public interface ICommonDamage
{
    /// <summary>
    /// Getter for damage.
    /// </summary>
    float LightDamage { get; }

    /// <summary>
    /// Getter for damage.
    /// </summary>
    float RangedDamage { get; }
}
