using UnityEngine;
using System;

/// <summary>
/// Handles player character's death.
/// </summary>
public class PlayerDeathBehaviour : DeathBehaviour
{
    private bool isAlive;

    private void Start()
    {
        isAlive = true;
    }

    public override void Die()
    {
        // Only happens once after the player reached 0 health.
        if (isAlive)
        {
            Debug.Log("Player died");
            OnPlayerDied();
            isAlive = false;
        }
    }

    protected virtual void OnPlayerDied() => PlayerDied?.Invoke();

    /// <summary>
    /// Event registered on PlayerAnimations.
    /// </summary>
    public event Action PlayerDied;
}
