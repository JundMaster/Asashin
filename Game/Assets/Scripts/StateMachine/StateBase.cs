using UnityEngine;

/// <summary>
/// Abstract Scriptable base class of all enemy states.
/// </summary>
public abstract class StateBase: ScriptableObject, IState
{
    /// <summary>
    /// Method that defines what happens when this state is initialized.
    /// </summary>
    /// <param name="obj">Parent object of this state.</param>
    public abstract void Initialize(object obj);

    /// <summary>
    /// Runs once on start.
    /// </summary>
    public abstract void Start();

    /// <summary>
    /// Runs every time the state machine enters this state.
    /// Finds playerTarget in case it's null.
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// Runs every time the state machine leaves this state.
    /// Sets player's last known position.
    /// </summary>
    public abstract void OnExit();

    /// <summary>
    /// Method that defines what this state does. Runs on update.
    /// </summary>
    /// <returns>Returns an IState.</returns>
    public abstract IState Update();
}
