using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats : MonoBehaviour, IStats
{
    // Stats
    [SerializeField] protected CommonStatsScriptableObj commonStats;

    // Death Behavior
    [SerializeField] private DeathBehaviour deathBehaviour;

    public float Health
    { 
        get => commonStats.CurrentHealth; 
        set => commonStats.CurrentHealth = value;
    }

    public float Damage => commonStats.Damage;

    /// <summary>
    /// Takes an amount of damage.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    public void TakeDamage(float damage)
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
}
