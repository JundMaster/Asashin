/// <summary>
/// Interface for components.
/// </summary>
public interface IAction
{
    /// <summary>
    /// Runs on update.
    /// </summary>
    void ComponentUpdate();

    /// <summary>
    /// Runs on fixedUpdated.
    /// </summary>
    void ComponentFixedUpdate();

    /// <summary>
    /// Method to know if the player is performing a certain action.
    /// </summary>
    /// <returns>Returns true if player is performing an action.</returns>
    bool Performing { get; }
}
