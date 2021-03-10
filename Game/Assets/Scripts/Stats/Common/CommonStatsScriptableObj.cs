using UnityEngine;

[CreateAssetMenu(fileName = "Common Stats")]
/// <summary>
/// Scriptable object with player and enemies stats.
/// </summary>
public sealed class CommonStatsScriptableObj : ScriptableObject
{
    [SerializeField] private float maxHealth;
    public float MaxHealth => maxHealth;

    [SerializeField] private float currentHealth;
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }

    [SerializeField] private float damage;
    public float Damage => damage;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }
}
