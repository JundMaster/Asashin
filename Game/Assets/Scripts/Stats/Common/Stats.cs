﻿using UnityEngine;
using System;

/// <summary>
/// Class responsible for common stats.
/// </summary>
public abstract class Stats : MonoBehaviour, IDamageable, ICommonDamage
{
    // Stats
    [SerializeField] protected CommonStatsScriptableObj commonStats;

    // Death Behavior
    [SerializeField] private DeathBehaviour deathBehaviour;

    public float Health { get; private set; }

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
                deathBehaviour.Die();
            }
        }
        OnTookDamage();
    }

    /// <summary>
    /// Heals an amount of damage.
    /// </summary>
    /// <param name="health">Heal to receive.</param>
    public void HealHealth(float health)
    {
        if (Health + health > commonStats.MaxHealth)
        {
            Health = commonStats.MaxHealth;
        }
        else
        {
            Health += health;
        }
        OnTookDamage();
    }

    protected virtual void OnTookDamage() => TookDamage?.Invoke();

    /// <summary>
    /// Event registered on UIHealthBar.
    /// </summary>
    public event Action TookDamage;
}
