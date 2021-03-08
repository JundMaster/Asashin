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
}
