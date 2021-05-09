using UnityEngine;
using System;

/// <summary>
/// Class responsible for trigger actions on animation events.
/// </summary>
public class EnemyAnimationEvents : MonoBehaviour
{
    /// <summary>
    /// Invokes Hit event.
    /// </summary>
    protected virtual void OnHit() => Hit?.Invoke();

    /// <summary>
    /// Invokes AgentCanMove event.
    /// </summary>
    protected virtual void OnAgentCanMove() => AgentCanMove?.Invoke();

    /// <summary>
    /// Event registered on Enemy agressive state.
    /// </summary>
    public event Action Hit;

    /// <summary>
    /// Event registered on Enemy agressive state.
    /// </summary>
    public event Action AgentCanMove;
}
