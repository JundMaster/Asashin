/// <summary>
/// Interface responsible for player damage.
/// </summary>
public interface IPlayerDamage: ICommonDamage
{
    /// <summary>
    /// Getter for damage.
    /// </summary>
    float StrongDamage { get; }

    /// <summary>
    /// Getter for damage.
    /// </summary>
    float FirebombKunaiDamage { get; }
}
