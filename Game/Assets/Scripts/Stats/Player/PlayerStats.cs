using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStats : Stats
{
    [SerializeField] private PlayerStatsScriptableObj playerStats;

    public byte Lives { get => playerStats.Lives; set => playerStats.Lives = value; }
    public byte Kunais { get => playerStats.Kunais; set => playerStats.Kunais = value; }
    public byte FirebombKunais { get => playerStats.FirebombKunais; set => playerStats.FirebombKunais = value; }
    public byte SmokeGrenades { get => playerStats.SmokeGrenades; set => playerStats.SmokeGrenades = value; }
}
