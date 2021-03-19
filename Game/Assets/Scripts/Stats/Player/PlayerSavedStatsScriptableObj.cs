using UnityEngine;

[CreateAssetMenu(fileName ="Player Saved Stats")]
/// <summary>
/// Scriptable object responsible for handling related player stats.
/// </summary>
public sealed class PlayerSavedStatsScriptableObj : ScriptableObject
{
    [SerializeField] private byte kunais;
    public byte Kunais { get => kunais; set => kunais = value; }

    [SerializeField] private byte firebombKunais;
    public byte FirebombKunais { get => firebombKunais; set => firebombKunais = value; }

    [SerializeField] private byte healthFlasks;
    public byte HealthFlasks { get => healthFlasks; set => healthFlasks = value; }

    [SerializeField] private byte smokeGrenades;
    public byte SmokeGrenades { get => smokeGrenades; set => smokeGrenades = value; }

    public float SavedHealth { get; set; }

    [SerializeField] private byte defaultKunais;
    public byte DefaultKunais { get => defaultKunais; set => defaultKunais = value; }

    [SerializeField] private byte defaultFirebombKunais;
    public byte DefaultFirebombKunais { get => defaultFirebombKunais; set => defaultFirebombKunais = value; }

    [SerializeField] private byte defaultHealthFlasks;
    public byte DefaultHealthFlasks { get => defaultHealthFlasks; set => defaultHealthFlasks = value; }

    [SerializeField] private byte defaultSmokeGrenades;
    public byte DefaultSmokeGrenades { get => defaultSmokeGrenades; set => defaultSmokeGrenades = value; }

    [SerializeField] private float defaultSavedHealth;
    public float DefaultSavedHealth => defaultSavedHealth;
}
