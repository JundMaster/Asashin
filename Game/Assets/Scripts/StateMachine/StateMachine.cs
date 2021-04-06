using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class responsible for handling states.
/// </summary>
public class StateMachine
{
    private readonly IEnumerable<IState> states;

    private IState currentState;

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

    private void SwitchToNewState(IState nextState)
    {
        currentState.OnExit();
        currentState = nextState;
        currentState.OnEnter();
    }
}
