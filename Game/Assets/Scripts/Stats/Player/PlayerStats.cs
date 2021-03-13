﻿using UnityEngine;

/// <summary>
/// Class responsible for handling player's stats.
/// </summary>
public sealed class PlayerStats : Stats, IPlayerDamage
{
    [SerializeField] private PlayerStatsScriptableObj playerStats;

    public byte Lives { get => playerStats.Lives; set => playerStats.Lives = value; }
    public byte Kunais { get => playerStats.Kunais; set => playerStats.Kunais = value; }
    public byte FirebombKunais { get => playerStats.FirebombKunais; set => playerStats.FirebombKunais = value; }
    public byte SmokeGrenades { get => playerStats.SmokeGrenades; set => playerStats.SmokeGrenades = value; }
    public float StrongDamage => commonStats.Damage * 2;
    public float FirebombKunaiDamage => commonStats.RangedDamage * 10;
}
