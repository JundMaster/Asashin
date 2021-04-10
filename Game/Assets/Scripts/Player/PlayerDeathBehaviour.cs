using UnityEngine;
using System;

/// <summary>
/// Handles player character's death.
/// </summary>
public class PlayerDeathBehaviour: MonoBehaviour
{
    private PlayerStats stats;
    private bool isAlive;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        stats.Die += Die;
    }

    private void OnDisable()
    {
        stats.Die -= Die;
    }

    private void Start()
    {
        isAlive = true;
    }

    /// <summary>
    /// This method happens when the player's health reaches 0.
    /// Triggers an event.
    /// </summary>
    public void Die()
    {
        // Only happens once after the player reached 0 health.
        if (isAlive)
        {
            OnPlayerDied();
            isAlive = false;
        }
    }

    protected virtual void OnPlayerDied() => PlayerDied?.Invoke();

    /// <summary>
    /// Event registered on PlayerAnimations and PlayerInputCustom and Check.
    /// </summary>
    public event Action PlayerDied;
}
