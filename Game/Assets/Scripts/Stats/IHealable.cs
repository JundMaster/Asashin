/// <summary>
/// Interface responsible for player and enemy health stats.
/// </summary>
public interface IHealable
{
    /// <summary>
    /// Heals an amount of damage.
    /// </summary>
    /// <param name="health">Heal to receive.</param>
    void HealHealth(float health);
}
