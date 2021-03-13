using UnityEngine;

[CreateAssetMenu(fileName ="Player Stats")]
/// <summary>
/// Scriptable object responsible for handling related player stats.
/// </summary>
public sealed class PlayerStatsScriptableObj : ScriptableObject
{
    [SerializeField] private byte kunais;
    public byte Kunais { get => kunais; set => kunais = value; }

    [SerializeField] private byte firebombKunais;
    public byte FirebombKunais { get => firebombKunais; set => firebombKunais = value; }

    [SerializeField] private byte healthFlasks;
    public byte HealthFlasks { get => healthFlasks; set => healthFlasks = value; }

    [SerializeField] private byte smokeGrenades;
    public byte SmokeGrenades { get => smokeGrenades; set => smokeGrenades = value; }
}
