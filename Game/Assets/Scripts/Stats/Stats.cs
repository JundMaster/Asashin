using UnityEngine;
using System;

/// <summary>
/// Class responsible for common stats.
/// </summary>
public abstract class Stats : MonoBehaviour, IDamageable, ICommonDamage
{
    // Stats
    [SerializeField] protected CommonStatsScriptableObj commonStats;

    public float Health { get; protected set; }

    public float LightDamage => commonStats.Damage;

    public float RangedDamage => commonStats.RangedDamage;

    private void Start()
    {
        Health = commonStats.MaxHealth;
    }

    /// <summary>
    /// Takes an amount of damage.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    public void TakeDamage(float damage)
    {
        // If this body receives damage
        if (damage > 0)
        {
            if (Health - damage > 0)
            {
                Health -= damage;
            }
            else
            {
                Health = 0;
                OnDie();
            }
        }
        OnTookDamage();
    }

    protected virtual void OnTookDamage() => TookDamage?.Invoke();

    /// <summary>
    /// Event registered on UIHealthBar.
    /// Event registered on EnemyDamaged.
    /// </summary>
    public event Action TookDamage;

    protected virtual void OnDie() => Die?.Invoke();

    /// <summary>
    /// Event registered on classes with stats class.
    /// </summary>
    public event Action Die;
}
