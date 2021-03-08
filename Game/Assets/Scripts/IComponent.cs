/// <summary>
/// Interface for components.
/// </summary>
public interface IComponent
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
