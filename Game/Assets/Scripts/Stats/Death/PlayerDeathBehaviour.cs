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

    /// <summary>
    /// This method happens when the player's health reaches 0.
    /// Triggers an event.
    /// </summary>
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
    /// Event registered on PlayerAnimations and PlayerInputCustom.
    /// </summary>
    public event Action PlayerDied;
}
