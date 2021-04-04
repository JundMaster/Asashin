using UnityEngine;

/// <summary>
/// Abstract Scriptable object responsible for controlling enemy states.
/// </summary>
public abstract class EnemyState : ScriptableObject, IEnemyState
{
    /// <summary>
    /// Method that defines what happens when this state is initialized.
    /// </summary>
    /// <param name="enemy">Enemy parent of this state.</param>
    public abstract void Initialize(Enemy enemy);

    /// <summary>
    /// Method that defines what this state does.
    /// </summary>
    /// <param name="enemy">Enemy parent of this state.</param>
    /// <returns>Returns an IEnemyState.</returns>
    public abstract IEnemyState Execute(Enemy enemy);
}
