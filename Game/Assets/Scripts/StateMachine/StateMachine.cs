using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class responsible for handling states.
/// </summary>
public class StateMachine
{
    private readonly IEnumerable<IState> states;

    private IState currentState;

    /// <summary>
    /// Constructor for StateMachine.
    /// </summary>
    /// <param name="states">States to intialize.</param>
    /// <param name="obj">Parent object of this state machine.</param>
    public StateMachine (IEnumerable<IState> states, object obj)
    {
        this.states = states;

        currentState = states.First();

        // Initializes and starts all states
        foreach (IState state in states)
        {
            state.Initialize(obj);
            state.Start();
        }
    }

    /// <summary>
    /// Runs on fixed update.
    /// Runs current state's fixed update.
    /// </summary>
    public void FixedUpdate()
    {
        if (currentState == null) currentState = states.First();

        IState nextState = currentState?.FixedUpdate();

        if (nextState != null &&
            nextState != currentState)
        {
            SwitchToNewState(nextState);
        }
    }

    /// <summary>
    /// Switches to a new state, triggers OnExit from previous state and
    /// OnEnter from the next state.
    /// </summary>
    /// <param name="nextState">IState to switch to.</param>
    private void SwitchToNewState(IState nextState)
    {
        currentState.OnExit();
        currentState = nextState;
        currentState.OnEnter();
    }
}
