﻿using UnityEngine;
using System;

/// <summary>
/// Class responsible for common stats.
/// </summary>
public abstract class Stats : MonoBehaviour, IDamageable, ICommonDamage
{
    /// <summary>
    /// This variable is initialized on the children classes.
    /// </summary>
    protected CommonStatsScriptableObj commonStats;

    public float Health { get; set; }
    public float MaxHealth => commonStats.MaxHealth;

    public float LightDamage => commonStats.Damage;

    public float RangedDamage => commonStats.RangedDamage;

    protected void Start() =>
        Health = commonStats.MaxHealth;

    /// <summary>
    /// Takes an amount of damage.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    /// <param name="typeOfDamage">Type of this damage.</param>
    public void TakeDamage(float damage, TypeOfDamage typeOfDamage)
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
                return;
            }
        }

        // If the player didn't die
        switch (typeOfDamage)
        {
            case TypeOfDamage.EnemyMelee:
                OnTookDamage();
                break;
            case TypeOfDamage.EnemyRanged:
                OnTookDamage();
                break;
            case TypeOfDamage.PlayerMelee:
                OnAnyDamageOnEnemy();
                OnMeleeDamageOnEnemy();
                break;
            case TypeOfDamage.PlayerRanged:
                OnAnyDamageOnEnemy();
                break;
            case TypeOfDamage.PlayerBlockDamage:
                OnNoDamageBlock();
                break;
            case TypeOfDamage.None:
                OnOnlyUpdateUI();
                break;
        }  
    }

    protected void OnNoDamageBlock() => NoDamageBlock?.Invoke();

    /// <summary>
    /// Event registered on PlayerAnimations.
    /// Triggers block reflect animation.
    /// </summary>
    public event Action NoDamageBlock;

    protected virtual void OnTookDamage() => TookDamage?.Invoke();

    /// <summary>
    /// Event registered on UIHealthBar.
    /// Event registered on PlayerMovement.
    /// Event registered on Cheats.
    /// </summary>
    public event Action TookDamage;

    /// <summary>
    /// Method that calls OnlyUpdateUI event.
    /// </summary>
    protected virtual void OnOnlyUpdateUI() => OnlyUpdateUI?.Invoke();

    /// <summary>
    /// Event registered on UIHealthBar.
    /// Only happens to update ui health bar.
    /// </summary>
    public event Action OnlyUpdateUI;

    protected virtual void OnMeleeDamageOnEnemy() =>
        MeleeDamageOnEnemy?.Invoke();

    /// <summary>
    /// Event registered on enemy temporary blindness state.
    /// </summary>
    public event Action MeleeDamageOnEnemy;

    protected virtual void OnAnyDamageOnEnemy() => AnyDamageOnEnemy?.Invoke();

    /// <summary>
    /// Event registered on enemy states.
    /// </summary>
    public event Action AnyDamageOnEnemy;

    protected virtual void OnDie() => Die?.Invoke();

    /// <summary>
    /// Event registered on classes with stats class.
    /// </summary>
    public event Action Die;
}
