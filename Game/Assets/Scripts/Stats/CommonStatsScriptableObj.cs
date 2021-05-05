using UnityEngine;

[CreateAssetMenu(fileName = "Common Stats")]
/// <summary>
/// Scriptable object with player and enemies stats.
/// </summary>
public sealed class CommonStatsScriptableObj : ScriptableObject
{
    [SerializeField] private float maxHealth;
    public float MaxHealth => maxHealth;

    [SerializeField] private float damage;
    public float Damage => damage;

    [SerializeField] private float rangedDamage;
    public float RangedDamage => rangedDamage;

    [SerializeField] private float firebombKunaiDamage;
    public float FirebombKunaiDamage => firebombKunaiDamage;
}
