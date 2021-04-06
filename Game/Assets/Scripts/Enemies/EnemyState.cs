using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Abstract Scriptable object responsible for controlling enemy states.
/// </summary>
public abstract class EnemyState: ScriptableObject, IState
{
    protected Enemy enemy;
    protected Transform playerTarget;
    protected Transform myTarget;
    protected NavMeshAgent agent;

    /// <summary>
    /// Method that defines what happens when this state is initialized.
    /// </summary>
    /// <param name="obj">Parent object of this state.</param>
    public virtual void Initialize(object obj)
    {
        enemy = obj as Enemy;
        myTarget = enemy.MyTarget;
        playerTarget = enemy.PlayerTarget;
        agent = enemy.Agent;
    }

    /// <summary>
    /// Runs once on start.
    /// </summary>
    public virtual void Start()
    {
        // Left blank on purpose
    }

    /// <summary>
    /// Runs every time the state machine enters this state.
    /// </summary>
    public virtual void OnEnter()
    {
        if (playerTarget == null) playerTarget = enemy.PlayerTarget;
    }

    /// <summary>
    /// Runs every time the state machine leaves this state.
    /// </summary>
    public virtual void OnExit()
    {
        // Left blank on purpose
    }

    /// <summary>
    /// Method that defines what this state does. Runs on fixed update.
    /// </summary>
    /// <returns>Returns an IState.</returns>
    public virtual IState FixedUpdate()
    {
        if (playerTarget == null) playerTarget = enemy.PlayerTarget;
        return null;
    }

}
