using UnityEngine;

[CreateAssetMenu(fileName ="Player Saved Stats")]
/// <summary>
/// Scriptable object responsible for handling related player stats.
/// </summary>
public sealed class PlayerSavedStatsScriptableObj : ScriptableObject
{
    [SerializeField] private int kunais;
    public int Kunais { get => kunais; set => kunais = value; }

    [SerializeField] private int firebombKunais;
    public int FirebombKunais { get => firebombKunais; set => firebombKunais = value; }

    [SerializeField] private int healthFlasks;
    public int HealthFlasks { get => healthFlasks; set => healthFlasks = value; }

    [SerializeField] private int smokeGrenades;
    public int SmokeGrenades { get => smokeGrenades; set => smokeGrenades = value; }

    public float SavedHealth { get; set; }

    [SerializeField] private int defaultKunais;
    public int DefaultKunais { get => defaultKunais; set => defaultKunais = value; }

    [SerializeField] private int defaultFirebombKunais;
    public int DefaultFirebombKunais { get => defaultFirebombKunais; set => defaultFirebombKunais = value; }

    [SerializeField] private int defaultHealthFlasks;
    public int DefaultHealthFlasks { get => defaultHealthFlasks; set => defaultHealthFlasks = value; }

    [SerializeField] private int defaultSmokeGrenades;
    public int DefaultSmokeGrenades { get => defaultSmokeGrenades; set => defaultSmokeGrenades = value; }

    [SerializeField] private float defaultSavedHealth;
    public float DefaultSavedHealth => defaultSavedHealth;
}
