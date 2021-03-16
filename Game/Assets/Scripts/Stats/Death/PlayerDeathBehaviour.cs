using UnityEngine;
using System;

/// <summary>
/// Handles player character's death.
/// </summary>
public class PlayerDeathBehaviour : DeathBehaviour
{
    public bool IsAlive { get; private set; }

    private void Start()
    {
        IsAlive = true;
        OnPlayerSpawn();
    }

    public override void Die()
    {
        if (IsAlive)
        {
            Debug.Log("Player died");
            OnPlayerDied();
            IsAlive = false;
        }
    }

    protected virtual void OnPlayerDied() => PlayerDied?.Invoke();
    protected virtual void OnPlayerSpawn() => PlayerSpawn?.Invoke();

    /// <summary>
    /// Event registered on                  .
    /// </summary>
    public event Action PlayerDied;
    /// <summary>
    /// Event registered on                     .
    /// </summary>
    public event Action PlayerSpawn;
}
