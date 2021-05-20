/// <summary>
/// Interface implemented by base states.
/// </summary>
public interface IState
{
    /// <summary>
    /// Method that defines what happens when this state is initialized.
    /// </summary>
    /// <param name="obj">Parent object of this state.</param>
    void Initialize(object obj);

    /// <summary>
    /// Runs once on start.
    /// </summary>
    void Start();

    /// <summary>
    /// Runs every time the state machine enters this state.
    /// </summary>
    void OnEnter();

    /// <summary>
    /// Runs every time the state machine leaves this state.
    /// </summary>
    void OnExit();

    /// <summary>
    /// Method that defines what this state does. Runs on update.
    /// </summary>
    /// <returns>Returns an IState.</returns>
    IState Update();
}
