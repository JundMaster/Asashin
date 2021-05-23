using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class responsible for handling states.
/// </summary>
public class StateMachine
{
    private readonly IEnumerable<IState> states;
    private readonly object parentObject;
    private IState currentState;

    private readonly float delayToLeaveState = 0.1f;
    private float currentTimerToLeaveState;

    /// <summary>
    /// Constructor for StateMachine.
    /// </summary>
    /// <param name="states">States to intialize.</param>
    /// <param name="parentObject">Parent object of this state machine.</param>
    public StateMachine (IEnumerable<IState> states, object parentObject)
    {
        this.states = states;
        this.parentObject = parentObject;
    }

    /// <summary>
    /// Initializes states.
    /// </summary>
    public void Initialize()
    {
        currentState = states.First();

        // Initializes and starts all states
        foreach (IState state in states)
        {
            state?.Initialize(parentObject);
            state?.Start();
        }

        currentState.OnEnter();
    }

    /// <summary>
    /// Runs on update.
    /// Runs current state's update.
    /// </summary>
    public void Update()
    {
        IState nextState = currentState?.Update();
        
        // If the next state is different than the current one, it changes state
        // Has a small delay to leave the current state
        if (nextState != null &&
            nextState != currentState &&
            Time.time - currentTimerToLeaveState > delayToLeaveState)
        {
            SwitchToNewState(nextState);
        }
    }

    /// <summary>
    /// Switches to a new state.
    /// Triggers current state OnExit, changes current state, triggers
    /// current state OnEnter with property.
    /// </summary>
    /// <param name="nextState">IState to switch to.</param>
    public void SwitchToNewState(IState nextState)
    {
        currentState?.OnExit();
        currentState = nextState;
        currentState?.OnEnter();

        currentTimerToLeaveState = Time.time;
    }
}
