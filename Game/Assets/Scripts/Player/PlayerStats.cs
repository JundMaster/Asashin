using UnityEngine;
using System;

/// <summary>
/// Class responsible for handling player's stats.
/// </summary>
public sealed class PlayerStats : Stats, IPlayerDamage, IHealable
{
    [SerializeField] private PlayerSavedStatsScriptableObj playerStats;

    // Items
    public int Kunais 
    { get => playerStats.Kunais; set => playerStats.Kunais = value; }
    public int FirebombKunais 
    { get => playerStats.FirebombKunais; set => playerStats.FirebombKunais = value; }
    public int HealthFlasks 
    { get => playerStats.HealthFlasks; set => playerStats.HealthFlasks = value; }
    public int SmokeGrenades 
    { get => playerStats.SmokeGrenades; set => playerStats.SmokeGrenades = value; }

    // Stats
    public float StrongDamage => commonStats.Damage * 2;
    public float FirebombKunaiDamage => commonStats.RangedDamage * 10;

    /// <summary>
    /// Heals an amount of damage.
    /// </summary>
    /// <param name="health">Heal to receive.</param>
    public void HealHealth(float health)
    {
        if (Health + health > commonStats.MaxHealth)
        {
            Health = commonStats.MaxHealth;
        }
        else
        {
            Health += health;
        }
        OnHealedDamage();
    }

    private void OnHealedDamage() => HealedDamage?.Invoke();

    public event Action HealedDamage;
}
