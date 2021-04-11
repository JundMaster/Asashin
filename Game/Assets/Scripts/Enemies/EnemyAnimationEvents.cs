using UnityEngine;
using System;

/// <summary>
/// Class responsible for trigger actions on animation events.
/// </summary>
public class EnemyAnimationEvents : MonoBehaviour
{
    /// <summary>
    /// Invoeks Hit event.
    /// </summary>
    protected virtual void OnHit() => Hit?.Invoke();

    /// <summary>
    /// Event registered on Enemy class.
    /// </summary>
    public event Action Hit;
}
