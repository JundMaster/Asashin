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

    [Header("Ranged damage is multiplied by this value in order to obtain firebomb kunai damage")]
    [Range(1f, 5f)][SerializeField] private float firebombKunaiRangedDamageMultiplier;
    public float FirebombKunaiRangedDamageMultiplier => firebombKunaiRangedDamageMultiplier;
}
