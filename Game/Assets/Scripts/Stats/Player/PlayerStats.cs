using UnityEngine;

/// <summary>
/// Class responsible for handling player's stats.
/// </summary>
public sealed class PlayerStats : Stats, IPlayerDamage
{
    [SerializeField] private PlayerStatsScriptableObj playerStats;

    // Items
    public byte Kunais { get => playerStats.Kunais; set => playerStats.Kunais = value; }
    public byte FirebombKunais { get => playerStats.FirebombKunais; set => playerStats.FirebombKunais = value; }
    public byte HealthFlasks { get => playerStats.HealthFlasks; set => playerStats.FirebombKunais = value; }
    public byte SmokeGrenades { get => playerStats.SmokeGrenades; set => playerStats.SmokeGrenades = value; }

    // Stats
    public float StrongDamage => commonStats.Damage * 2;
    public float FirebombKunaiDamage => commonStats.RangedDamage * 10;
}
