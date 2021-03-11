using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats : MonoBehaviour, IDamageable, ICommonDamage
{
    // Stats
    [SerializeField] protected CommonStatsScriptableObj commonStats;

    // Death Behavior
    [SerializeField] private DeathBehaviour deathBehaviour;

    public float Health { get; set; }

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
